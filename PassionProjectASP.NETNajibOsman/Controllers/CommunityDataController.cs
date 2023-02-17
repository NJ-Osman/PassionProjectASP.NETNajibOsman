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
    public class CommunityDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Community in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Communities in the database, including their associated Community.
        /// </returns>
        /// <example>
        /// GET: api/CommunityData/ListCommunities
        /// </example>
        public IEnumerable<CommunityDto> ListCommunities()
        {
            List<Community> Communities = db.Communities.ToList();
            List<CommunityDto> CommunityDtos = new List<CommunityDto>();

            Communities.ForEach(c => CommunityDtos.Add(new CommunityDto()
            {
                CommunityID = c.CommunityID,
                CommunityName = c.CommunityName,
                CommunityBio = c.CommunityBio
            }));
            return CommunityDtos;
        }
        /// <summary>
        /// Returns all Communities in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Community in the system matching up to the Community ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Community</param>
        /// <example>
        /// GET: api/CommunityData/FindCommunity/5
        /// </example>
        [ResponseType(typeof(Community))]
        public IHttpActionResult GetCommunity(int id)
        {
            Community Community = db.Communities.Find(id);
            CommunityDto CommunityDto = new CommunityDto()
            {
                CommunityID = Community.CommunityID,
                CommunityName = Community.CommunityName,
                CommunityBio = Community.CommunityBio
            };
            if (Community == null)
            {
                return NotFound();
            }

            return Ok(CommunityDto);
        }
        /// <summary>
        /// Updates a particular Community in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Community ID primary key</param>
        /// <param name="Community">JSON FORM DATA of aa Community</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// Post: Community api/CommunityData/UpdateCommunity/5
        /// FORM DATA: Community JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult PutCommunity(int id, Community community)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != community.CommunityID)
            {
                return BadRequest();
            }

            db.Entry(community).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(id))
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
        /// Adds a Community to the system
        /// </summary>
        /// <param name="Community">JSON FORM DATA of a Community</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Community ID, Community Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CommunityData/AddCommunity
        /// FORM DATA: Community JSON Object
        /// </example>
        [ResponseType(typeof(Community))]
        [HttpPost]
        public IHttpActionResult PostCommunity(Community community)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Communities.Add(community);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = community.CommunityID }, community);
        }

        /// <summary>
        /// Deletes a community from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Community</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CommunityData/DeleteCommunity/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Community))]
        public IHttpActionResult DeleteCommunity(int id)
        {
            Community community = db.Communities.Find(id);
            if (community == null)
            {
                return NotFound();
            }

            db.Communities.Remove(community);
            db.SaveChanges();

            return Ok(community);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommunityExists(int id)
        {
            return db.Communities.Count(e => e.CommunityID == id) > 0;
        }
    }
}