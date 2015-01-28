using Assets.Ghostbit.Core.Utils;
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
    internal class StartCmd : LoggedCommand
    {
        [Inject]
        public CreateGameInstance CreateGameInstance { get; set; }

        [Inject]
        public LoadLevel LoadLevel { get; set; }

        protected override void DoExecute()
        {
            CreateGameInstance.Dispatch();
            
            IGame game = injectionBinder.GetInstance<IGame>();
            LoadLevel.Dispatch(game.MainLevel);
        }
    }
}
