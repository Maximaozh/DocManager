namespace Shared.Dto
{
    public class UserInfo
    {
        public required string Id { get; set; }
        public required string Login { get; set; }
        public required string Role { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
    }
}
