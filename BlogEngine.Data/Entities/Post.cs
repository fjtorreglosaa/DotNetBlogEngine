using System;

namespace BlogEngine.Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string PostContent { get; set; }
        public DateTime DatePublished { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }

        public int Status { get; set; }
    }
}
