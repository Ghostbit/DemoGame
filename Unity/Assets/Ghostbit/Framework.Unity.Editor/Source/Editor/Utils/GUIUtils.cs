using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Framework.Unity.Editor.Utils
{
    public static class GUIUtils
    {
        public static void MakeCommandButton(string cmd, string label, Action callback)
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
