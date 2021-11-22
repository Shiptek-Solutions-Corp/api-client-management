using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace xas.core.Request.DTO
{
    public class Cus_UpdateBulk
    {
        public List<Guid> requestId { get; set; }
        public string status { get; set; }
    }
}
