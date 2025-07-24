using SQLite;

namespace C971App.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Pin { get; set; }

        // Optional: Add additional user information if needed
        public bool IsFirstLogin { get; set; } = true;
    }
}