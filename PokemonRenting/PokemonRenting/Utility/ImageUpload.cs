namespace PokemonRenting.Web.Utility
{
    public class ImageUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string SaveImageFile(IFormFile pokemonImageUrl)
        {
           
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = Path.Combine(webRootPath, "upload");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }


                string fileName = Guid.NewGuid().ToString() +
                    Path.GetExtension(pokemonImageUrl.FileName);
                string filePath = Path.Combine(uploadPath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    pokemonImageUrl.CopyTo(fileStream);
                }
                return Path.Combine("upload", fileName);
         
        }
    }
}
