using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PassionProjectASP.NETNajibOsman.Models.Game;

namespace PassionProjectASP.NETNajibOsman.Models.ViewModels
{
    public class DetailsGame
    {
        public GameDto SelectedGame { get; set; }
        public IEnumerable<PlayerDto> PlayedGames { get; set; }
    }
}