using NLog;
using strange.extensions.command.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostbit.Framework.Unity.Commands
{
    public abstract class LoggedCommand : Command
    {
        private string name;
        protected Logger logger;

        public LoggedCommand()
        {
            logger = LogManager.GetLogger(GetType().FullName);
        }

        public sealed override void Execute()
        {
            logger.Trace("Execute");
            DoExecute();
        }

        public override void Fail()
        {
            logger.Trace("Fail");
            base.Fail();
        }

        public override void Retain()
        {
            logger.Trace("Retain");
            base.Retain();
        }

        public override void Release()
        {
            logger.Trace("Release");
            base.Release();
        }

        public override void Restore()
        {
            logger.Trace("Restore");
            base.Restore();
        }

        protected abstract void DoExecute();
    }
}
