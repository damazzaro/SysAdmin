using System;
using System.Collections.Generic;

namespace SysAdmin.Models
{
    public partial class Comments
    {
        public int CommentId { get; set; }
        public string UserEmail { get; set; }
        public string CommentText { get; set; }
        public int FileId { get; set; }
    }
}
