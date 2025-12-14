using System.Windows;
using lab2_2.Request;
using lab4_2.Entity;

namespace lab4_2.Pages;

public partial class RateSpeciality : Window
{
    public RateModel RateModel { get; set; }

    public RateSpeciality(string username)
    {
        InitializeComponent();
        LoadStudentInfo(username);
    }

    private async void LoadStudentInfo(string username)
    {
        // Отримуємо детальну інформацію про студента
        var studentDetailedInfo = await GetStudentDetailedInfoFromServer(username);

        // Ініціалізуємо модель після отримання даних
        RateModel = new RateModel { Username = username, Speciality = studentDetailedInfo.Specialization };
        DataContext = RateModel;
    }

    private async Task<StudentDetailedInfo> GetStudentDetailedInfoFromServer(string username)
    {
        // Створюємо об'єкт запиту
        var request = new GetStudentDetailedInfo();

        // Отримуємо детальну інформацію про студента
        var studentDetailedInfo = await request.FetchStudentDetailedInfo(username);
        return studentDetailedInfo;
    }

    private async void OnSaveRating(object sender, RoutedEventArgs e)
    {
        if (await Rate())
        {
            MessageBox.Show($"Ви оцінили спеціальність на {RateModel.Rating}");
        }
    }

    private async Task<bool> Rate()
    {
        var request = new RateRequest();

        return await request.RateAsync(RateModel.Username, RateModel.Speciality, RateModel.Rating.ToString());
    }
}