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

        IResourceRequest LoadAsync<TAsset>(string path)
            where TAsset : UnityEngine.Object;

        IResourceRequest LoadAsyncUri<TAsset>(Uri uri)
            where TAsset : UnityEngine.Object;
    }
}
