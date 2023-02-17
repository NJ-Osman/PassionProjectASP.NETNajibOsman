using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProjectASP.NETNajibOsman.Models;

namespace PassionProjectASP.NETNajibOsman.Controllers
{
    public class PlayerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all players in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all players in the database, including their associated community.
        /// </returns>
        /// <example>
        /// GET: api/AnimalData/ListPlayers
        /// </example>
        [HttpGet]
        public IEnumerable<PlayerDto> ListPlayers()
        {
            List<Player> Players = db.Players.ToList();
            List<PlayerDto> PlayerDtos = new List<PlayerDto>();

            Players.ForEach(p => PlayerDtos.Add(new PlayerDto()
            {
                PlayerID = p.PlayerID,
                PlayerName = p.PlayerName,
                PlayerBio = p.PlayerBio,
                CommunityName = p.Community.CommunityName
            }));
                return PlayerDtos;
        }

        /// <summary>
        /// Gathers information about all players related to a particular community ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all players in the database, including their associated community matched with a particular community ID
        /// </returns>
        /// <param name="id">Community ID.</param>
        /// <example>
        /// GET: api/PlayerData/ListPlayerForCommunity/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PlayerDto))]
        public IHttpActionResult ListPlayerForCommunity(int id)
        {
            List<Player> Players = db.Players.Where(p => p.CommunityID == id).ToList();
            List<PlayerDto> PlayerDtos = new List<PlayerDto>();

            Players.ForEach(p => PlayerDtos.Add(new PlayerDto()
            {
                PlayerID = p.PlayerID,
                PlayerName = p.PlayerName,
                PlayerBio = p.PlayerBio,
                CommunityID = p.Community.CommunityID,
                CommunityName = p.Community.CommunityName
            }));

            return Ok(PlayerDtos);
        }

        /// <summary>
        /// Gathers information about players related to a particular game
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all players in the database, including their associated community that match to a particular keeper id
        /// </returns>
        /// <param name="id">Community ID.</param>
        /// <example>
        /// GET: api/AnimalData/ListPlayersForGame/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PlayerDto))]
        public IHttpActionResult ListPlayersForGame(int id)
        {
            //all players that have games which match with our ID
            List<Player> Players = db.Players.Where(
                p => p.Games.Any(
                    g => g.GameID == id
                )).ToList();
            List<PlayerDto> PlayerDtos = new List<PlayerDto>();

            Players.ForEach(p => PlayerDtos.Add(new PlayerDto()
            {
                PlayerID = p.PlayerID,
                PlayerName = p.PlayerName,
                PlayerBio = p.PlayerBio,
                CommunityID = p.Community.CommunityID,
                CommunityName = p.Community.CommunityName
            }));

            return Ok(PlayerDtos);
        }

        /// <summary>
        /// Associates a particular game with a particular player
        /// </summary>
        /// <param name="playerid">The player ID primary key</param>
        /// <param name="gameid">The game ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/PlayerData/AssociatePlayerWithGame/9/1
        /// </example>
        [HttpPost]
        [Route("api/PlayerData/AssociatePlayerWithGame/{playerid}/{gameid}")]
        public IHttpActionResult AssociatePlayerWithGame(int playerid, int gameid)
        {

            Player SelectedPlayer = db.Players.Include(p => p.Games).Where(p => p.PlayerID == playerid).FirstOrDefault();
            Game SelectedGame = db.Games.Find(gameid);

            if (SelectedPlayer == null || SelectedGame == null)
            {
                return NotFound();
            }

            SelectedPlayer.Games.Add(SelectedGame);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular game and a particular player
        /// </summary>
        /// <param name="animalid">The player ID primary key</param>
        /// <param name="gameid">The game ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/AnimalData/AssociatePlayerWithGame/9/1
        /// </example>
        [HttpPost]
        [Route("api/PlayerData/UnAssociatePlayerWithGame/{playerid}/{gameid}")]
        public IHttpActionResult UnAssociatePlayerWithGame(int playerid, int gameid)
        {

            Player SelectedPlayer = db.Players.Include(p => p.Games).Where(p => p.PlayerID == playerid).FirstOrDefault();
            Game SelectedGame = db.Games.Find(gameid);

            if (SelectedPlayer == null || SelectedGame == null)
            {
                return NotFound();
            }

            SelectedPlayer.Games.Remove(SelectedGame);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns all players in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An player in the system matching up to the player ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the player</param>
        /// <example>
        /// GET: api/PlayerData/FindPlayer/5
        /// </example>
        [ResponseType(typeof(Player))]
        [HttpGet]
        public IHttpActionResult FindPlayer(int id)
        {
            Player Player = db.Players.Find(id);
            PlayerDto PlayerDto = new PlayerDto()
            {
                PlayerID = Player.PlayerID,
                PlayerName = Player.PlayerName,
                PlayerBio = Player.PlayerBio,
                CommunityName = Player.Community.CommunityName
            };
            if (Player == null)
            {
                return NotFound();
            }

            return Ok(PlayerDto);
        }

        /// <summary>
        /// Updates a particular player in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the player ID primary key</param>
        /// <param name="animal">JSON FORM DATA of an player</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/PlayerData/UpdatePlayer/5
        /// FORM DATA: Player JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePlayer(int id, Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != player.PlayerID)
            {
                return BadRequest();
            }

            db.Entry(player).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an player to the system
        /// </summary>
        /// <param name="animal">JSON FORM DATA of an player</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Player ID, Player Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/PlayerData/AddPlayer
        /// FORM DATA: Player JSON Object
        /// </example>
        [ResponseType(typeof(Player))]
        [HttpPost]
        public IHttpActionResult AddPlayer(Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Players.Add(player);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = player.PlayerID }, player);
        }

        /// <summary>
        /// Deletes an player from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the player</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PlayerData/DeletePlayer/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Player))]
        [HttpPost]
        public IHttpActionResult DeletePlayer(int id)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }

            db.Players.Remove(player);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerExists(int id)
        {
            return db.Players.Count(e => e.PlayerID == id) > 0;
        }
    }
}