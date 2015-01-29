using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Framework.Unity.Services
{
    public interface IResourceSystem
    {
        void Init();

        ResourceLoadRequest LoadAsync<TAsset>(string path)
            where TAsset : UnityEngine.Object;

        ResourceLoadRequest LoadAsyncUri<TAsset>(string uri)
            where TAsset : UnityEngine.Object;
    }
}
