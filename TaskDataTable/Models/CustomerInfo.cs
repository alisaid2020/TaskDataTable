using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskDataTable.Models
{
    public class CustomerInfo
    {
        [Key]
        public int Id { get; set; }
        public string CusId { get; set; }

        public string CusName { get; set; }

        public int notification { get; set; }
        public bool? Status { get; set; }

    }
}