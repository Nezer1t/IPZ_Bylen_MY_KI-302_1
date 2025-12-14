using System.Windows;
using lab4_2.Entity;
using lab4_2.Pages;
using lab4_2.Request;

namespace lab4_2
{
    public partial class Register : Window
    {
        private RegisterValues _person;

        public Register()
        {
            InitializeComponent();

            _person = new RegisterValues();
            DataContext = _person;
        }

        private async void RegisterClick(object sender, RoutedEventArgs e)
        {
            _person.Password = PasswordBox.Password;

            // Перевірка, чи всі поля заповнені
            if (string.IsNullOrWhiteSpace(_person.Name) || string.IsNullOrWhiteSpace(_person.Email) ||
                string.IsNullOrWhiteSpace(_person.Password))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var registerRequest = new StudentRegister();
            bool isRegistered = await registerRequest.RegisterAsync(_person.Name, _person.Password, _person.Email);

            if (isRegistered)
            {
                // Перехід на головну сторінку
                var mainPage = new MainPage(_person.Name, _person.Password);
                mainPage.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Помилка при реєстрації. Спробуйте ще раз.");
            }
        }

        private void GoBackToLogin(object sender, RoutedEventArgs e)
        {
            var loginPage = new Login();
            loginPage.Show();
            Close();
        }
    }
}