using PassionProjectASP.NETNajibOsman.Models;
using PassionProjectASP.NETNajibOsman.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PassionProjectASP.NETNajibOsman.Controllers
{
    public class GameController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static GameController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/gamedata/");
        }
        // GET: Game/List
        public ActionResult List()
        {
            string url = "listgame";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<GameDto> Game = response.Content.ReadAsAsync<IEnumerable<GameDto>>().Result;
            return View(Game);
        }

        // GET: Game/Details/5
        public ActionResult Details(int id)
        {

            DetailsGame ViewModel = new DetailsGame();

            string url = "communitydata/findgame/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            GameDto SelectedGame = response.Content.ReadAsAsync<GameDto>().Result;

            ViewModel.SelectedGame = SelectedGame;

            url = "playerdata/listplayersforgame/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PlayerDto> PlayedGames = response.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;

            ViewModel.PlayedGames = PlayedGames;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        //GET: Community/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Game/Create
        [HttpPost]
        public ActionResult Create(Game Game)
        {

            //objective: add a new Game into our system using the API
            //curl -H "Content-Type:application/json" -d @Species.json https://localhost:44324/api/GameData/addGa,e 
            string url = "gamedata/addgame";

            string jsonpayload = jss.Serialize(Game);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Game/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "gamedata/findgame/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GameDto selectedGame = response.Content.ReadAsAsync<GameDto>().Result;
            return View(selectedGame);
        }

        // POST: Game/Update/5
        [HttpPost]
        public ActionResult Edit(int id, Game Game)
        {
            string url = "gamedata/updategame/" + id;
            string jsonpayload = jss.Serialize(Game);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Game/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "gamedata/findgame/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GameDto selectedGame = response.Content.ReadAsAsync<GameDto>().Result;
            return View(selectedGame);
        }

        // POST: Game/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "gameedata/deletegame/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
