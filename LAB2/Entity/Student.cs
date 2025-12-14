namespace lab4_2.Entity;

public class Student
{
    public string Name { get; set; }
    public string Username { get; set; }
    public double Rating { get; set; }

    public Student(string name, string userName, double rating)
    {
        Name = name;
        Username = name;
        Rating = rating;
    }

    public Student() { }
}