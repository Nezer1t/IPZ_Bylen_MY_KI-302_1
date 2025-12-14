namespace lab4_2.Entity;

public class StudentDetailedInfo
{
    public string Username { get; set; }
    public string Name { get; set; }
    public double Rating { get; set; }
    public string University { get; set; }
    public string Specialization { get; set; }
    public string Email { get; set; }

    public StudentDetailedInfo(string name, double rating, string university, string specialization, string email)
    {
        Name = name;
        Rating = rating;
        University = university;
        Specialization = specialization;
        Email = email;
    }
    
    public StudentDetailedInfo() { }
}