using Ghostbit.Framework.Core.Utils;
using Ghostbit.Framework.Unity.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using UnityEngine;

namespace Ghostbit.Framework.Unity.Services
{
    public class ResourceSystem : IResourceSystem
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        [Inject]
        public ResourceManifest Manifest { get; set; }

        public ResourceSystem()
        {
            Service.Set<ResourceSystem>(this);
        }

        public void Init()
        {
            
        }

        public ResourceLoadRequest LoadAsync<TAsset>(string path)
            where TAsset : UnityEngine.Object
        {
            ResourceManifest.ResourceEntry entry = null;
            if(!Manifest.resources.TryGetValue(path, out entry))
                throw new Exception("path was not found in manifest entries: " + path);

            return LoadAsyncUri<TAsset>(entry.uri);
        }

        public ResourceLoadRequest LoadAsyncUri<TAsset>(string uri)
            where TAsset : UnityEngine.Object
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
            {
                throw new Exception("Invalid uri: " + uri);
            }

            // TODO: utility for parsing query params
            Uri _uri = new Uri(uri);

            NameValueCollection nvp = QueryParams.ParseQueryString(_uri.Query);
            if (nvp["type"] != null && nvp["type"] != typeof(TAsset).FullName)
            {
                throw new Exception("The requested asset type '" + typeof(TAsset).FullName +
                    "' does not match the type '" + nvp["type"] + "' specified by the uri.");
            }

            ResourceLoadRequest request = null;
            switch (_uri.Scheme)
            {
                case ResourceManifest.URI_SCHEME_RESOURCES:
                    string path = _uri.LocalPath;
                    if (path.StartsWith("/"))
                        path = path.Substring(1);
                    logger.Trace("Resources.LoadAsyn<{0}>({1})", typeof(TAsset).Name, path);
                    request = new ResourceRequest(Resources.LoadAsync<TAsset>(path));
                    break;
                case ResourceManifest.URI_SCHEME_BUNDLE:
                    // TODO
                    break;
                case ResourceManifest.URI_SCHEME_DEV_BUNDLE:
                    // TODO
                    break;
                default:
                    throw new Exception("Invalid resource scheme: " + _uri.Scheme);
            }

            return request;
        }
    }
}
