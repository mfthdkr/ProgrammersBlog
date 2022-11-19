using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using System.IO;
using System;
using System.Threading.Tasks;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Authorization;

namespace ProgrammersBlog.Mvc.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwroot;
        private readonly string _imgFolder = "img";
        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = _env.WebRootPath;

        }

        

        public async Task<Shared.Utilities.Results.Abstract.IDataResult<ImageUploadedDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName="userImages")
        {
            if (!Directory.Exists($"{_wwwroot}/{_imgFolder}/{folderName}"))
            {
                Directory.CreateDirectory($"{_wwwroot}/{_imgFolder}/{folderName}");
            }

            string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            string fileExtension = Path.GetExtension(pictureFile.FileName);

            DateTime dateTime = DateTime.Now;
            string newFileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($"{_wwwroot}/{_imgFolder}/{folderName}", newFileName);

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            return new Shared.Utilities.Results.Concrete.DataResult<ImageUploadedDto>(ResultStatus.Success,$"{userName} adlı kulanıcının resmi başarıyla yüklenmiştir.",new ImageUploadedDto
            {
                FullName= $"{folderName}/{newFileName}",
                OldName = oldFileName,
                Extension = fileExtension,
                FolderName= folderName,
                Path = path,
                Size = pictureFile.Length
            });
        }

        public Shared.Utilities.Results.Abstract.IDataResult<ImageDeletedDto> Delete(string pictureName)
        {

            var fileToDelete = Path.Combine($"{_wwwroot}/{_imgFolder}/", pictureName); // fileToDelete path
            if (System.IO.File.Exists(fileToDelete)) // dosya varsa, sil
            {
                var fileInfo = new FileInfo(fileToDelete);
                var imageDeleteDto = new ImageDeletedDto
                {
                    FullName = pictureName,
                    Extension = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length
                };
                System.IO.File.Delete(fileToDelete);
                return new Shared.Utilities.Results.Concrete.DataResult<ImageDeletedDto>(ResultStatus.Success, imageDeleteDto);

            }
            else
            {
                return new Shared.Utilities.Results.Concrete.DataResult<ImageDeletedDto>(ResultStatus.Error, $"Böyle bir resim bulunamadı", null);
            }


        }

    }
}
