using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentContent { get; set; }
        public DateTime DateCommented { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
