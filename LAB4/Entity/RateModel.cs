namespace lab4_2.Entity;

public class RateModel
{
    public string Username { get; set; }
    public int Rating { get; set; }
    public string Speciality { get; set; }

    public RateModel(string username, int rating, string speciality)
    {
        Username = username;
        Rating = rating;
        Speciality = speciality;
    }
    
    public RateModel() { }
}