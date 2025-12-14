using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Globalization;
using lab4_2.Entity;
using lab4_2.ServerRequest;

namespace lab4_2.Pages
{
    public partial class Search : Window
    {
        public string _email;
        public string _password;
        private SearchStudentsRequest _searchRequest;

        public Search(string email, string password)
        {
            InitializeComponent();
            _email = email;
            _password = password;
            _searchRequest = new SearchStudentsRequest();
        }

        private async void SearchByName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameSearchBox.Text))
                {
                    MessageBox.Show("Будь ласка, введіть ім'я для пошуку.", "Помилка", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var results = await _searchRequest.SearchByName(NameSearchBox.Text);
                ResultsListBox.ItemsSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SearchByRating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(RatingSearchBox.Text))
                {
                    MessageBox.Show("Будь ласка, введіть рейтинг для пошуку.", "Помилка", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                double rating;
                if (!double.TryParse(RatingSearchBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out rating))
                {
                    MessageBox.Show("Будь ласка, введіть коректний рейтинг.", "Помилка", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var results = await _searchRequest.SearchByRating(rating);
                ResultsListBox.ItemsSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SearchByUniversity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UniversitySearchBox.Text))
                {
                    MessageBox.Show("Будь ласка, введіть університет для пошуку.", "Помилка", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var results = await _searchRequest.SearchByUniversity(UniversitySearchBox.Text);
                ResultsListBox.ItemsSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SearchBySpecialty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SpecialtySearchBox.Text))
                {
                    MessageBox.Show("Будь ласка, введіть спеціальність для пошуку.", "Помилка", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var results = await _searchRequest.SearchBySpecialty(SpecialtySearchBox.Text);
                ResultsListBox.ItemsSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is Student student)
                {
                    var detailedInfo = new DetailedInfo(student.Name);
                    detailedInfo.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainWindow = new MainPage(_email, _password);
                mainWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
