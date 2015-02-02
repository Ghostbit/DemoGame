using Ghostbit.Framework.Core.Utils;
using Ghostbit.Framework.Unity.Models;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Framework.Unity.Services
{
    internal static class ResourceLoader<TAsset>
        where TAsset : UnityEngine.Object
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static IResourceRequest LoadAsyncUri(Uri uri)
        {
            NameValueCollection nvp = QueryParams.ParseQueryString(uri.Query);
            if (nvp["type"] != null && nvp["type"] != typeof(TAsset).FullName)
            {
                throw new ResourceException("The requested asset type '" + typeof(TAsset).FullName +
                    "' does not match the type '" + nvp["type"] + "' specified by the uri.");
            }

            IResourceRequest request = null;
            switch (uri.Scheme)
            {
                case ResourceManifest.URI_SCHEME_RESOURCES:
                    request = LoadFromResources(uri);
                    break;
                case ResourceManifest.URI_SCHEME_BUNDLE:
                    request = LoadFromBundle(uri);
                    break;
                case ResourceManifest.URI_SCHEME_DEV_BUNDLE:
                    request = LoadFromDevBundle(uri);
                    break;
                default:
                    throw new ResourceException("Invalid resource scheme: " + uri.Scheme);
            }

            return request;
        }

        private static IResourceRequest LoadFromResources(Uri uri)
        {
            string path = uri.AbsolutePath;
            logger.Trace("Resources.LoadAsync<{0}>({1})", typeof(TAsset).Name, path);
            return new ResourceRequest(path, Resources.LoadAsync<TAsset>(path));
        }

        private static IResourceRequest LoadFromBundle(Uri uri)
        {
            return null;
        }

        private static IResourceRequest LoadFromDevBundle(Uri uri)
        {
            return null;
        }
    }
}
