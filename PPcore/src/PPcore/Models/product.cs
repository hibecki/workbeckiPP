using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPcore.Models
{
    public partial class product
    {
        //[Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "รหัสผลิตผล")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string product_code { get; set; }
        [Display(Name = "ประเภทผลิตผล")]
        public string product_type_code { get; set; }
        [Display(Name = "กลุ่มผลิตผล")]
        public string product_group_code { get; set; }
        [Display(Name = "ผลิตผล")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string product_desc { get; set; }
        [Display(Name = "ลำดับ")]
        public int rec_no { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
