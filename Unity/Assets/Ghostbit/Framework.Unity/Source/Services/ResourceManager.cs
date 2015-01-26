using Ghostbit.Framework.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Framework.Unity.Services
{
    public class ResourceManager
    {
        private Dictionary<string, AssetBundle> loadedBundles;

        public ResourceManager()
        {
            Service.Set<ResourceManager>(this);
            loadedBundles = new Dictionary<string, AssetBundle>();
        }
    }
}
