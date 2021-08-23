using BlobStorage.AliyunOss;
using BlobStorage.FileSystem;
using BlobStorage.Samples.Containers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobStorage.Samples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlobStorage.Samples", Version = "v1" });
                c.EnableAnnotations();
            });

            services.AddBlobStorage(options =>
            {
                // AliyunOss provider
                options.ConfigureContainer<AliyunOssContainer>(container =>
                {
                    container.UseAliyunOss(Configuration.GetSection("Aliyun"));
                    //container.UseAliyunOss(aliyun =>
                    //{
                    //    aliyun.AccessKeyId = "your aliyun access key id";
                    //    aliyun.AccessKeySecret = "your aliyun access key secret";
                    //    aliyun.Endpoint = "your oss endpoint";
                    //});
                });

                // FileSystem provider
                options.ConfigureContainer<FileSystemContainer>(container =>
                {
                    container.UseFileSystem(fileSystem=> 
                    {
                        fileSystem.BasePath = @"E:\my-files";
                    });
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlobStorage.Samples v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
