using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ghostbit.Framework.Unity.Services
{
    public interface IResourceCache
    {
        bool IsCached(string path);
        bool IsReserved(string path);
        void Reserve(string path);
        void Add(string path, UnityEngine.Object resource);
        void Remove(string path);
        void RemoveAll();
        
        UnityEngine.Object GetResource(string path);

        TAsset GetResource<TAsset>(string path)
            where TAsset : UnityEngine.Object;
    }

    public class ResourceCache : IResourceCache
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public Dictionary<string, UnityEngine.Object> cache;

        public ResourceCache()
        {
            cache = new Dictionary<string, UnityEngine.Object>();
        }

        public bool IsCached(string path)
        {
            return cache.ContainsKey(path);
        }

        public bool IsReserved(string path)
        {
            if (IsCached(path) && GetResource(path) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reserve(string path)
        {
            logger.Trace("Reserve: {0}", path);
            if(IsCached(path))
            {
                throw new ResourceException("Cannot reserve path that is already cached or reserved: " + path);
            }

            cache.Add(path, null);
        }

        public void Add(string path, UnityEngine.Object resource)
        {
            logger.Trace("Add: {0}, {1}", path, resource);
            if (resource == null)
            {
                throw new ResourceException("Cannot cache null resource for path: " + path);
            }

            if(IsReserved(path))
            {
                cache[path] = resource;
            }
            else if (IsCached(path))
            {
                throw new ResourceException("Cannot add a resource that is already cached: " + path);
            }
            else
            {
                cache.Add(path, resource);
            }
        }

        public void Remove(string path)
        {
            logger.Trace("Remove: {0}", path);
            if(!IsCached(path))
            {
                throw new ResourceException("Cannot remove un-cached resource for path: " + path);
            }

            cache.Remove(path);
        }

        public void RemoveAll()
        {
            logger.Trace("RemoveAll");
            cache.Clear();
        }

        public UnityEngine.Object GetResource(string path)
        {
            UnityEngine.Object resource = null;
            cache.TryGetValue(path, out resource);
            logger.Trace("GetResource {0} = {1}", path, resource);
            return resource;
        }

        public TAsset GetResource<TAsset>(string path) 
            where TAsset : UnityEngine.Object
        {
            return GetResource(path) as TAsset;
        }
    }
}
