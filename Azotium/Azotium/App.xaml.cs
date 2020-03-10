using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Azotium
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static Ini Config { get; private set; }

        public static string RootPath { get => AppDomain.CurrentDomain.BaseDirectory; } 

        public App()
        {
            // Load and write config.
            var configFilePath = Path.Combine(RootPath, "config.ini");
            if (File.Exists(configFilePath))
            {
                App.Config = new Ini(configFilePath);
                App.Config.Load();
            }
            else
            {
                App.Config = new Ini(configFilePath);
            }
            this.Exit += (sender, args) =>
            {
                App.Config.Save();
            };
        }
    }
}
