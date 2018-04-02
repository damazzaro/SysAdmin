using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SysAdmin.Models
{
    public partial class Comments
    {
        public int CommentId { get; set; }

        public string UserEmail { get; set; }

        [MaxLength(500, ErrorMessage = "Your comment may not be over 500 characters")]
        public string CommentText { get; set; }

        public int FileId { get; set; }
    }
}
