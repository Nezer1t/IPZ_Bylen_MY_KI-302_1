using System.Windows;
using lab4_2.Entity;
using lab4_2.Pages;
using lab4_2.Request;

namespace lab4_2;

public partial class Login : Window
{
    private LoginValues _person;

    public Login()
    {
        InitializeComponent();

        _person = new LoginValues();
        DataContext = _person;
    }

    private async void ButtonClick(object sender, RoutedEventArgs e)
    {
        try
        {
            _person.Password = PasswordBox.Password;
            
            // Перевірка, чи поля порожні
            if (string.IsNullOrWhiteSpace(_person.Username) || string.IsNullOrWhiteSpace(_person.Password))
            {
                MessageBox.Show("Будь ласка, введіть і Username, і пароль.", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var loginRequest = new StudentLogin();
            bool isLoggedIn = await loginRequest.LoginAsync(_person.Username, _person.Password);

            // bool isLoggedIn = true;
            // _person.Username = "Barusu1";
            // _person.Password = "Barusu1@gmail.com";
            
            if (isLoggedIn)
            {
                // Перехід на головну сторінку
                var mainPage = new MainPage(_person.Username, _person.Password);
                mainPage.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Логін не вдався. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RegisterClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var register = new Register();
            register.Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}