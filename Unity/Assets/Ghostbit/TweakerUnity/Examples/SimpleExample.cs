using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using Ghostbit.Framework.Unity.Utils;
using Ghostbit.Tweaker;
using Ghostbit.Tweaker.Core;

public class SimpleExample : MonoBehaviour
{
    private Tweaker tweaker;

    [Invokable("ExampleInvokable")]
    private static void ExampleInvokable()
    {
        Debug.Log("ExampleInvokable invoked");
    }
    
    void Start()
    {
        tweaker = new Tweaker();
        tweaker.Init();

        IInvokable invokable = tweaker.Invokables.GetInvokable("ExampleInvokable");
        invokable.Invoke();
    }
}
