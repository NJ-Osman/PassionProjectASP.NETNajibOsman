using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProjectASP.NETNajibOsman.Models
{
    public class Game
    {
        [Key]
        public int GameID { get; set; }
        public string GameName { get; set; }
        public string GameDescription { get; set; }

        public ICollection<Player> Players { get; set; }
    }

        public class GameDto 
        {
            public int GameID { get; set; }
            public string GameName { get; set; }
            public string GameDescription { get; set; }
        }
}