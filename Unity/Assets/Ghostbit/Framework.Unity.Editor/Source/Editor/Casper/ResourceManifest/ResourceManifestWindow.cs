using Ghostbit.Framework.Unity.Editor.Utils;
using Ghostbit.Framework.Unity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Ghostbit.Framework.Unity.Editor.Casper
{
    public class ResourceManifestWindow : EditorWindow
    {
        [MenuItem("Window/Ghostbit/Casper/ResourceManifest")]
        public static void ShowWindow()
        {
            GetWindow<ResourceManifestWindow>();
        }

        [MenuItem("Ghostbit/Casper/ResourceManifest/Build")]
        private static void MenuBuildManifest()
        {
            var window = GetWindow<ResourceManifestWindow>();
            window.BuildManifest();
        }

        private DirectoryInfo baseDirectory;
        private static ResourceManifest manifest;

        private void OnGUI()
        {
            GUILayout.Label("Commands");
            GUIUtils.MakeCommandButton("Build Manifest", "", BuildManifest);
        }

        private void BuildManifest()
        {
            Debug.Log("Building Manifest...");

            manifest = new ResourceManifest();

            baseDirectory = new DirectoryInfo("Assets");
            if(!baseDirectory.Exists)
            {
                EditorUtility.DisplayDialog("Resource Manifest: Could not find Assets directory",
                                            "Could not locate the project Assets directory",
                                            "Okay");
                return;
            }
            Log("Base Directory: " + baseDirectory.FullName);

            UpdateMetaData();
            UpdateResourceEntries();
            UpdateBundleEntries();
            SaveManifest();

            EditorUtility.UnloadUnusedAssets();
            AssetDatabase.Refresh(ImportAssetOptions.Default);
        }

        private void UpdateMetaData()
        {
            // TODO: version should increment each time the file is generated.
            // requires storing the current manifest in memory
            // load the manifest if it is not currently in memory
            manifest.metaData.version = -1;
        }

        private void UpdateResourceEntries()
        {
            manifest.resources.Clear();

            // TODO: search without wildcard? ie: "Resources"
            DirectoryInfo[] directories = baseDirectory.GetDirectories("*", SearchOption.AllDirectories);
            foreach (DirectoryInfo di in directories)
            {
                if (di.Name != "Resources")
                    continue;

                Log("Found resouce directory: " + di.FullName);
                FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories);
                foreach (FileInfo fi in files)
                {
                    if (fi.Extension == ".meta")
                        continue;

                    string pathToLoad = PathUtil.MakeRelativePath(baseDirectory.FullName, fi.FullName);
                    UnityObject obj = AssetDatabase.LoadAssetAtPath(pathToLoad, typeof(UnityObject));

                    string path = PathUtil.MakeRelativePath(di.FullName + @"\", fi.FullName);
                    path = Path.ChangeExtension(path.Replace(@"\", "/"), null);

                    //UriBuilder uri = new UriBuilder();
                    //uri.Scheme = ResourceManifest.URI_SCHEME_RESOURCES;
                    //uri.Path = path;
                    //uri.Query = "type=" + obj.GetType().FullName;
                    // ???
                    // Uri builder always includes a host even though one is not needed.
                    // Just build the uri by hand for now.
                    Uri uri = new Uri(string.Format("{0}:{1}?type={2}", ResourceManifest.URI_SCHEME_RESOURCES, path, obj.GetType().FullName));

                    var entry = new ResourceManifest.ResourceEntry();
                    entry.path = path;
                    entry.uri = uri.ToString();
                    manifest.resources.Add(path, entry);
                    Log("New Entry: " + entry.path + " => " + entry.uri);

                }
            }
        }

        private void UpdateBundleEntries()
        {
            manifest.bundles.Clear();
        }

        private void SaveManifest()
        {
            TextWriter writer = new StreamWriter(ResourceManifest.MANIFEST_PATH_ABSOLUTE + ".txt");
            JsonSerializer ser = new JsonSerializer();
            ser.Serialize(writer, manifest);
            writer.Close();
            Debug.Log("ResourceManifest: Manifest saved to " + ResourceManifest.MANIFEST_PATH_ABSOLUTE + ".txt");
        }

        private void Log(string msg)
        {
            Debug.Log("ResourceManifest: " + msg);
        }
    }
}
