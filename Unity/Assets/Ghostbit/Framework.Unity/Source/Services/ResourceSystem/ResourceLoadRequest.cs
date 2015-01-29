using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Framework.Unity.Services
{
    public abstract class ResourceLoadRequest
    {
        private AsyncOperation op;

        internal ResourceLoadRequest(AsyncOperation op)
        {
            this.op = op;
        }

        public bool allowSceneActivation
        {
            get { return op.allowSceneActivation; }
            set { op.allowSceneActivation = value; }
        }

        public bool isDone { get { return op.isDone; } }

        public int priority
        {
            get { return op.priority; }
            set { op.priority = value; }
        }

        public float progress { get { return op.progress; } }

        public abstract UnityEngine.Object asset { get; }
    }

    public sealed class ResourceRequest : ResourceLoadRequest
    {
        private UnityEngine.ResourceRequest request;

        internal ResourceRequest(UnityEngine.ResourceRequest request) :
            base(request)
        {
            this.request = request;
        }

        public override UnityEngine.Object asset
        {
            get { return request.asset; }
        }
    }

    public sealed class AssetBundleRequest : ResourceLoadRequest
    {
        private UnityEngine.AssetBundleRequest request;

        internal AssetBundleRequest(UnityEngine.AssetBundleRequest request) :
            base(request)
        {
            this.request = request;
        }

        public override UnityEngine.Object asset
        {
            get { return request.asset; }
        }
    }
}
