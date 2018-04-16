using MahApps.Metro;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using MahApps.Metro.IconPacks;

namespace WpfApplication
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // get the current app style (theme and accent) from the application
            // you can then use the current theme and custom accent instead set a new theme
            Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);

            // now set the Green accent and dark theme
            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent("BaseDark"),
                                        ThemeManager.GetAppTheme("BaseDark")); // or appStyle.Item1

            base.OnStartup(e);
        }
    }

    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}