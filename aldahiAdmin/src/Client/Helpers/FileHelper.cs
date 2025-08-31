using Microsoft.AspNetCore.Components.Forms;
using FirstCall.Client.Shared;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using FirstCall.Application.Requests;
using System.Linq;
using FirstCall.Application.Enums;

namespace FirstCall.Client.Helpers
{
    public static class FileHelper
    {
      //  public static List<ValidationResult> Errors { get; set; } = new List<ValidationResult>();

        public static async Task<List<UploadRequest>> UploadFiles(InputFileChangeEventArgs e, UploadType uploadType, long? maxSize, List<string>? types)
        {

            var files = e.GetMultipleFiles();

            var valid = ValidateFiles(files?.ToList()!, maxSize, types);

            var Files = new List<UploadRequest>();

            var Errors = new List<ValidationResult>();
            if (!valid)
            {
               

                return new List<UploadRequest>();
            }

            foreach (var file in files)
            {
                var name = file.Name;
                var type = file.ContentType;
                var size = file.Size;
                var extension = Path.GetExtension(file.Name);

                var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                var bytes = ms.ToArray();

                Files.Add(new UploadRequest()
                {
                    Data = bytes,
                    UploadType = uploadType,
                    Extension = extension,
                    FileName = name,
                });
            }
            return Files;


        }
        public static bool ValidateFiles(List<IBrowserFile> files, long? MaxSize, List<string>? types)
        {
            var errors = new List<string>();
            var Errors = new List<ValidationResult>();

            if ((files == null || !files.Any()))
                errors.Add("No uploaded files");

            foreach (var file in files)
            {
                var name = file.Name;
                var type = file.ContentType;
                var size = file.Size;

                if (types.Count() > 0 && !types.Contains(type))
                    errors.Add("File type not allowed");
                if (MaxSize != 0 && size > MaxSize)
                {
                    var mbLimit = MaxSize / (1024 * 1024); //in MB
                    var mbSize = size / (1024 * 1024);
                    errors.Add($"Size({mbSize} MB) {"File size exceeded the allowed limit"} ({mbLimit} MB)");
                }
            }

            //if (errors.Count > 0)
            //    Errors.Add(new ValidationResult() { Field = "Uploaded Files", ErrorMessage = errors.First() });

            return Errors.Count() == 0;
        }
    }
}
