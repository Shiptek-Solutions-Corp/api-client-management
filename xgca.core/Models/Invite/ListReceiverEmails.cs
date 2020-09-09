using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace xgca.core.Models.Invite
{
    public class ListReceiverEmails
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public List<string> Receivers { get; set; }
    }
}
