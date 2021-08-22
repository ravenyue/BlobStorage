using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlobStorage.Samples.Dtos
{
    public class BlobDto
    {
        [Required]
        public string BucketName { get; set; }

        [Required]
        public string BlobName { get; set; }
    }
}
