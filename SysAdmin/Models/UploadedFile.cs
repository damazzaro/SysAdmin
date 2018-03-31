using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SysAdmin.Models
{
    public class UploadedFile
    {

        public string FileUserEmail { get; set; }

        [Required, FileExtensions(Extensions=".pdf", ErrorMessage="You may only upload .pdf's")]
        public FormFile FileUpload { get; set; }

        [DisplayName("File Name (Leave blank if you don't want to change the file name)")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "You must specify a file description")]
        [DisplayName("File Description")]
        public string FileDescription { get; set; }
    }
}

