# BlobStorage
Standard BLOB Storage library in .NET Core.  

The library provides an abstraction of BLOB object storage and integrates multiple storage providers.  

You can easily change your BLOB storage without changing your application code. 

The project is inspired by [ABP BLOB Storing](https://docs.abp.io/en/abp/latest/Blob-Storing) and can be used independently of the abp framework.
## BLOB Storage Providers

The library has already the following storage provider implementations:

* [Aliyun](#Configure-AliyunOss-Porvider): Stores BLOBs on the [Aliyun Storage Service](https://help.aliyun.com/product/31815.html).
* [Azure](#Configure-AzureBlob-Porvider): Stores BLOBs on the [Azure BLOB storage](https://azure.microsoft.com/en-us/services/storage/blobs/).
* [Aws](#Configure-AmazonS3-Porvider): Stores BLOBs on the [Amazon Simple Storage Service](https://aws.amazon.com/s3/).
* [Minio](#Configure-Minio-Porvider): Stores BLOBs on the [MinIO Object storage](https://min.io/).
* [File System](#Configure-FileSystem-Porvider): Stores BLOBs in a folder of the local file system, as standard files.

Multiple providers **can be used together** by the help of the **container system**, where each container can uses a different provider.

## Install
To install `BlobStorage` package, run the following command in Nuget Package Manager Console.
```
PM> Install-Package RavenYu.BlobStorage
```
Choose to install provider you needed
```
PM> Install-Package RavenYu.BlobStorage.AliyunOss
PM> Install-Package RavenYu.BlobStorage.AmazonS3
PM> Install-Package RavenYu.BlobStorage.AzureBlob
PM> Install-Package RavenYu.BlobStorage.Minio
PM> Install-Package RavenYu.BlobStorage.FileSystem
```

## Getting Started

### Configure default container
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlobStorage(options =>
    {
        options.ConfigureDefaultContainer(container =>
        {
            // Choose to add provider you needed
            container.UseAliyunOss(o => { ... });
            container.UseAzureBlob(o => { ... });
            container.UseFileSystem(o => { ... });
            container.UseAmazonS3(o => { ... });
            container.UseMinio(o => { ... });
        });
    });

    // Or another way to configure
    services.AddBlobStorage()
        .AddAliyunOss(o => { ... });
        //.AddAzureBlob(o => { });
        //.AddFileSystem(o => { });
        //.AddAmazonS3(o => { });
        //.AddMinio(o => { });
}
```
Inject and use the `Iblobcontainer`
```csharp
public class SomeService
{
    private readonly IBlobContainer _blobContainer;

    public SomeService(IBlobContainer blobContainer)
    {
        _blobContainer = blobContainer;
    }

    public async Task SaveAsync(Stream stream)
    {
        await _blobContainer.SaveAsync("blob-name.jpg", stream);
    }

    public async Task GetAsync()
    {
        await _blobContainer.GetAsync("blob-name.jpg");
    }

    public async Task DeleteAsync()
    {
        await _blobContainer.DeleteAsync("blob-name.jpg");
    }
}
```

### Typed IBlobContainer
Typed BLOB container system is a way of creating and managing multiple containers in an application;

To create a typed container, you need to create a simple class decorated with the `BlobContainerName` attribute:

```csharp
[BlobContainerName("pictures")]
public class PicturesContainer
{
    
}
```
Startup.cs
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlobStorage(options =>
    {
        options.ConfigureContainer<PicturesContainer>(container =>
        {
            container.UseAliyunOss(o => { ... });
        });
    });

    // Or another way to configure
    services.AddBlobStorage()
        .AddAliyunOss<PicturesContainer>(o => { ... });
}
```

Once you create the container class, you can inject `IBlobContainer<T>` for your container type.

```csharp
public class SomeService
{
    private readonly IBlobContainer<PicturesContainer>  _blobContainer;

    public SomeService(IBlobContainer<PicturesContainer> blobContainer)
    {
        _blobContainer = blobContainer;
    }

    public async Task SaveAsync(Stream stream)
    {
        await _blobContainer.SaveAsync("blob-name.jpg", stream);
    }

    public async Task GetAsync()
    {
        await _blobContainer.GetAsync("blob-name.jpg");
    }
}
```
### The Default Container
If you don't use the generic argument and directly inject the `IBlobContainer` (as explained before), you get the default container. Another way of injecting the default container is using `IBlobContainer<DefaultContainer>`, which returns exactly the same container.

The name of the default container is `default`.

### Named Containers
Typed containers are just shortcuts for named containers. You can inject and use the `IBlobContainerFactory` to get a BLOB container by its name:

Startup.cs
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlobStorage(options =>
    {
        options.ConfigureContainer("pictures",container =>
        {
            container.UseAliyunOss(o => { ... });
        });
    });

    // Or another way to configure
    services.AddBlobStorage()
        .AddAliyunOss("pictures", o => { ... });
}
```
Inject and use the `IBlobContainerFactory`
```csharp
public class SomeService
{
    private readonly IBlobContainer _blobContainer;

    public SomeService(IBlobContainerFactory blobContainerFactory)
    {
        _blobContainer = blobContainerFactory.Create("pictures");
    }
}
```

### IBlobContainerFactory
`IBlobContainerFactory` is the service that is used to create the BLOB containers. One example was shown above.

Example: Create a container by name
```csharp
var blobContainer = blobContainerFactory.Create("pictures");
```

Example: Create a container by type
```csharp
var blobContainer = blobContainerFactory.Create<PicturesContainer>();
```

### The Bucket Name
The default bucket name is the container name, you can use it as a container mapped to a bucket

```csharp
var blobContainer = blobContainerFactory.Create("pictures");
// Save to "pictures" bucket, blob name is "avatar.jpg"
await blobContainer.SaveAsync("avatar.jpg", stream);
```
You can also ignore the container name and use the specified bucket name
```csharp
// Save to "user-pictures" bucket, blob name is "avatar.jpg"
await blobContainer.SaveAsync("user-pictures", "avatar.jpg", stream);
```

### Configure AliyunOss Porvider
```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBlobStorage()
            .AddAliyunOss(options =>
            {
                options.AccessKeyId = "your aliyun access key id";
                options.AccessKeySecret = "your aliyun access key secret";
                options.Endpoint = "your oss endpoint";
                options.CreateBucketIfNotExists = true;
            });
        
        //Or configure from appsettings.json
        services.AddBlobStorage()
            .AddAliyunOss(Configuration.GetSection("Aliyun"))
    }
}
```


#### Options

* **AccessKeyId** ([NotNull]string): AccessKey is the key to access the Alibaba Cloud API. It has full permissions for the account. Please keep it safe! Recommend to follow [Alibaba Cloud security best practicess](https://help.aliyun.com/document_detail/102600.html),Use RAM sub-user AccessKey to call API.
* **AccessKeySecret** ([NotNull]string): Same as above.
* **Endpoint** ([NotNull]string): Endpoint is the external domain name of OSS. See the [document](https://help.aliyun.com/document_detail/31837.html) for details. 
* **CreateBucketIfNotExists** (bool): Default value is `false`, If a bucket does not exist in Aliyun, `AliyunOssBlobProvider` will try to create it.

### Configure AzureBlob Porvider
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlobStorage()
        .AddAzureBlob(options =>
        {
            options.ConnectionString = "your azure connection string";
            options.CreateBucketIfNotExists = true;
        });
}
```

### Options

* **ConnectionString** (string): A connection string includes the authorization information required for your application to access data in an Azure Storage account at runtime using Shared Key authorization. Please refer to Azure documentation: https://docs.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string
* **CreateBucketIfNotExists** (bool): Default value is `false`, If a bucket does not exist in azure, `AzureBlobProvider` will try to create it.


### Configure AmazonS3 Porvider
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlobStorage()
        .AddAmazonS3(options =>
        {
            options.AccessKeyId = "your Aws access key id";
            options.SecretAccessKey = "your Aws access key secret";
            options.Region = "the system name of the service";
            options.CreateBucketIfNotExists = true;
        });
}
```
### Options

* **AccessKeyId** (string): AWS Access Key ID.
* **SecretAccessKey** (string): AWS Secret Access Key.
* **Region** (string): The system name of the service.
* **CreateBucketIfNotExists** (bool): Default value is `false`, If a bucket does not exist in Aws, `AmazonS3BlobProvider` will try to create it.


### Configure Minio Porvider
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlobStorage()
        .AddMinio(options =>
        {
            options.Endpoint = "your minio endpoint";
            options.AccessKey = "your minio accessKey";
            options.SecretKey = "your minio secretKey";
            options.WithSSL = false;
            options.CreateBucketIfNotExists = true;
        });
}
```

### Options

* **Endpoint** (string): URL to object storage service. Please refer to MinIO Client SDK for .NET: https://docs.min.io/docs/dotnet-client-quickstart-guide.html
* **AccessKey** (string): Access key is the user ID that uniquely identifies your account. 
* **SecretKey** (string): Secret key is the password to your account.
* **WithSSL** (bool): Default value is `false`,Chain to MinIO Client object to use https instead of http.
* **CreateBucketIfNotExists** (bool): Default value is `false`, If a bucket does not exist in minio, `MinioBlobProvider` will try to create it.


### Configure FileSystem Porvider
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlobStorage()
        .AddFileSystem(aliyun =>
        {
            options.BasePath = "C:\\my-files" 
            options.AppendBucketNameToBasePath = true;
        });
}
```

### Options

* **BasePath** (string): The base folder path to store BLOBs. It is required to set this option.
* **AppendBucketNameToBasePath** (bool; default: `true`): Indicates whether to create a folder with the bucket name inside the base folder. If you store multiple buckets in the same `BaseFolder`, leave this as `true`. Otherwise, you can set it to `false` if you don't like an unnecessarily deeper folder hierarchy.

## Supported APIs

`IBlobContainer` is the main interface to store and read BLOBs.

### Saving BLOBs
**SaveAsync** method is used to save a new BLOB or replace an existing BLOB. It can save a **Stream** by default, but there is a shortcut extension method to save byte arrays.

**SaveAsync** gets the following parameters:

* **bucketName** (string): Bucket name, Default is the container name.
* **blobName** (string): Unique name of the BLOB.
* **stream** (Stream) or **bytes** (byte[]): The stream to read the BLOB content or a byte array.
* **overrideExisting** (bool): Default value is `true` and replace the BLOB content if it does already exists. Set `false` will throw a `BlobAlreadyExistsException` if there is already a BLOB in the bucket with the same name..

### Reading BLOBs

* `GetAsync`: Gets a BLOB name and returns a `BlobResponse`, object contains BLOB content and metadata. This method throws `BlobNotFoundException`, if it can not find the BLOB with the given name.
* `GetOrNullAsync`: In opposite to the `GetAsync` method, this one returns `null` if there is no BLOB found with the given name.
* `GetAllBytesAsync`: Returns a `byte[]` instead of a `Stream`. Still throws exception if can not find the BLOB with the given name.
* `GetAllBytesOrNullAsync`: In opposite to the `GetAllBytesAsync` method, this one returns `null` if there is no BLOB found with the given name.
* `StatAsync`: Get blob metadata return `BlobStat` object, This method throws `BlobNotFoundException`, if it can not find the BLOB with the given name.
* `StatOrNullAsync`: In opposite to the `StatAsync` method, this one returns `null` if there is no BLOB found with the given name.

### Deleting BLOBs

`DeleteAsync` method gets a BLOB name and deletes the BLOB data. It doesn't throw any exception if given BLOB was not found. Instead, it returns a `bool` indicating that the BLOB was actually deleted or not, if you care about it.

### Other Methods

* `ExistsAsync` method simply checks if there is a BLOB in the bucket with the given name.
