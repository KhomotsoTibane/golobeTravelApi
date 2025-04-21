namespace GolobeTravelApi.Services
{
    public class ImageService
    {
        private readonly string _bucketName;
        private readonly string _region;
        private readonly string _folder;

        public ImageService(IConfiguration configuration)
        {
            _bucketName = configuration["AWS:BucketName"] ?? "glb-s3-images";
            _region = configuration["AWS:Region"] ?? "af-south-1";
            _folder = configuration["AWS:ImageFolder"] ?? "images";
            
        }

        public string GetEntityImageUrl(string entityName, string extension = "jpg")
        {
            if (string.IsNullOrWhiteSpace(entityName)) return null;


            var lowerFileName = $"{entityName.ToLowerInvariant()}.{extension}";
            var encodedFileName = Uri.EscapeDataString(lowerFileName);

            var path = string.IsNullOrWhiteSpace(_folder)
                ? encodedFileName
                : $"{_folder}/{encodedFileName}";

            return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{path}";
        }
    }
}
