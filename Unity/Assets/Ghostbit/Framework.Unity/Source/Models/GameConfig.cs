using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostbit.Framework.Unity.Models
{
    [Serializable]
    public class GameConfig
    {
        public string shortTitle;
        public string longTitle;
        //public string[] args;

        public GameConfig Clone()
        {
            GameConfig clone = MemberwiseClone() as GameConfig;
            //clone.args = new string[args.Length];
            //args.CopyTo(clone.args, 0);
            return clone;
        }
    }
}
