namespace TaskDataTable.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class StudentDegrees_kw
    {
        public int ID { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage ="You Must Enter StudntName")]
        public string StudentName { get; set; }
        [Required(ErrorMessage ="bnsansnasasnm")]
        public int? SchoolID { get; set; }

        [StringLength(500)]
        public string SchoolName { get; set; }

        public decimal? Percentage { get; set; }

        [StringLength(100)]
        public string Nationality { get; set; }

        //[Remote("ChckEmail","Home",AdditionalFields ="ID",ErrorMessage ="Email Already Exists ")]
        [Remote("IsAlreadyExistEmail", "Home", AdditionalFields = "ID", ErrorMessage = "EmailId already exists in database.")]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "You Must Enter Email")]
        public string Email { get; set; }

        [StringLength(150)]
        public string UserName { get; set; }

        [StringLength(250)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool? IS_Deleted { get; set; }

        public string ProfilePic { get; set; }
    }
}
