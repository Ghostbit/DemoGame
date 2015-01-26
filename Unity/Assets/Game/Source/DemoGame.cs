using UnityEngine;
using System.Collections;
using Ghostbit.Framework.Unity;
using Ghostbit.Framework.Core.Utils;
using NLog;

namespace Ghostbit.DemoGame
{
    [GhostbitGame]
    public class DemoGame : IGame
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        public string ShortTitle { get { return "demogame"; } }
        public string LongTitle { get { return "Demo Game"; } }
        public string MainLevel { get { return "Main"; } }

        public DemoGame()
        {
            logger.Trace("DemoGame ctor");
            Service.Set<DemoGame>(this);
        }
    }
}
