using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace CartografiasMusicais.CrossCutting.Utils
{
    public class ValidExtensionAttribute : ValidationAttribute
    {
        private readonly string[] AllowedFileExtensions;

        public ValidExtensionAttribute(string extensions)
        {
            AllowedFileExtensions = extensions.Split(',');
        }

        public override bool IsValid(object file)
        {
            var formFile = file as IFormFile;


            if (file != null && !AllowedFileExtensions.Contains(Path.GetExtension(formFile.FileName)))
            {
                ErrorMessage = "Apenas arquivos do tipo: " + string.Join(", ", AllowedFileExtensions);
                return false;
            }

            return true;
        }
    }
}
