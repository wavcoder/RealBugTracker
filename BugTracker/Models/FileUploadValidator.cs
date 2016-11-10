using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
   
   
        public static class FileUploadValidator
        {
            public static bool IsWebFriendlyFile(HttpPostedFileBase file)
            {
                // check for actual object
                if (file == null)
                    return false;
                // check size - file must be less than 2 MB and greater than 1 KB
                if (file.ContentLength > 67108864)
                    return false;
                string fileExt = Path.GetExtension(file.FileName).ToLower();
                if (fileExt == ".jpg" || fileExt == ".doc" || fileExt == ".xls"
                || fileExt == ".png"  || fileExt == ".gif" ||fileExt == ".bmp" 
                || fileExt == ".pdf"  || fileExt == ".txt" || fileExt == ".doc" 
                || fileExt == ".docx")
            {
                return true;
                }
                else
                {
                    return false;

                }
            }
        
    }
}