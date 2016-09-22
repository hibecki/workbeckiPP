using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class mem_reward
    {
        public string member_code { get; set; }
        [Display(Name = "ลำดับที่")]
        public int rec_no { get; set; }
        [Display(Name = "ชื่อ / คำอธิบาย")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string reward_desc { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        [HiddenInput]
        public byte[] rowversion { get; set; }
    }
}
