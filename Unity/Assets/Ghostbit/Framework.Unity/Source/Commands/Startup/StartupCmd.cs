using Ghostbit.Framework.Unity.Services;
using Ghostbit.Framework.Unity.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Framework.Unity.Commands
{
    internal class StartupCmd : LoggedCommand
    {
        [Inject]
        public CreateGameInstance CreateGameInstance { get; set; }

        [Inject]
        public LoadLevel LoadLevel { get; set; }

        [Inject]
        public LoadResourceManifest LoadResourceManifest { get; set; }


        protected override void DoExecute()
        {
            LoadResourceManifest.Dispatch();
            CreateGameInstance.Dispatch();            
            LoadLevel.Dispatch(injectionBinder.GetInstance<IGame>().MainLevel);
        }
    }
}
