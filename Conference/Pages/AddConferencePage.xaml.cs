using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Collections.Specialized.BitVector32;

namespace Conference.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddConferencePage.xaml
    /// </summary>
    public partial class AddConferencePage : Page
    {
        Conferences conferenceEdit = new Conferences();
        Managers managerUser = new Managers();
        string Action { set; get; }
        public AddConferencePage(Managers manager)
        {
            InitializeComponent();

            Action = "Добавление";

            FormBox.ItemsSource = ConnectDataBase.connectDataBase.Forms.ToList();
            FormBox.SelectedIndex = 0;

            StatusBox.ItemsSource = ConnectDataBase.connectDataBase.Statuses.ToList();
            StatusBox.SelectedIndex = 0;

            managerUser = manager;
            NameBox.Text = managerUser.surname + " " + managerUser.name + " " + managerUser.patronymic;
        }
        public AddConferencePage(Managers manager, Conferences conference)
        {
            InitializeComponent();

            conferenceEdit = conference;

            Action = "Редактирование";
            HeaderBox.Text = "Редактировать данные о конференции";
            AddConferenceButton.Content = "Сохранить";
            
            HeaderConferenceBox.Text = conferenceEdit.nameConference;
            DateStart.SelectedDate = conferenceEdit.dateStart;
            DateEnd.SelectedDate = conferenceEdit.dateEnd;
            PlaceBox.Text = conferenceEdit.place;
            DescriptionBox.Text = conferenceEdit.description;

            FormBox.ItemsSource = ConnectDataBase.connectDataBase.Forms.ToList();
            StatusBox.ItemsSource = ConnectDataBase.connectDataBase.Statuses.ToList();

            FormBox.SelectedIndex = (int)(conferenceEdit.idForm - 1);
            StatusBox.SelectedIndex = conferenceEdit.idStatus - 1;

            managerUser = manager;
            NameBox.Text = managerUser.surname + " " + managerUser.name + " " + managerUser.patronymic;
        }

        private void AddConferenceButton_Click(object sender, RoutedEventArgs e)
        {
            if (Action == "Добавление")
            {
                if (HeaderConferenceBox.Text != null && DateStart.SelectedDate != null)
                {
                    Conferences conference = new Conferences
                    {
                        nameConference = HeaderConferenceBox.Text,
                        dateStart = (DateTime)DateStart.SelectedDate,
                        dateEnd = (DateTime)DateEnd.SelectedDate,
                        place = PlaceBox.Text,
                        description = DescriptionBox.Text,
                        idForm = FormBox.SelectedIndex + 1,
                        idStatus = StatusBox.SelectedIndex + 1,
                    };

                    ConnectDataBase.connectDataBase.Conferences.Add(conference);
                    ConnectDataBase.connectDataBase.SaveChanges();

                    if (MessageBox.Show("Данные о конференции были добавлены.", "Добавление...", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                        mainWindow.Navigation.Navigate(new Pages.ConferencePage(managerUser));
                    }
                }
                else
                {
                    MessageBox.Show("Вы заполнили не все поля!", "Добавление...", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (Action == "Редактирование")
            {
                if (HeaderConferenceBox.Text != null && DateStart.SelectedDate != null)
                {
                    ConnectDataBase.connectDataBase.Conferences.Find(conferenceEdit.idConference).nameConference = HeaderConferenceBox.Text;
                    ConnectDataBase.connectDataBase.Conferences.Find(conferenceEdit.idConference).dateStart = (DateTime)DateStart.SelectedDate;
                    ConnectDataBase.connectDataBase.Conferences.Find(conferenceEdit.idConference).dateEnd = (DateTime)DateEnd.SelectedDate;
                    ConnectDataBase.connectDataBase.Conferences.Find(conferenceEdit.idConference).place = PlaceBox.Text;
                    ConnectDataBase.connectDataBase.Conferences.Find(conferenceEdit.idConference).description = DescriptionBox.Text;
                    ConnectDataBase.connectDataBase.Conferences.Find(conferenceEdit.idConference).idForm = FormBox.SelectedIndex + 1;
                    ConnectDataBase.connectDataBase.Conferences.Find(conferenceEdit.idConference).idStatus = StatusBox.SelectedIndex + 1;

                    ConnectDataBase.connectDataBase.SaveChanges();

                    if (MessageBox.Show("Данные о конференции были обновлены.", "Обновление...", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                        mainWindow.Navigation.Navigate(new Pages.MainPage(managerUser));
                    }
                }
                else
                {
                    MessageBox.Show("Заполнены не все поля");
                }
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
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Navigation.Navigate(new Pages.MainPage(managerUser));
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Navigation.Navigate(new Pages.LoginPage());
        }

        /* Метод, осуществляющий валидацию данных для поля DatePicker */
        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            string justNumbers = new String(datePicker.Text.Where(Char.IsDigit).ToArray());
            if (justNumbers.Length == 8)
            {
                string newDate = justNumbers.Insert(2, "/").Insert(5, "/");
                try
                {
                    datePicker.SelectedDate = DateTime.Parse(newDate);
                }
                catch
                {
                    datePicker.Text = "";
                }
            }
        }
    }
}
