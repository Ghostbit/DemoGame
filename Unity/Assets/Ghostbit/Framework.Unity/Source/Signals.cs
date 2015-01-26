using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostbit.Framework.Unity.Signals
{
    internal class Start : Signal { }
    internal class CreateGameInstance : Signal { }

    public class LoadLevel : Signal<string> { }
    public class ShowLoadingScreen : Signal { }
    public class HideLoadingScreen : Signal { }
}
