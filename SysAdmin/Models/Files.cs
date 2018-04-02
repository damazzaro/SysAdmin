using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SysAdmin.Models
{
    public partial class Files
    {
        public int FileId { get; set; }

        [DisplayName("Author")]
        public string UserEmail { get; set; }

        [DisplayName("File Name")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "You must specify a file description")]
        [DisplayName("File Description")]
        [MaxLength(250, ErrorMessage = "The description may not be over 250 characters")]
        public string FileDescription { get; set; }

        public bool FileVerified { get; set; }

    }
}
