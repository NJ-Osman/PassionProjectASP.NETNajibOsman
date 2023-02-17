using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProjectASP.NETNajibOsman.Models
{
    public class Player
    {
        [Key]
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string PlayerBio { get; set; }

        [ForeignKey("Community")]
        public int CommunityID { get; set; }
        public virtual Community Community { get; set; }

        public ICollection<Game> Games { get; set; }
    }

    public class PlayerDto
    {
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string PlayerBio { get; set; }
        public int CommunityID { get; set; }
        public string CommunityName { get; set; }
    }

}