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

        private void RRIGHT_Button_Copy1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    

}