using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace xas.core.Request.DTO
{
    public class Cus_DeleteBulk
    {
        [Required]
        public List<Guid> RequestId { get; set; }
    }
}
