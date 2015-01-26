using UnityEngine;
using System.Collections;
using System;

namespace Ghostbit.Framework.Unity
{
    public interface IGame
    {
        string ShortTitle { get; }
        string LongTitle { get; }
        string MainLevel { get; }
    }
}