using UnityEngine;
using Ghostbit.Framework.Core;
using Ghostbit.Framework.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Xml;
using NLog.Config;
using NLog;
using System.Collections;
using strange.extensions.context.impl;

namespace Ghostbit.Framework.Unity
{
    // Tag your main game class that inherits from IGame with this attribute.
    [AttributeUsage(AttributeTargets.Class)]
    public class GhostbitGame : Attribute {}

    public class Ghostbit : ContextView
    {
        private Logger logger;

        private void Awake()
        {
            Service.Set<Ghostbit>(this);
            DontDestroyOnLoad(this);

            InitLogging();
            InitContext();           
        }

        private void InitLogging()
        {
            const string configPath = "nlog.config";
            TextAsset config = Resources.Load<TextAsset>(configPath);
            StringReader sr = new StringReader(config.text);
            XmlReader xr = XmlReader.Create(sr);
            LogManager.Configuration = new XmlLoggingConfiguration(xr, null);
            logger = LogManager.GetCurrentClassLogger();
            logger.Info("Logger initialized from {0}", configPath);
        }

        private void InitContext()
        {
            context = new GhostbitContext(this, true);
            context.Start();
        }
    }
}