# BlobStorage
Generic BLOB Storage library in .NET Core

# BLOB Storage Providers

The library has already the following storage provider implementations:

* [Aliyun](Blob-Storing-Aliyun.md): Stores BLOBs on the [Aliyun Storage Service](https://help.aliyun.com/product/31815.html).
* [Azure](Blob-Storing-Azure.md): Stores BLOBs on the [Azure BLOB storage](https://azure.microsoft.com/en-us/services/storage/blobs/).
* [Aws](Blob-Storing-Aws.md): Stores BLOBs on the [Amazon Simple Storage Service](https://aws.amazon.com/s3/).
* [Minio](Blob-Storing-Minio.md): Stores BLOBs on the [MinIO Object storage](https://min.io/).
* [File System](Blob-Storing-File-System.md): Stores BLOBs in a folder of the local file system, as standard files.

# Aliyun Provider
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlobStorage()
        .AddAliyunOss<AliyunOssContainer>(aliyun =>
        {
            aliyun.AccessKeyId = "your aliyun access key id";
            aliyun.AccessKeySecret = "your aliyun access key secret";
            aliyun.Endpoint = "your oss endpoint";
            aliyun.CreateContainerIfNotExists = true;
        });
}
```