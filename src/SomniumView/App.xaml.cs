using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace SomniumView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            var name = Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(name);
            var appDomain = AppDomain.CurrentDomain;
            appDomain.AppendPrivatePath(Path.Combine(path, "library"));
        }


    }
}
