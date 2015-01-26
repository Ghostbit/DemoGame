using Assets.Ghostbit.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ghostbit.Framework.Unity.Commands
{
    internal class CreateGameInstanceCmd : LoggedCommand
    {
        [Inject]
        public GameConfig Config { get; set; }

        protected override void DoExecute()
        {
            Type type = GetGameType();
            injectionBinder.Bind<IGame>().To(type).ToSingleton();
            IGame game = injectionBinder.GetInstance<IGame>();

            Config.longTitle = game.LongTitle;
            Config.shortTitle = game.ShortTitle;

            logger.Info("UnityGame initialized: {0} : {1}", Config.shortTitle, Config.longTitle);
        }

        private Type GetGameType()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var types = from type in assembly.GetTypes()
                        where Attribute.IsDefined(type, typeof(GhostbitGame))
                        select type;

            Type[] typesArray = types.ToArray();
            if (typesArray.Length == 0)
            {
                throw new Exception("Failed to find class with GhostbitGame attribute.");
            }
            else if (typesArray.Length > 1)
            {
                throw new Exception("Found more than one class with GhostbitGame attribute.");
            }
            else
            {
                Type type = typesArray[0];
                if (!typeof(IGame).IsAssignableFrom(type))
                {
                    throw new Exception("Class marked with GhostbitGame attribute does not implement IGame");
                }

                logger.Info("Found class marked with GhostbitGame: {0}", type.FullName);
                return type;
            }
        }
    }
}
