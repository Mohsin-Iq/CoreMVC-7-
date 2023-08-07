namespace Practic2.Models
{
    public class Student : Book
    {
        public int StudentID { get; set; }
        public string? StudentName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public bool IsAdmin { get; set; }
    }
}
