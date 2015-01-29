using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using Ghostbit.Framework.Core.Utils;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using Ghostbit.Framework.Unity.Signals;
using Ghostbit.Framework.Unity.Commands;
using Ghostbit.Framework.Unity.Services;
using Ghostbit.Framework.Unity.Models;

namespace Ghostbit.Framework.Unity
{
    public class GhostbitContext : MVCSContext
    {
        public GhostbitContext() : base()
        {
            
        }

        public GhostbitContext(MonoBehaviour view, bool autoStartup) :
            base(view, autoStartup)
        {
            
        }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
            Service.Set<MVCSContext>(this);
        }

        public override void Launch()
        {
            base.Launch();

            Startup start = (Startup)injectionBinder.GetInstance<Startup>();
            start.Dispatch();
        }

        protected override void mapBindings()
        {
            MapModels();
            MapServices();
            MapCommands();
        }

        private void MapModels()
        {
            injectionBinder.Bind<GameConfig>().ToSingleton();
            injectionBinder.Bind<ResourceManifest>().ToSingleton();

        }

        private void MapServices()
        {
            injectionBinder.Bind<Ghostbit>().ToValue(Service.Get<Ghostbit>());
            injectionBinder.Bind<ResourceSystem>().ToSingleton();
        }

        private void MapCommands()
        {
            commandBinder.Bind<Startup>().To<StartupCmd>().Once();
            commandBinder.Bind<CreateGameInstance>().To<CreateGameInstanceCmd>().Once();
            commandBinder.Bind<LoadResourceManifest>().To<LoadResourceManifestCmd>();
            commandBinder.Bind<LoadLevel>().To<LoadLevelCmd>();
            injectionBinder.Bind<LoadLevelComplete>().ToSingleton();
        }
    }
}