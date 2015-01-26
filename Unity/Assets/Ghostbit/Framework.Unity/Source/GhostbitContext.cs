using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using Ghostbit.Framework.Core.Utils;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using Ghostbit.Framework.Unity.Signals;
using Ghostbit.Framework.Unity.Commands;
using Ghostbit.Framework.Unity.Services;

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
            Service.Set<MVCSContext>(this);
        }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        public override void Launch()
        {
            base.Launch();

            Start start = (Start)injectionBinder.GetInstance<Start>();
            start.Dispatch();
        }

        protected override void mapBindings()
        {
            MapServices();
            MapModels();
            MapCommands();
        }

        private void MapCommands()
        {
            commandBinder.Bind<Start>().To<StartCmd>().Once();
            commandBinder.Bind<CreateGameInstance>().To<CreateGameInstanceCmd>().Once();
            commandBinder.Bind<LoadLevel>().To<LoadLevelCmd>();
        }

        private void MapModels()
        {
            injectionBinder.Bind<GameConfig>().ToSingleton();
        }

        private void MapServices()
        {
            injectionBinder.Bind<Ghostbit>().ToValue(Service.Get<Ghostbit>());
            injectionBinder.Bind<ResourceManager>().ToSingleton();
        }
    }
}