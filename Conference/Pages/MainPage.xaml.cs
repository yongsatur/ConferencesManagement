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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        Managers managerUser = new Managers();
        public MainPage(Managers manager)
        {
            InitializeComponent();

            ChangeStatus();
            LoadConferences();
            LoadTile();
            LoadCalenndar();

            managerUser = manager;
            NameBox.Text = managerUser.surname + " " + managerUser.name + " " + managerUser.patronymic;
        }

        /* Метод, осуществляющий загрузку конференций и фильтрацию их по статусу */
        public void LoadConferences()
        {
            DateTime date = Calendar.DisplayDate;
            int month = date.Month, year = date.Year;

            List<Conferences> conferencesList = new List<Conferences>();

            var conferences = ConnectDataBase.connectDataBase.Conferences.ToList();

            if (AllConferenceButton.IsChecked == true)
            {
                for (int i = 0; i < conferences.Count; i++) 
                {
                    if (conferences[i].dateStart.Month == month && conferences[i].dateStart.Year == year) { conferencesList.Add(conferences[i]); }
                }
                
            }
            else if (ActiveConferenceButton.IsChecked == true) 
            {
                for (int i = 0; i < conferences.Count; i++)
                {
                    if (conferences[i].dateStart.Month == month && conferences[i].dateStart.Year == year && conferences[i].idStatus == 1) { conferencesList.Add(conferences[i]); }
                }
            }
            else if (CancelConferenceButton.IsChecked == true) 
            {
                for (int i = 0; i < conferences.Count; i++)
                {
                    if (conferences[i].dateStart.Month == month && conferences[i].dateStart.Year == year && conferences[i].idStatus == 3) { conferencesList.Add(conferences[i]); }
                }
            }
            else if (PassiveConferenceButton.IsChecked == true) 
            {
                for (int i = 0; i < conferences.Count; i++)
                {
                    if (conferences[i].dateStart.Month == month && conferences[i].dateStart.Year == year && conferences[i].idStatus == 2) { conferencesList.Add(conferences[i]); }
                }
            }
            ConferencesList.ItemsSource = conferencesList;
        }

        /* Метод, изменяющий статус конференции, если дата ее окончания меньше сегодняшнего числа */
        public void ChangeStatus() 
        {
            var conferences = ConnectDataBase.connectDataBase.Conferences.ToList();

            for (int i = 0; i < conferences.Count; i++) 
            {
                if (DateTime.Today > conferences[i].dateEnd && conferences[i].idStatus != 2) 
                {
                    ConnectDataBase.connectDataBase.Conferences.Find(conferences[i].idConference).idStatus = 2;
                    ConnectDataBase.connectDataBase.SaveChanges();
                }
            }
        }

        /* Метод, осуществляющий загрузку данных в статистику */
        public void LoadTile()
        {
            int planeConferences = 0, endConferences = 0, activeConferences = 0, cancelConferences = 0;

            var conferences = ConnectDataBase.connectDataBase.Conferences.ToList();

            for (int i = 0; i < conferences.Count; i++)
            {
                if (conferences[i].idStatus == 1) { activeConferences += 1; }
                if (conferences[i].idStatus == 2) { endConferences += 1; }
                if (conferences[i].idStatus == 3) { cancelConferences += 1; }

                planeConferences += 1;
            }

            PlaneConferencesBox.Text = planeConferences.ToString();
            ActiveConferencesBox.Text = activeConferences.ToString();
            EndConferencesBox.Text = endConferences.ToString();
            CancelConferencesBox.Text = cancelConferences.ToString();

            if (planeConferences >= 11 && planeConferences <= 20) { PlaneSentenseBlock.Text = "Конференций было запланировано"; }
            else if (planeConferences == 1) { PlaneSentenseBlock.Text = "Конференция была запланирована"; }
            else if (planeConferences >= 2 && planeConferences <= 4) { PlaneSentenseBlock.Text = "Конференции были запланированы"; }
            else if (planeConferences  == 0 || (planeConferences >= 5 && planeConferences <= 9)) { PlaneSentenseBlock.Text = "Конференций было запланировано"; }
            else if (planeConferences.ToString()[-1] == 1) { PlaneSentenseBlock.Text = "Конференция была запланирована"; }
            else if (planeConferences.ToString()[-1] == 2 || 
                     planeConferences.ToString()[-1] == 3 || 
                     planeConferences.ToString()[-1] == 4) { PlaneSentenseBlock.Text = "Конференции были запланированы"; }
            else { PlaneSentenseBlock.Text = "Конференций было запланировано"; }

            if (endConferences >= 11 && endConferences <= 20) { EndSentenseBlock.Text = "Конференций было завершено"; }
            else if (endConferences == 1) { EndSentenseBlock.Text = "Конференция была завершена"; }
            else if (endConferences >= 2 && endConferences <= 4) { EndSentenseBlock.Text = "Конференции были завершены"; }
            else if (endConferences == 0 || (endConferences >= 5 && endConferences <= 9)) { EndSentenseBlock.Text = "Конференций было завершено"; }
            else if (endConferences.ToString()[-1] == 1) { EndSentenseBlock.Text = "Конференция была завершена"; }
            else if (endConferences.ToString()[-1] == 2 ||
                     endConferences.ToString()[-1] == 3 ||
                     endConferences.ToString()[-1] == 4) { EndSentenseBlock.Text = "Конференции были завершены"; }
            else { EndSentenseBlock.Text = "Конференций было завершено"; }

            if (activeConferences >= 11 && activeConferences <= 20) { ActiveSentenseBlock.Text = "Конференций ожидается"; }
            else if (activeConferences == 1) { ActiveSentenseBlock.Text = "Конференция ожидается"; }
            else if (activeConferences >= 2 && activeConferences <= 4) { ActiveSentenseBlock.Text = "Конференции ожидается"; }
            else if (activeConferences == 0 || (activeConferences >= 5 && activeConferences <= 9)) { ActiveSentenseBlock.Text = "Конференций ожидается"; }
            else if (activeConferences.ToString()[-1] == 1) { ActiveSentenseBlock.Text = "Конференция ожидается"; }
            else if (activeConferences.ToString()[-1] == 2 ||
                     activeConferences.ToString()[-1] == 3 ||
                     activeConferences.ToString()[-1] == 4) { ActiveSentenseBlock.Text = "Конференции ожидается"; }
            else { ActiveSentenseBlock.Text = "Конференций ожидается"; }

            if (cancelConferences >= 11 && cancelConferences <= 20) { CancelSentenseBlock.Text = "Конференций было отменено"; }
            else if (cancelConferences == 1) { CancelSentenseBlock.Text = "Конференция была отменена"; }
            else if (cancelConferences >= 2 && cancelConferences <= 4) { CancelSentenseBlock.Text = "Конференции были отменены"; }
            else if (cancelConferences == 0 || (cancelConferences >= 5 && cancelConferences <= 9)) { CancelSentenseBlock.Text = "Конференции были отменены"; }
            else if (cancelConferences.ToString()[-1] == 1) { CancelSentenseBlock.Text = "Конференция была отменена"; }
            else if (cancelConferences.ToString()[-1] == 2 ||
                     cancelConferences.ToString()[-1] == 3 ||
                     cancelConferences.ToString()[-1] == 4) { CancelSentenseBlock.Text = "Конференции были отменены"; }
            else { CancelSentenseBlock.Text = "Конференций было отменено"; }
        }

        /* Метод, отмечающий на календаре дни, в которые назначены конференции */
        public void LoadCalenndar() 
        {
            List<CalendarDateRange> dates = new List<CalendarDateRange>();
            var conferences = ConnectDataBase.connectDataBase.Conferences.ToList();

            for (int i = 0; i < conferences.Count; i++)
            {
                Calendar.BlackoutDates.Add(new CalendarDateRange(conferences[i].dateStart, (DateTime)conferences[i].dateEnd));
            }
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Navigation.Navigate(new Pages.MainPage(managerUser));
        }
        private void Conference_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Navigation.Navigate(new Pages.ConferencePage(managerUser));
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Navigation.Navigate(new Pages.LoginPage());
        }

        private void AllConferenceButton_Click(object sender, RoutedEventArgs e)
        {
            LoadConferences();
        }
        private void ActiveConferenceButton_Click(object sender, RoutedEventArgs e)
        {
            LoadConferences();
        }
        private void CancelConferenceButton_Click(object sender, RoutedEventArgs e)
        {
            LoadConferences();
        }
        private void PassiveConferenceButton_Click(object sender, RoutedEventArgs e)
        {
            LoadConferences();
        }

        private void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            LoadConferences();
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

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
