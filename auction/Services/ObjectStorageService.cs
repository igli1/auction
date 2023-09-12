using auction.Configuration;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;

namespace auction.Services;

public class ObjectStorageService
{
    private static MinioClient _minioClient;
    private readonly MinioConfiguration _minioConfiguration;
    private readonly ILogger<ObjectStorageService> _logger;
    private const string DefaultProduct = "product.jpg";
    private const string DefaultProfile = "Profile.webp";
    public ObjectStorageService(IOptions<MinioConfiguration> options, ILogger<ObjectStorageService> logger)
    {
        _logger = logger;
        _minioConfiguration = options.Value;
        
        _minioClient = new MinioClient()
            .WithEndpoint(_minioConfiguration.Endpoint)
            .WithCredentials(_minioConfiguration.AccessKey, _minioConfiguration.SecretKey)
            .WithSSL(_minioConfiguration.UseSSL)
            .Build();
        
    }

    public async Task<Stream> GetFileAsync(string objectKey, bool isProfile)
    {
        try
        {
            IServerSideEncryption sse = null;
            var contentType = "application/octet-stream";
            var stream = new MemoryStream();
            
            if(objectKey == String.Empty && isProfile)
                objectKey = DefaultProfile;
            else if (objectKey == String.Empty)
                objectKey = DefaultProduct;
            
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_minioConfiguration.BucketName)
                .WithObject(objectKey)
                .WithServerSideEncryption(sse)
                .WithCallbackStream((s) =>
                {
                    s.CopyTo(stream);
                });
            
            await _minioClient.GetObjectAsync(getObjectArgs).ConfigureAwait(false);

            stream.Position = 0;
            
            return stream;
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex,"Error getting the file from MinIO");
            return null;
        }
    }
    
    public async Task<bool> UploadFileAsync(string objectKey, Stream fileStream)
    {
        try
        {
            var bktExistArg = new BucketExistsArgs().WithBucket(_minioConfiguration.BucketName);
            bool found = await _minioClient.BucketExistsAsync(bktExistArg);
            if (!found)
            {
                var mkBtExistArg = new MakeBucketArgs().WithBucket(_minioConfiguration.BucketName);
                await _minioClient.MakeBucketAsync(mkBtExistArg);

            }
            else
            {
                var contentType = "application/octet-stream";
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(_minioConfiguration.BucketName)
                    .WithObject(objectKey)
                    .WithStreamData(fileStream)
                    .WithContentType(contentType)
                    .WithObjectSize(fileStream.Length);
            
                await _minioClient.PutObjectAsync(putObjectArgs);
            }
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex,"Error uploading the file to MinIO");
            return false;
        }

        var statObjectArgs = new StatObjectArgs()
            .WithBucket(_minioConfiguration.BucketName)
            .WithObject(objectKey);
        
        var objectStat = await _minioClient.StatObjectAsync(statObjectArgs);
        if (objectStat != null)
            return true;
        return false;
    }
}