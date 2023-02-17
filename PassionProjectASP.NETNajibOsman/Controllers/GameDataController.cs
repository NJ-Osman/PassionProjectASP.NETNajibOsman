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
using static PassionProjectASP.NETNajibOsman.Models.Game;

namespace PassionProjectASP.NETNajibOsman.Controllers
{
    public class GameDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Games in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Games in the database, including their associated communities.
        /// </returns>
        /// <example>
        /// GET: api/GameData/ListGames
        /// </example>
        [HttpGet]
        public IEnumerable<GameDto> ListGames()
        {
            List<Game> Games = db.Games.ToList();
            List<GameDto> GameDtos = new List<GameDto>();

            Games.ForEach(g => GameDtos.Add(new GameDto()
            {
                GameID = g.GameID,
                GameName = g.GameName,
                GameDescription = g.GameDescription,
            }));
            return GameDtos;
        }

        /// <summary>
        /// Returns all Games in the system associated with a particular player.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Games in the database taking care of a particular players
        /// </returns>
        /// <param name="id">Player Primary Key</param>
        /// <example>
        /// GET: api/GameData/ListGamesForPlayer/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Game))]
        public IHttpActionResult ListGamesForPlayers(int id)
        {
            List<Game> Games = db.Games.Where(
                g=>g.Players.Any(
                    p=>p.PlayerID==id)
                ).ToList();
            List<GameDto> GameDtos = new List<GameDto>();

            Games.ForEach(g => GameDtos.Add(new GameDto()
            {
                GameID = g.GameID,
                GameName = g.GameName,
                GameDescription = g.GameDescription,
            }));

            return Ok(GameDtos);
        }

        /// <summary>
        /// Returns all Games in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Game in the system matching up to the Game ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Game</param>
        /// <example>
        /// GET: api/GameData/FindGame/5
        /// </example>
        [ResponseType(typeof(GameDto))]
        [HttpGet]
        public IHttpActionResult FindGame(int id)
        {
            Game Game = db.Games.Find(id);
            GameDto GameDto = new GameDto()
            {
                GameID = Game.GameID,
                GameName = Game.GameName,
                GameDescription = Game.GameDescription
            };
            if (Game == null)
            {
                return NotFound();
            }

            return Ok(GameDto);
        }

        /// <summary>
        /// Updates a particular Game in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Game ID primary key</param>
        /// <param name="Keeper">JSON FORM DATA of an Game</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/GameData/UpdateGame/5
        /// FORM DATA: Game JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateGame(int id, Game Game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Game.GameID)
            {
                return BadRequest();
            }

            db.Entry(Game).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
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
        /// Adds an Game to the system
        /// </summary>
        /// <param name="Game">JSON FORM DATA of an Game</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Game ID, Game Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/Gamer\Data/AddGame
        /// FORM DATA: Game JSON Object
        /// </example>
        [ResponseType(typeof(Game))]
        public IHttpActionResult AddGame(Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Games.Add(game);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = game.GameID }, game);
        }

        /// <summary>
        /// Deletes an Game from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Game</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/GameData/DeleteGame/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Game))]
        public IHttpActionResult DeleteGame(int id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }

            db.Games.Remove(game);
            db.SaveChanges();

            return Ok(game);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GameExists(int id)
        {
            return db.Games.Count(e => e.GameID == id) > 0;
        }
    }
}