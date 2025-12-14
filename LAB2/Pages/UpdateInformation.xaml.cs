using System.Windows;
using lab2_2.Request;
using lab4_2.Entity;
using lab4_2.Request;

namespace lab4_2.Pages
{
    public partial class UpdateInformation : Window
    {
        public string Username;
        public string Password;
        private StudentDetailedInfo _studentDetailedInfo;

        public UpdateInformation(string username, string password)
        {
            InitializeComponent();
            Username = username;
            Password = password;

            GetStudentDetailedInfoFromServer(username);
        }

        private async void GetStudentDetailedInfoFromServer(string username)
        {
            // Створюємо об'єкт запиту
            GetStudentDetailedInfo request = new GetStudentDetailedInfo();

            // Отримуємо детальну інформацію про студента
            _studentDetailedInfo = await request.FetchStudentDetailedInfo(username);
            DataContext = _studentDetailedInfo;
        }


        private async void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var updateInfoRequest = new UpdateInfoRequest();
            if (await updateInfoRequest.Update(Username, _studentDetailedInfo))
            {
                var mainPage = new MainPage(Username, Password);
                mainPage.Show();
                Close();
            }
        }

    }
}