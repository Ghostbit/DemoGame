using NLog;
using strange.extensions.dispatcher.eventdispatcher.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostbit.strange
{
    public class GhostbitEventDispatcher : EventDispatcher
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override void Dispatch(object eventType, object data)
        {
            logger.Trace("Dispatch event: {0} : {1}", eventType, data);
            base.Dispatch(eventType, data);
        }
    }
}
