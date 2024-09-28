using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Conference.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var managers = ConnectDataBase.connectDataBase.Managers.ToList();

            try 
            {
                if (loginBox.Text == "" || passwordBox.Password == "")
                {
                    warningBox.Text = "Вы не ввели данные в поля формы!";
                    warningBox.Visibility = Visibility.Visible;
                }
                else 
                {
                    for (int i = 0; i < managers.Count; i++)
                    {
                        if (loginBox.Text == managers[i].login && passwordBox.Password == managers[i].password)
                        {
                            warningBox.Visibility = Visibility.Collapsed;

                            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                            mainWindow.Navigation.Navigate(new Pages.MainPage(managers[i]));
                        }
                        else
                        {
                            warningBox.Text = "Вы ввели неправильный логин или пароль!";
                            warningBox.Visibility = Visibility.Visible;
                        }
                    }
                }
            } 
            catch 
            {
                warningBox.Text = "Возникла проблема при входе в учетную запись. Пожалуйста, повторите попытку позже.";
                warningBox.Visibility = Visibility.Visible;
            }
        }
    }
}
