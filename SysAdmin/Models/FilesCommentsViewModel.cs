using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysAdmin.Models
{
    public class FilesCommentsViewModel
    {

        public Files Files { get; set; }
        public List<Comments> Comments { get; set; }
        public Comments Comment { get; set; }
    }
}
