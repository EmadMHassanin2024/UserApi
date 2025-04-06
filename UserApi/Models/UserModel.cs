namespace UserApi.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // سيتم تحسين الأمان لاحقًا بتشفيرها
        public string Email { get; set; }
    }
}
