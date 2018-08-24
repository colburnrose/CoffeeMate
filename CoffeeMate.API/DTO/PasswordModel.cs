namespace CoffeeMate.API.DTO
{
    public class PasswordModel
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}