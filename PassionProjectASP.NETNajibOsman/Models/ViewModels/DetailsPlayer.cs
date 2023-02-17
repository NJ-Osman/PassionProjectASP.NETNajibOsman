using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PassionProjectASP.NETNajibOsman.Models.Game;

namespace PassionProjectASP.NETNajibOsman.Models.ViewModels
{
    public class DetailsPlayer
    {

        public PlayerDto SelectedPlayer { get; set; }
        public IEnumerable<GameDto> ResponsibleGames { get; set; }

        public IEnumerable<GameDto> AvailableGames { get; set; }
    }
}