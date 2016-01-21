using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ClientPortal.Models;

namespace ClientPortal.Controllers
{
    public class FundingRequestsController : ApiController
    {
        private ClientPortalContext db = new ClientPortalContext();

        // GET: api/FundingRequests
        public IQueryable<FundingRequest> GetFundingRequests()
        {
            return db.FundingRequests;
        }

        // GET: api/FundingRequests/5
        [ResponseType(typeof(FundingRequest))]
        public async Task<IHttpActionResult> GetFundingRequest(int id)
        {
            FundingRequest fundingRequest = await db.FundingRequests.FindAsync(id);
            if (fundingRequest == null)
            {
                return NotFound();
            }

            return Ok(fundingRequest);
        }

        // PUT: api/FundingRequests/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFundingRequest(int id, FundingRequest fundingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fundingRequest.Id)
            {
                return BadRequest();
            }

            db.Entry(fundingRequest).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FundingRequestExists(id))
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

        // POST: api/FundingRequests
        [ResponseType(typeof(FundingRequest))]
        public async Task<IHttpActionResult> PostFundingRequest(FundingRequest fundingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FundingRequests.Add(fundingRequest);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = fundingRequest.Id }, fundingRequest);
        }

        // DELETE: api/FundingRequests/5
        [ResponseType(typeof(FundingRequest))]
        public async Task<IHttpActionResult> DeleteFundingRequest(int id)
        {
            FundingRequest fundingRequest = await db.FundingRequests.FindAsync(id);
            if (fundingRequest == null)
            {
                return NotFound();
            }

            db.FundingRequests.Remove(fundingRequest);
            await db.SaveChangesAsync();

            return Ok(fundingRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FundingRequestExists(int id)
        {
            return db.FundingRequests.Count(e => e.Id == id) > 0;
        }
    }
}