using MahApps.Metro;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace WpfApplication
{

    public partial class AccentStyleWindow : MetroWindow
    {
        public void ChangeAppStyle()
        {
            ThemeManager.ChangeAppStyle(this,
                                        ThemeManager.GetAccent("Red"),
                                        ThemeManager.GetAppTheme("BaseDark"));
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