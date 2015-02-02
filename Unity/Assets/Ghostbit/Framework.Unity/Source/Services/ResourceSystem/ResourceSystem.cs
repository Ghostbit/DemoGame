using Ghostbit.Framework.Core.Utils;
using Ghostbit.Framework.Unity.Models;
using NLog;
using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Ghostbit.Framework.Unity.Services
{
    public class ResourceSystem : IResourceSystem
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        [Inject]
        public ResourceManifest Manifest { get; set; }

        private ResourceCache cache;

        public ResourceSystem()
        {
            Service.Set<ResourceSystem>(this);
        }

        public void Init()
        {
            logger.Info("Init");
            cache = new ResourceCache();
        }

        public IResourceRequest LoadAsync<TAsset>(string path)
            where TAsset : UnityEngine.Object
        {
            ResourceManifest.ResourceEntry entry = null;
            if (!Manifest.resources.TryGetValue(path, out entry))
            {
                throw new Exception("path was not found in manifest entries: " + path);
            }

            if(!Uri.IsWellFormedUriString(entry.uri, UriKind.RelativeOrAbsolute))
            {
                throw new ResourceException("Uri is not well formed: " + entry.uri);
            }

            return LoadAsyncUri<TAsset>(new Uri(entry.uri));
        }

        public IResourceRequest LoadAsyncUri<TAsset>(Uri uri)
            where TAsset : UnityEngine.Object
        {
            string path = uri.AbsolutePath;
            if (cache.IsCached(path))
            {
                return new CachedResourceRequest(path, cache.GetResource<TAsset>(path));
            }
            else
            {
                cache.Reserve(path);
                IResourceRequest request = ResourceLoader<TAsset>.LoadAsyncUri(uri);
                GhostbitRoot.StartRadicalCoroutine(WaitForLoadToFinish(request));
                return request;
            }
        }

        public IEnumerator WaitForLoadToFinish(IResourceRequest request)
        {
            yield return request;
            cache.Add(request.path, request.asset);
        }
    }

    public class ResourceException : Exception, ISerializable
    {
        public ResourceException(string message)
            : base(message)
        {
        }
    }
}
