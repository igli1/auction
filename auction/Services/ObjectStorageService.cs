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
    

    public ObjectStorageService(IOptions<MinioConfiguration> options, IHttpClientFactory httpClientFactory, ILogger<ObjectStorageService> logger)
    {
        _logger = logger;
        _minioConfiguration = options.Value;
        
        _minioClient = new MinioClient()
            .WithEndpoint(_minioConfiguration.Endpoint)
            .WithCredentials(_minioConfiguration.AccessKey, _minioConfiguration.SecretKey)
            .WithSSL()
            .Build();
    }

    public async Task<Stream> GetFileAsync(string objectKey)
    {
        try
        {
            IServerSideEncryption sse = null;
            var contentType = "application/octet-stream";
            var stream = new MemoryStream();

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
            return new MemoryStream();
        }
    }
    
    public async Task UploadFileAsync(string objectKey, Stream fileStream)
    {
        try
        {
            var contentType = "application/octet-stream";
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_minioConfiguration.BucketName)
                .WithObject(objectKey)
                .WithStreamData(fileStream)
                .WithContentType(contentType);
            
            await _minioClient.PutObjectAsync(putObjectArgs);
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex,"Error uploading the file to MinIO");
        }
    }
}