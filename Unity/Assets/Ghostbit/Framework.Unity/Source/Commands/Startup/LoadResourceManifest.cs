using Ghostbit.Framework.Unity.Models;
using Ghostbit.Framework.Unity.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Framework.Unity.Commands
{
    public class LoadResourceManifestCmd : LoggedCommand
    {
        [Inject]
        public ResourceManifest Manifest { get; set; }

        [Inject]
        public ResourceSystem ResourceSystem { get; set; }

        protected override void DoExecute()
        {
            TextAsset asset = Resources.Load<TextAsset>(ResourceManifest.MANIFEST_PATH_RELATIVE);
            if (asset == null)
                throw new Exception("A resource manifest was not found at " + ResourceManifest.MANIFEST_PATH_ABSOLUTE);

            ResourceManifest manifest = null;
            JsonSerializer ser = new JsonSerializer();
            StringReader reader = new StringReader(asset.text);
            manifest = (ResourceManifest)ser.Deserialize(reader, typeof(ResourceManifest));
            reader.Close();
            manifest.ShallowCopyTo(Manifest);
            ResourceSystem.Init();
        }
    }
}
