using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostbit.Framework.Unity.Signals
{
    internal class Startup : Signal { }
    internal class CreateGameInstance : Signal { }
    internal class LoadResourceManifest : Signal { }

    public class LoadLevel : Signal<string> { }
    public class LoadLevelComplete : Signal<string> { }
    public class ShowLoadingScreen : Signal { }
    public class HideLoadingScreen : Signal { }
}
