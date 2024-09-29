using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Conference.Pages
{
    /// <summary>
    /// Логика взаимодействия для ConferencePage.xaml
    /// </summary>
    public partial class ConferencePage : Page
    {
        Managers managerUser = new Managers();
        public ConferencePage(Managers manager)
        {
            InitializeComponent();

            Statuses status = new Statuses { idStatus = 0, nameStatus = "Все" };
            List<Statuses> statuses = new List<Statuses> { status };
            statuses.AddRange(ConnectDataBase.connectDataBase.Statuses.ToList());
            StatusBox.ItemsSource = statuses;

            Forms form = new Forms { idForm = 0, nameForm = "Все" };
            List<Forms> forms = new List<Forms> { form };
            forms.AddRange(ConnectDataBase.connectDataBase.Forms.ToList());
            FormBox.ItemsSource = forms;

            LoadConferences();

            managerUser = manager;
            NameBox.Text = managerUser.surname + " " + managerUser.name + " " + managerUser.patronymic;
        }

        public void LoadConferences()
        {
            List<Conferences> conferencesList = new List<Conferences>();

            var conferences = ConnectDataBase.connectDataBase.Conferences.ToList();
            var sorted = conferences.OrderByDescending(u => u.dateStart).ToList();

            for (int i = (Convert.ToInt32(numberPage.Content) * 10) - 10; i < Convert.ToInt32(numberPage.Content) * 10; i++) 
            {
                try 
                {
                    if (StatusBox.SelectedIndex == 0 && FormBox.SelectedIndex == 0) { conferencesList.Add(sorted[i]); }
                    else if (StatusBox.SelectedIndex == sorted[i].idStatus && FormBox.SelectedIndex == 0) { conferencesList.Add(sorted[i]); }
                    else if (StatusBox.SelectedIndex == 0 && FormBox.SelectedIndex == sorted[i].idForm) { conferencesList.Add(sorted[i]); }
                    else if (StatusBox.SelectedIndex == sorted[i].idStatus && FormBox.SelectedIndex == sorted[i].idForm) { conferencesList.Add(sorted[i]); }
                }
                catch { }
            }
            ConferencesList.ItemsSource = conferencesList;
        }

        private void Conference_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Navigation.Navigate(new Pages.ConferencePage(managerUser));
        }
        private void Main_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Navigation.Navigate(new Pages.MainPage(managerUser));
        }
        private void AddConferenceButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Navigation.Navigate(new Pages.AddConferencePage(managerUser));
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Navigation.Navigate(new Pages.LoginPage());
        }

        private void StatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FormBox.SelectedIndex == -1) { }
            else { LoadConferences(); }
        }
        private void FormBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadConferences();
        }

        /* Метод, осуществляющий поиск по названию конференций */
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StatusBox.SelectedIndex = 0;
            FormBox.SelectedIndex = 0;

            List<Conferences> conferencesList = new List<Conferences>();

            var conferences = ConnectDataBase.connectDataBase.Conferences.ToList();

            for (int i = (Convert.ToInt32(numberPage.Content) * 10) - 10; i < Convert.ToInt32(numberPage.Content) * 10; i++)
            {
                try
                {
                    if (conferences[i].nameConference.ToLower().Contains(SearchBox.Text.ToLower())) { conferencesList.Add(conferences[i]); }
                }
                catch { }
            }
            ConferencesList.ItemsSource = conferencesList;
        }

        /* Метод для пагинации */
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            double numberPageAll = ConnectDataBase.connectDataBase.Conferences.Count() / 10;

            if (Convert.ToInt32(numberPage.Content) <= numberPageAll)
            {
                int num = Convert.ToInt32(numberPage.Content) + 1;
                numberPage.Content = num.ToString();
                LoadConferences();
            }
            else
            {
                if (ConnectDataBase.connectDataBase.Conferences.Count() % 2 == 0)
                {
                    LoadConferences();
                }
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(numberPage.Content) == 1)
            {
                LoadConferences();
            }
            else
            {
                int number = Convert.ToInt32(numberPage.Content) - 1;
                numberPage.Content = number.ToString();
                LoadConferences();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            int idConference = (int)((Button)sender).DataContext;

            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Navigation.Navigate(new Pages.AddConferencePage(managerUser, ConnectDataBase.connectDataBase.Conferences.Find(idConference)));
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int idConference = (int)((Button)sender).DataContext;

            if (MessageBox.Show("Вы действительно хотите удалить конференцию?", "Удаление...", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ConnectDataBase.connectDataBase.Conferences.Remove(ConnectDataBase.connectDataBase.Conferences.Find(idConference));
                ConnectDataBase.connectDataBase.SaveChanges();

                LoadConferences();
            }
        }
    }
}
