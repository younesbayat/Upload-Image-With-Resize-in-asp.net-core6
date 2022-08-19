using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
/*
Upload Image With Resize in asp.net core6
https://ironcode.ir
email:info.younesbayat@gmail.com
 */
namespace UploadImage.Pages
{
    public class IndexModel : PageModel
    {
        private IWebHostEnvironment Environment; // this is need to work with SixLabors.ImageSharp library
        public IndexModel( IWebHostEnvironment environment)
        {
         Environment = environment;
        }
        //index page
        public void OnGet()
        {

        }

        //form post to the upload and resize image
        public IActionResult OnPost(IFormFile image,int width, int height)
        {
            if (image != null) 
            {
                if (width==0 || height==0)
                {
                    //Default size 
                    width = 450;
                    height = 400;
                }
                    
                    string img = Upload(image,width,height);
                    ViewData["ImageName"] = img;//image Name after save in to the wwwroot/images for Write in to the data base
            }

            return Page();
        }

        public string Upload(IFormFile file, int width, int height)
        {

            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };//separate the image extensions
            var uploadsRootFolder = Path.Combine("images/"); //set root for the save

            var ext = Path.GetExtension(file.FileName);
            if (allowedExtensions.Contains(ext))
            {
                string path = Path.Combine(this.Environment.WebRootPath, uploadsRootFolder);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(file.FileName);

                using var im = Image.Load(file.OpenReadStream());
                im.Mutate(x => x.Resize(width, height));
                im.Save(Path.Combine(path, fileName));


                return fileName;
            }
            return null;
        }
    }
}