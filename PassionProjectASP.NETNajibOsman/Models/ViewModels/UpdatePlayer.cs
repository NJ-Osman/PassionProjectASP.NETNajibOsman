using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectASP.NETNajibOsman.Models.ViewModels
{
    public class UpdatePlayer
    {
        //This viewmodel is a class which stores information that we need to present to /Player/Update/{}

        //the existing player information

        public PlayerDto SelectedPlayer { get; set; }

        // all species to choose from when updating this animal

        public IEnumerable<CommunityDto> CommunityOptions { get; set; }
    }
}