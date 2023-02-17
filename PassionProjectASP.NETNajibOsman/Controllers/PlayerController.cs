using PassionProjectASP.NETNajibOsman.Models;
using PassionProjectASP.NETNajibOsman.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static PassionProjectASP.NETNajibOsman.Models.Game;

namespace PassionProjectASP.NETNajibOsman.Controllers
{
    public class PlayerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PlayerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/");
        }

        // GET: Player/List
        public ActionResult List()
        {
            //objective:: communicate with our player data api to retrieve a list of players
            //curl https://localhost:44341/api/playerdata/listplayers

            string url = "player/listplayers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PlayerDto> players = response.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;
           //Debug.WriteLine("Number of players received: ");
            //Debug.WriteLine(players.Count());

            return View(players);
        }

        // GET: Player/Details/5
        public ActionResult Details(int id)
        {

            DetailsPlayer ViewModel = new DetailsPlayer();

            //objective:: communicate with our player data api to retrieve a one player
            //curl https://localhost:44341/api/playerdata/findplayer/{id}

            string url = "playerdata/findplayer/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            PlayerDto Selectedplayer = response.Content.ReadAsAsync<PlayerDto>().Result;
            Debug.WriteLine("player received: ");
            Debug.WriteLine(Selectedplayer.PlayerName);

            ViewModel.SelectedPlayer = Selectedplayer ;

            url = "gamedata/listgamesforplayer/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<GameDto> ResponsibleGames = response.Content.ReadAsAsync<IEnumerable<GameDto>>().Result;

            ViewModel.ResponsibleKeepers = ResponsibleGames;

            url = "gamedata/listgamesnotplayingforplayer/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<GameDto> AvailableGames = response.Content.ReadAsAsync<IEnumerable<GameDto>>().Result;

            ViewModel.AvailableGames = AvailableGames;


            return View(ViewModel);

        }

        //POST: Game/Associate/{gameid}
        [HttpPost]
        public ActionResult Associate(int id, int GameID)
        {
            Debug.WriteLine("Attempting to associate player :" + id + " with game " + GameID);

            //call our api to associate player with game
            string url = "playerdata/associateplayerwithgame/" + id + "/" + GameID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Game/UnAssociate/{id}?Game={gameID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int GameID)
        {
            Debug.WriteLine("Attempting to unassociate player :" + id + " with game: " + GameID);

            //call our api to associate player with game
            string url = "animaldata/unassociateplayerwithgame/" + id + "/" + GameID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Player/New
        public ActionResult New()
        {
            //Information about all communities in the system.
            //Get api/communitydata/listcommunity

            string url = "communitydata/listcommunity";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CommunityDto> CommunityOptions = response.Content.ReadAsAsync<IEnumerable<CommunityDto>>().Result;

            return View(CommunityOptions);
        }

        // POST: Player/Create
        [HttpPost]
        public ActionResult Create(Player player)
        {
            Debug.WriteLine("The inputted player is :");
            Debug.WriteLine(player.PlayerName);
            //add a new player onto our system using the api
            //curl -d @player.json -H "Content-Type:application/json" https://localhost:44341/api/playerdata/addplayer
            string url = "playerdata/addplayer";

            string jsonpayload = jss.Serialize(player);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            } else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Player/Edit/5
        public ActionResult Edit(int id)
        {
            UpdatePlayer ViewModel = new UpdatePlayer();

            string url = "playerdata/findplayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDto SelectedPlayer = response.Content.ReadAsAsync<PlayerDto>().Result;
            ViewModel.SelectedPlayer = SelectedPlayer;

            url = "communitydata/listcommunity/";
            response = client.GetAsync(url).Result;
            IEnumerable<CommunityDto> CommunityOptions = response.Content.ReadAsAsync<IEnumerable<CommunityDto>>().Result;

            ViewModel.CommunityOptions = CommunityOptions;

            return View(ViewModel);
        }

        // POST: Player/Update/5
        [HttpPost]
        public ActionResult Edit(int id, Player player)
        {
            string url = "playerdata/updateplayer/" +id;
            string jsonpayload = jss.Serialize(player);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            } else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Player/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "playerdata/findplayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDto selectedplayer = response.Content.ReadAsAsync<PlayerDto>().Result;
            return View(selectedplayer);
        }

        // POST: Player/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "playerdata/deleteplayer/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            } else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
