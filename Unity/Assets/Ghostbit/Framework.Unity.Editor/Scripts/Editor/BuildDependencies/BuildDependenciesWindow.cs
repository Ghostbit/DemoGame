using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghostbit.Framework.Unity.Editor.Utils;
using System.Xml.Serialization;

namespace Ghostbit.Framework.Unity.Editor.Casper
{
    public class CasperWindow : EditorWindow
    {
        public enum ProcessDependenciesAction
        {
            Pull,
            Push
        }

        [Serializable]
        public class Dependency
        {
            public string name;
            public string sourcePath;
            public string destPath;
            public bool enabled;
        }

        private const string SETTINGS_PATH = "ProjectSettings/Casper.xml";

        [MenuItem("Window/Ghostbit/Casper")]
        public static void ShowWindow()
        {
            GetWindow<CasperWindow>();
        }

        [MenuItem("Ghostbit/Casper/Pull")]
        public static void PullDependencies()
        {

            if (EditorUtility.DisplayDialog("Ghostbit Casper: Pull Confirmation",
                                            "Are you sure you want to pull? Any uncommitted local changes will be lost.",
                                            "Yes", "No"))
            {
                var window = GetWindow<CasperWindow>();
                window.ProcessDependencies(ProcessDependenciesAction.Pull);
                AssetDatabase.Refresh(ImportAssetOptions.Default);
            }
        }

        [MenuItem("Ghostbit/Casper/Push")]
        public static void PushDependencies()
        {
            if (EditorUtility.DisplayDialog("Ghostbit Casper: Push Confirmation",
                                            "Are you sure you want to push? Any uncommitted changes to the Framework directory will be lost.",
                                            "Yes", "No"))
            {
                var window = GetWindow<CasperWindow>();
                window.ProcessDependencies(ProcessDependenciesAction.Push);
            }
        }

        [MenuItem("Ghostbit/Casper/Reset")]
        public static void ResetCasper()
        {
            GetWindow<CasperWindow>().Reset();
        }

        private string frameworkPath = "";
        private Dictionary<string, Dependency> dependencies;

        public CasperWindow()
        {

        }

        private void Init()
        {
            if (string.IsNullOrEmpty(frameworkPath))
            {
                DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
                frameworkPath = di.Parent.FullName + @"\Framework";
                frameworkPath = frameworkPath.Replace(@"\", "/");
            }

            if (dependencies == null)
            {
                dependencies = new Dictionary<string, Dependency>();
                AddDependency("UnityVS", "Framework.Unity.Editor/Assets/UnityVS", "Assets/UnityVS/", false);
                AddDependency("StrangeIoC", "libs/strangeioc/StrangeIoC/scripts/", "Assets/Ghostbit/StrangeIoC/Source/", false);
                AddDependency("Ash.Net Core", "libs/Ash.Net/Ash/Core/", "Assets/Ghostbit/Ash.Net/Source/Core/", false);
                AddDependency("Ash.Net Tools", "libs/Ash.Net/Ash/Tools/", "Assets/Ghostbit/Ash.Net/Source/Tools/", false);
                AddDependency("Tweaker.Core", "libs/TweakerUnity/Tweaker/Tweaker.Core/src/", "Assets/Ghostbit/Tweaker/Source/", false);
                AddDependency("TweakerUnity", "libs/TweakerUnity/Assets/Ghostbit/TweakerUnity/", "Assets/Ghostbit/TweakerUnity/", false);
                AddDependency("Framework.Core", "Framework.Core/Source", "Assets/Ghostbit/Framework.Core/Source/", false);
                AddDependency("Framework.Unity", "Framework.Unity/Assets/Ghostbit/Framework.Unity/", "Assets/Ghostbit/Framework.Unity/", false);
                AddDependency("NLog", "Framework.Unity/Assets/Ghostbit/NLog/", "Assets/Ghostbit/NLog/", false);

                AddDependency("Framework.Unity.Editor", "Framework.Unity.Editor/Assets/Ghostbit/Framework.Unity.Editor", "Assets/Ghostbit/Framework.Unity.Editor/", false);

                if (!ValidateDependencies())
                {
                    EditorUtility.DisplayDialog("Validate Dependencies Failed",
                                                "Some dependencies had errors and have been disabled.",
                                                "Okay");
                }
            }
        }

        private void Reset()
        {
            frameworkPath = "";
            dependencies = null;
        }

        private void OnEnable()
        {
            Init();
            LoadSettings();
        }

        private void OnDisable()
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            CasperSettings settings = new CasperSettings();

            settings.frameworkPath = frameworkPath;
            settings.dependencies = new DependencySettings[dependencies.Count];
            string[] keys = new string[dependencies.Count];
            dependencies.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; ++i)
            {
                Dependency dep = dependencies[keys[i]];
                DependencySettings depSettings = new DependencySettings();
                depSettings.key = keys[i];
                depSettings.enabled = dep.enabled;
                settings.dependencies[i] = depSettings;
            }

