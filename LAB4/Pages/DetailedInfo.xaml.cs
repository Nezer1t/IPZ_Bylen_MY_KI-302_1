using System.Windows;
using lab2_2.Request;
using lab4_2.Entity;
using lab4_2.ServerRequest;

namespace lab4_2.Pages;

public partial class DetailedInfo : Window
{
    private StudentDetailedInfo _studentDetailedInfo;

    public DetailedInfo(string username)
    {
        InitializeComponent();
        _studentDetailedInfo = new StudentDetailedInfo();

        // Ініціалізація контексту даних ще не відбувається
        GetStudentDetailedInfoFromServer(username);
    }

    private async void GetStudentDetailedInfoFromServer(string name)
    {
        // Створюємо об'єкт запиту
        GetStudentDetailedInfo request = new GetStudentDetailedInfo();

        // Отримуємо детальну інформацію про студента
        _studentDetailedInfo = await request.FetchStudentDetailedInfo(name);
        
        // Тепер оновлюємо контекст даних
        DataContext = _studentDetailedInfo;
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        Close();
    }
}