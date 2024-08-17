namespace Shared.Dto.Document
{
    //Информация о документе
    public class DocInfo
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public required string Path { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }
    public class DocInfoGet
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public int UserId { get; set; }
        public required string Path { get; set; }
    }
}
