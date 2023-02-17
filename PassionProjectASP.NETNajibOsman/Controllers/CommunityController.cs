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
    public class CommunityController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CommunityController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/communitydata/");
        }

        // GET: Community/List
        public ActionResult List()
        {
            string url = "listcommunity";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CommunityDto> Community = response.Content.ReadAsAsync<IEnumerable<CommunityDto>>().Result;
            return View(Community);
        }

        // GET: Community/Details/5
        public ActionResult Details(int id)
        {
            DetailsCommunity ViewModel = new DetailsCommunity();

            string url = "communitydata/findcommunity/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CommunityDto SelectedCommunity = response.Content.ReadAsAsync<CommunityDto>().Result;

            ViewModel.SelectedCommunity = SelectedCommunity;

            url = "playerdata/listplayersforcommunity/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PlayerDto> RelatedPlayers = response.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;

            ViewModel.RelatedPlayers = RelatedPlayers;

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

        // GET: Community/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "communitydata/findcommunity/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CommunityDto selectedCommunity = response.Content.ReadAsAsync<CommunityDto>().Result;
            return View(selectedCommunity);
        }


        // POST: Community/Create
        [HttpPost]
        public ActionResult Create(Community Community)
        {
            //objective: add a new Community into our system using the API
            //curl -H "Content-Type:application/json" -d @Species.json https://localhost:44324/api/CommunityData/addCommunity 
            string url = "communitydata/addcommunity";

            string jsonpayload = jss.Serialize(Community);

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

        // POST: Community/Update/5
        [HttpPost]
        public ActionResult Update(int id, Community Community)
        {

            string url = "communitydata/updatecommunity/" + id;
            string jsonpayload = jss.Serialize(Community);
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

        // GET: Community/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "communitydata/findcommunity/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CommunityDto selectedCommunity = response.Content.ReadAsAsync<CommunityDto>().Result;
            return View(selectedCommunity);
        }

        // POST: Community/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "communityData/deletcommunity/" + id;
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
