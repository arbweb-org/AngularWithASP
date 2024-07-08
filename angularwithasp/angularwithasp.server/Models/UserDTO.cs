namespace angularwithasp.server.Models
{
    public class UserDTO
    {
        public User[] Users { get; set; }
        public int Skip { get; set; }
        public int Total { get; set; }
    }
}