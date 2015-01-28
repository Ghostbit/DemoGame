using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostbit.Framework.Unity.Models
{
    [Serializable]
    public class ResourceManifest
    {
        public const string URI_SCHEME_RESOURCES = "resources";
        public const string URI_SCHEME_BUNDLE = "bundle";
        public const string SCHEME_DEV_BUNDLE = "devbundle";

        [Serializable]
        public class MetaData
        {
            public Int64 version = -1;
        }

        [Serializable]
        public class BundleEntry
        {
            public string id;
            public string uri;
        }

        [Serializable]
        public class ResourceEntry
        {
            public string id;
            public string uri;
        }

        public MetaData metaData = new MetaData();
        public Dictionary<string, BundleEntry> bundles = new Dictionary<string,BundleEntry>();
        public Dictionary<string, ResourceEntry> resources = new Dictionary<string,ResourceEntry>();
    }
}
