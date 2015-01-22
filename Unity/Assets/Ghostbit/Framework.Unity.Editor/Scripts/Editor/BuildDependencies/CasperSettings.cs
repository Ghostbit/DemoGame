using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Ghostbit.Framework.Unity.Editor.Casper
{
    [Serializable]
    public class CasperSettings
    {
        public string frameworkPath = "";
        public DependencySettings[] dependencies;
        
    }

    [Serializable]
    public class DependencySettings
    {
        public string key;
        public bool enabled;
    }
}