using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Dto.Comment
{
    public class AddCommentCriteriaDto
    {
        public string CommentContent { get; set; }
        public int AuthorId { get; set; }
    }
}
