using Assets.Ghostbit.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostbit.Framework.Unity.Commands
{
    public abstract class AsyncCommand : LoggedCommand
    {
        [Inject]
        public Ghostbit Ghostbit { get; set; }

        protected override void DoExecute()
        {
            Retain();
            logger.Info("Ghostbit = {0}", Ghostbit);
            Ghostbit.StartCoroutine(ExecuteAsync());

        }

        private IEnumerator ExecuteAsync()
        {
            var iter = DoExecuteAsync();
            yield return iter.Current;
            while(iter.MoveNext())
            {
                yield return iter.Current;
            }

            Release();
        }

        protected abstract IEnumerator DoExecuteAsync();
    }
}
