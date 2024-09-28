using System;
using System.Windows;

namespace Conference
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Navigation.Navigate(new Pages.LoginPage());
        }
    }
}
