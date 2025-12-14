using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using lab4_2.Entity;
using lab4_2.ServerRequest;

namespace lab4_2.Pages;

public partial class MainPage : Window
{
    public ObservableCollection<Student> Students { get; set; }
    public string _username;
    public string _password;

    public MainPage(string username, string password)
    {
        _username = username;
        _password = password;

        InitializeComponent();
        Students = new ObservableCollection<Student>();
        DataContext = this;

        GetStudentsFromServer();
    }

    private async void GetStudentsFromServer()
    {
        try
        {
            // Викликаємо асинхронний метод для отримання студентів
            var getStudents = new GetStudents();
            List<Student> studentList = await getStudents.FetchStudentsAsync();
            
            foreach (var student in studentList)
            {
                Console.WriteLine(student.Username);
                Students.Add(student);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error fetching students: {ex.Message}");
        }
    }
    
    private void DetailedInfoClick(object sender, RoutedEventArgs e)
    {
        var info = (Student)((Button)sender).DataContext;
        var detailedInfoName = new DetailedInfo(info.Username);
        detailedInfoName.Show();
    }
    
    private void Close(object sender, RoutedEventArgs e)
    {
        var login = new Login();
        login.Show();
        Close();
    }

    private void Search(object sender, RoutedEventArgs e)
    {
        var search = new Search(_username, _password);
        search.Show();
        Close();
    }

    private void UpdateInformationClick(object sender, RoutedEventArgs e)
    {
        var update = new UpdateInformation(_username, _password);
        update.Show();
        Close();
    }

    private void RateClick(object sender, RoutedEventArgs e)
    {
        var rate = new RateSpeciality(_username);
        rate.Show();
    }
}