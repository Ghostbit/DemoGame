using Ghostbit.Framework.Unity.Signals;
using strange.extensions.context.api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Framework.Unity.Commands
{
    public class LoadLevelCmd : AsyncCommand
    {
        [Inject]
        public string LevelName { get; set; }

        [Inject]
        public LoadLevelComplete LoadLevelCompolete { get; set; }

        protected override IEnumerator DoExecuteAsync()
        {
            // TODO: show loading screen
            logger.Info("Loading level: {0}", LevelName);
            AsyncOperation async = Application.LoadLevelAsync(LevelName);
            yield return async;
            logger.Info("Level loaded: {0}", LevelName);
            LoadLevelCompolete.Dispatch(LevelName);
            // TODO: hide loading screen
        }
    }
}
