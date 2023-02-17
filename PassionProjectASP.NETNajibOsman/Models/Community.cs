using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProjectASP.NETNajibOsman.Models
{
    public class Community
    {
        [Key]
        public int CommunityID { get; set; }
        public string CommunityName { get; set; }
        public string CommunityBio { get; set; }
    }

    public class CommunityDto
    {
        public int CommunityID { get; set; }
        public string CommunityName { get; set; }
        public string CommunityBio { get; set; }
    }

}