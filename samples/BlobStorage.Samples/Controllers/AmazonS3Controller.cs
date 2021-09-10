using BlobStorage.Samples.Containers;
using BlobStorage.Samples.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace BlobStorage.Samples.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AmazonS3Controller : ControllerBase
    {
        private readonly IBlobContainer<AmazonS3Container> _blobContainer;

        public AmazonS3Controller(IBlobContainer<AmazonS3Container> blobContainer)
        {
            _blobContainer = blobContainer;
        }

        [HttpPost("Blob")]
        public async Task<IActionResult> Save([FromForm] BlobSaveDto dto)
        {
            await _blobContainer.SaveAsync(
                dto.BucketName,
                dto.BlobName,
                dto.Blob.OpenReadStream(),
                dto.OverrideExisting);

            return Content("Saved");
        }

        [HttpGet("Blob")]
        public async Task<IActionResult> GetBlob([FromQuery] BlobDto dto, CancellationToken cancellationToken)
        {
            var response = await _blobContainer.GetOrNullAsync(dto.BucketName, dto.BlobName, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }
            return File(response.Content, MediaTypeNames.Application.Octet, Path.GetFileName(dto.BlobName));
        }

        [HttpDelete("Blob")]
        public async Task<IActionResult> Delete([FromQuery] BlobDto dto, CancellationToken cancellationToken)
        {
            var result = await _blobContainer.DeleteAsync(dto.BucketName, dto.BlobName, cancellationToken);

            return Content(result.ToString());
        }

        [HttpGet("Blob/Metadata")]
        public async Task<IActionResult> GetMetadata([FromQuery] BlobDto dto, CancellationToken cancellationToken)
        {
            var metadata = await _blobContainer.StatOrNullAsync(dto.BucketName, dto.BlobName, cancellationToken);

            if (metadata == null)
            {
                return NotFound();
            }
            return Ok(metadata);
        }

        [HttpGet("Blob/Exists")]
        public async Task<IActionResult> Exists([FromQuery] BlobDto dto, CancellationToken cancellationToken)
        {
            var result = await _blobContainer.ExistsAsync(dto.BucketName, dto.BlobName, cancellationToken);

            return Content(result.ToString());
        }
    }
}
