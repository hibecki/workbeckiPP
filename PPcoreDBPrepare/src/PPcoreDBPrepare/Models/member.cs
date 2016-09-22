using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class member
    {
        public string member_code { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string sex { get; set; }
        public string nationality { get; set; }
        public string mem_photo { get; set; }
        public string cid_type { get; set; }
        public string cid_card { get; set; }
        public string cid_card_pic { get; set; }
        public DateTime? birthdate { get; set; }
        public short? current_age { get; set; }
        public string religion { get; set; }
        public string place_name { get; set; }
        public string marry_status { get; set; }
        public string h_no { get; set; }
        public string lot_no { get; set; }
        public string village { get; set; }
        public string building { get; set; }
        public string floor { get; set; }
        public string room { get; set; }
        public string lane { get; set; }
        public string street { get; set; }
        public string subdistrict_code { get; set; }
        public string district_code { get; set; }
        public string province_code { get; set; }
        public int? country_code { get; set; }
        public string zip_code { get; set; }
        public string mstatus_code { get; set; }
        public string mem_type_code { get; set; }
        public string mem_group_code { get; set; }
        public string mlevel_code { get; set; }
        public string zone { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public string texta_address { get; set; }
        public string textb_address { get; set; }
        public string textc_address { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public string fax { get; set; }
        public string social_app_data { get; set; }
        public string email { get; set; }
        public string parent_code { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
