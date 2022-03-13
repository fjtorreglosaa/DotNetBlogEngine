using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Dto.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string CommentContent { get; set; }
        public int AuthorId { get; set; }
    }
}
