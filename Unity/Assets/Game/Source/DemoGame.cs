using UnityEngine;
using System.Collections;
using Ghostbit.Framework.Unity;
using Ghostbit.Framework.Core.Utils;
using NLog;
using Ghostbit.Framework.Unity.Services;
using Ghostbit.Framework.Unity.Signals;

namespace Ghostbit.DemoGame
{
    [GhostbitGame]
    public class DemoGame : IGame
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        [Inject]
        public LoadLevel LoadLevel { get; set; }

        [Inject]
        public LoadLevelComplete LoadLevelComplete { get; set; }

        public string ShortTitle { get { return "demogame"; } }
        public string LongTitle { get { return "Demo Game"; } }
        public string MainLevel { get { return "Main"; } }

        public DemoGame()
        {
            logger.Trace("DemoGame ctor");
            Service.Set<DemoGame>(this);
        }

        public void Init()
        {
            LoadLevelComplete.AddOnce(OnLevelLoaded);
        }

        private void OnLevelLoaded(string levelName)
        {
            logger.Trace("OnLevelLoaded: {0}", levelName);
            if (levelName == MainLevel)
            {
                //Service.Get<Ghostbit.Framework.Unity.Ghostbit>().StartCoroutine(LoadTestAsset());
            }
        }

        //private IEnumerator LoadTestAsset()
        //{
        //    ResourceSystem res = Service.Get<ResourceSystem>();
        //    var request = res.LoadAsync<TextAsset>("Test/TestResource");
        //    yield return request;
        //}
    }
}
