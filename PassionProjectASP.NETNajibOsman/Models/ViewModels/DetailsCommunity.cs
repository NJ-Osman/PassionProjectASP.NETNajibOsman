using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectASP.NETNajibOsman.Models.ViewModels
{
    public class DetailsCommunity
    {
        //the species itself that we want to display
        public CommunityDto SelectedCommunity { get; set; }

        //all of the related animals to that particular species
        public IEnumerable<PlayerDto> RelatedPlayers { get; set; }
    }
}