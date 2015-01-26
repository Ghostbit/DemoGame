using System;
using System.Collections.Generic;

namespace Ghostbit.Framework.Core.Utils
{
    public static class Service
    {
        [ThreadStatic]
        private static List<IServiceWrapper> wrappers;

        public static void Set<T>(T instance) where T : class
        {
            if (ServiceWrapper<T>.instance != null)
            {
                throw new Exception("An instance of this service class was already set.");
            }

            ServiceWrapper<T>.instance = instance;

            if (wrappers == null)
            {
                wrappers = new List<IServiceWrapper>();
            }

            wrappers.Add(new ServiceWrapper<T>());
        }

        public static T Get<T>() where T : class
        {
            return ServiceWrapper<T>.instance;
        }

        public static bool IsSet<T>() where T : class
        {
            return ServiceWrapper<T>.instance != null;
        }

        public static void Reset()
        {
            foreach(var wrapper in wrappers)
            {
                wrapper.Unset();
            }
            wrappers = null;
        }

        public static Dictionary<Type, object> GetServiceMapping()
        {
            var mappings = new Dictionary<Type, object>();
            foreach(var wrapper in wrappers)
            {
                mappings.Add(wrapper.Type, wrapper.AsObject);
            }
            return mappings;
        }
    }

    internal class ServiceWrapper<T> : IServiceWrapper where T : class
    {
        public static T instance = null;
    
        public void Unset()
        {
            ServiceWrapper<T>.instance = null;
        }

        public Type Type { get { return typeof(T); } }
        public object AsObject { get { return instance; } }
    }
    
    internal interface IServiceWrapper
    {
        void Unset();
        Type Type { get; }
        object AsObject { get; }
    }
}