            XmlSerializer ser = new XmlSerializer(typeof(CasperSettings));
            TextWriter writer = new StreamWriter(SETTINGS_PATH);
            ser.Serialize(writer, settings);
            writer.Close();
        }

        private void LoadSettings()
        {
            if (new FileInfo(SETTINGS_PATH).Exists)
            {
                CasperSettings settings = null;
                XmlSerializer ser = new XmlSerializer(typeof(CasperSettings));
                StreamReader reader = new StreamReader(SETTINGS_PATH);
                settings = (CasperSettings)ser.Deserialize(reader);
                reader.Close();

                frameworkPath = settings.frameworkPath;
                settings.dependencies.ToList().ForEach(UpdateDependencyFromSettings);
            }
        }

        private void UpdateDependencyFromSettings(DependencySettings depSettings)
        {
            Dependency dep;
            dependencies.TryGetValue(depSettings.key, out dep);
            if (dep == null)
            {
                Debug.Log("Could not find dependency using key: " + depSettings.key);
            }
            else
            {
                dep.enabled = depSettings.enabled;
            }
        }

        public void ProcessDependencies(ProcessDependenciesAction action)
        {
            Debug.Log("ProcessDependencies " + action.ToString());
            ValidateDependencies();
            foreach (Dependency dep in dependencies.Values)
            {
                if (dep.enabled)
                {
                    string fromPath = "";
                    string toPath = "";
                    string fromRootPath = "";
                    string toRootPath = "";
                    switch (action)
                    {
                        case ProcessDependenciesAction.Pull:
                            fromPath = dep.sourcePath;
                            toPath = dep.destPath;
                            fromRootPath = frameworkPath;
                            toRootPath = Directory.GetCurrentDirectory();
                            break;
                        case ProcessDependenciesAction.Push:
                            fromPath = dep.destPath;
                            toPath = dep.sourcePath;
                            fromRootPath = Directory.GetCurrentDirectory();
                            toRootPath = frameworkPath;
                            break;
                    }

                    //Debug.Log("fromPath " + fromPath + " toPath " + toPath);

                    string dstRootPath = Path.Combine(toRootPath, toPath);
                    DirectoryInfo dstDir = new DirectoryInfo(dstRootPath);
                    if (!dstDir.Exists)
                    {
                        dstDir.Create();
                    }

                    DirectoryInfo di = new DirectoryInfo(Path.Combine(fromRootPath, fromPath));
                    FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories);
                    foreach (FileInfo fi in files)
                    {
                        //Debug.Log("Found dependency file (" + fi.Extension + "):" + fi.FullName);
                        if (fi.Extension != ".meta")
                        {
                            string relativePath = PathUtil.MakeRelativePath(Path.Combine(fromRootPath, fromPath), fi.FullName);
                            //Debug.Log("relativePath = " + relativePath);
                            string srcPath = fi.FullName.Replace(@"\", "/");
                            string dstPath = Path.Combine(toRootPath, toPath);
                            dstPath = Path.Combine(dstPath, relativePath);
                            dstPath = dstPath.Replace(@"\", "/");
                            FileInfo dstFi = new FileInfo(dstPath);
                            if (!dstFi.Directory.Exists)
                            {
                                dstFi.Directory.Create();
                            }

                            Debug.Log(action.ToString() + " " + srcPath + " to " + dstPath);
                            FileUtil.ReplaceFile(srcPath, dstPath);
                        }
                    }
                }
            }
        }

        private void AddDependency(string name, string sourcePath, string destPath, bool enabled)
        {
            if (string.IsNullOrEmpty(name) || dependencies.ContainsKey(name))
            {
                EditorUtility.DisplayDialog("Invalid Dependency Name",
                                            "The dependency name '" + name + "' is invalid or already used. New dependency will not be added.",
                                            "Okay");
                return;
            }

            Dependency dep = new Dependency();
            dep.name = name;
            dep.sourcePath = sourcePath;
            dep.destPath = destPath;
            dep.enabled = enabled;
            dependencies.Add(name, dep);
        }

        private bool ValidateDependencies()
        {
            if (string.IsNullOrEmpty(frameworkPath))
            {
                EditorUtility.DisplayDialog("Ghostbit Casper: Framework Path Invalid",
                                            "The framework path is not set.",
                                            "Okay");
                return false;
            }

            if (!Directory.Exists(frameworkPath))
            {
                EditorUtility.DisplayDialog("Ghostbit Casper: Framework Path Does Not Exist",
                                            "The framework path does not exist.",
                                            "Okay");
                return false;
            }

            bool error = false;

            foreach (Dependency dep in dependencies.Values)
            {
                error = !ValidateDependency(dep);
            }

            return !error;
        }

        private bool ValidateDependency(Dependency dep)
        {
            //Debug.Log("ValidateDependency...");
            //Debug.Log("Directory.Exists(" + Path.Combine(frameworkPath, dep.sourcePath) + ") = " + Directory.Exists(Path.Combine(frameworkPath, dep.sourcePath)));
            //Debug.Log("Uri.IsWellFormedUriString(dep.sourcePath, UriKind.RelativeOrAbsolute) = " + Uri.IsWellFormedUriString(dep.sourcePath, UriKind.RelativeOrAbsolute));

            string path = Path.Combine(frameworkPath, dep.sourcePath);
            if (string.IsNullOrEmpty(dep.sourcePath) ||
                !Uri.IsWellFormedUriString(dep.sourcePath, UriKind.Relative) ||
                !Directory.Exists(path))
            {
                EditorUtility.DisplayDialog("Ghostbit Casper: Invalid Dependency sourcePath",
                                            "The path '" + path + "' is invalid. New dependency will not be added.",
                                            "Okay");
                dep.enabled = false;
                return false;
            }

            if (!string.IsNullOrEmpty(dep.destPath) &&
                !Uri.IsWellFormedUriString(dep.destPath, UriKind.RelativeOrAbsolute))
            {
                EditorUtility.DisplayDialog("Ghostbit Casper: Invalid Dependency destPath",
                                            "The path '" + dep.destPath + "' is invalid. New dependency will not be added.",
                                            "Okay");
                dep.enabled = false;
                return false;
            }

            return true;
        }

        private void OnGUI()
        {
            GUILayout.Label("Options");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Framework Path"))
            {
                string newPath = EditorUtility.OpenFolderPanel("Set Root Path", "", "");
                if (newPath != frameworkPath)
                    SaveSettings();

            }
            GUILayout.Label(frameworkPath);
            GUILayout.EndHorizontal();

            GUILayout.Label("Commands");
            MakeCommand("Pull", "Pull all dependencies into current project.", PullDependencies);
            MakeCommand("Push", "Push all dependencies back to where they originated from.", PushDependencies);

            GUILayout.Label("Dependencies");
            if (dependencies != null)
            {
                foreach (Dependency dep in dependencies.Values)
                {
                    MakeDependency(dep);
                }
            }
        }

        private void MakeDependency(Dependency dep)
        {
            bool enabled = EditorGUILayout.Toggle(dep.name, dep.enabled);
            if (enabled != dep.enabled)
            {
                dep.enabled = enabled;
                if (dep.enabled)
                {
                    ValidateDependency(dep);
                }
                SaveSettings();
            }
        }

        private void MakeCommand(string cmd, string label, Action callback)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(cmd))
            {
                callback();
            }
            GUILayout.Label(label);
            GUILayout.EndHorizontal();
        }
    }
}