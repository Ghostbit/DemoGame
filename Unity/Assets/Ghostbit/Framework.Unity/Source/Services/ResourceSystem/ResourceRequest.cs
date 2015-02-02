using Ghostbit.Framework.Unity.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Framework.Unity.Services
{
    public interface IResourceRequest
    {
        string path { get; }
        bool allowSceneActivation { get; set; }
        int priority { get; set; }
        float progress { get; }
        UnityEngine.Object asset { get; }
        bool finished { get; }
    }

    internal class CachedResourceRequest : CoroutineReturn, IResourceRequest
    {
        private string _path;
        private UnityEngine.Object _asset;

        public CachedResourceRequest(string path, UnityEngine.Object asset)
        {
            _path = path;
            _asset = asset;
        }

        public string path { get { return _path; } }

        public bool allowSceneActivation
        {
            get { return false; }
            set {  }
        }

        public int priority
        {
            get { return 0; }
            set {  }
        }

        public float progress { get { return 1f; } }

        public UnityEngine.Object asset { get { return _asset; } }

        public override bool finished
        {
            get { return true; }
            set { base.finished = value; }
        }
    }

    internal abstract class ResourceRequestAsync : CoroutineReturn, IResourceRequest
    {
        private string _path;
        private AsyncOperation op;

        public ResourceRequestAsync(string path, AsyncOperation op)
        {
            _path = path;
            this.op = op;
        }

        public string path { get { return _path; } }

        public bool allowSceneActivation
        {
            get { return op.allowSceneActivation; }
            set { op.allowSceneActivation = value; }
        }

        public int priority
        {
            get { return op.priority; }
            set { op.priority = value; }
        }

        public float progress { get { return op.progress; } }

        public abstract UnityEngine.Object asset { get; }

        public override bool finished
        {
            get { return op.isDone; }
            set { base.finished = value; }
        }
    }

    internal sealed class ResourceRequest : ResourceRequestAsync
    {
        private UnityEngine.ResourceRequest request;

        public ResourceRequest(string path, UnityEngine.ResourceRequest request) :
            base(path, request)
        {
            this.request = request;
        }

        public override UnityEngine.Object asset
        {
            get { return request.asset; }
        }
    }

    //public sealed class AssetBundleRequest : ResourceRequestAsync
    //{
    //    private UnityEngine.AssetBundleRequest request;

    //    internal AssetBundleRequest(UnityEngine.AssetBundleRequest request) :
    //        base(request)
    //    {
    //        this.request = request;
    //    }

    //    public override UnityEngine.Object asset
    //    {
    //        get { return request.asset; }
    //    }
    //}
}
