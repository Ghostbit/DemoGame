using UnityEngine;
using System.Collections;
using System;

namespace Ghostbit.Framework.Unity.System
{
    public class CoroutineReturn
    {
        public virtual bool finished { get; set; }
        public virtual bool cancel { get; set; }
    }

    public class RadicalRoutine
    {
        private bool cancel;
        public IEnumerator enumerator;
        public event Action Cancelled;
        public event Action Finished;

        public void Cancel()
        {
            cancel = true;
        }

        private static RadicalRoutine own = new RadicalRoutine();

        public static IEnumerator Run(IEnumerator extendedCoRoutine)
        {
            return own.Execute(extendedCoRoutine);
        }

        public static RadicalRoutine Create(IEnumerator extendedCoRoutine)
        {
            var rr = new RadicalRoutine();
            rr.enumerator = rr.Execute(extendedCoRoutine);
            return rr;
        }

        private IEnumerator Execute(IEnumerator extendedCoRoutine)
        {
            while (!cancel && extendedCoRoutine != null && extendedCoRoutine.MoveNext())
            {
                var v = extendedCoRoutine.Current;
                var cr = v as CoroutineReturn;
                if (cr != null)
                {
                    if (cr.cancel)
                    {
                        cancel = true;
                        break;
                    }
                    while (!cr.finished)
                    {
                        if (cr.cancel)
                        {
                            cancel = true;
                            break;
                        }
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    yield return v;
                }
            }

            if (cancel && Cancelled != null)
            {
                Cancelled();
            }
            if (Finished != null)
            {
                Finished();
            }
        }
    }
}