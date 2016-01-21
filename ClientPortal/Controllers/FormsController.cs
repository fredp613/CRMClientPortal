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
using System.Web.Http.Cors;
using ClientPortal.CRM;
    
namespace ClientPortal.Controllers
{
    [Authorize]
    public class FormsController : ApiController
    {
        private ClientPortalContext db = new ClientPortalContext();
        private CrmConnector _crmContext = new CrmConnector();

        // GET: api/Forms
        public IQueryable<Form> GetForms()
        {
            return db.Forms;
        }
        
        [Route("api/forms/getFormFields")]
        [EnableCors(origins: "http://localhost:60752", headers: "*", methods: "*")]
        public IEnumerable<FormField> GetFormFieldsCRM()
        {
            //var formFields = new List<FormField>();

            var formFieldsCRM = _crmContext.getFormFields("account");
            return formFieldsCRM;
        }

        // GET: api/Forms/5
        [ResponseType(typeof(Form))]
        public async Task<IHttpActionResult> GetForm(Guid id)
        {
            Form form = await db.Forms.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }

            return Ok(form);
        }

        // PUT: api/Forms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutForm(Guid id, Form form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != form.CrmFormId)
            {
                return BadRequest();
            }

            db.Entry(form).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormExists(id))
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

        // POST: api/Forms
        [ResponseType(typeof(Form))]
        public async Task<IHttpActionResult> PostForm(Form form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            form.CrmFormId = Guid.NewGuid();
            form.CreatedOn = DateTime.Now;
            form.UpdatedOn = DateTime.Now;
            db.Forms.Add(form);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FormExists(form.CrmFormId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = form.CrmFormId }, form);
        }

        // DELETE: api/Forms/5
        [ResponseType(typeof(Form))]
        public async Task<IHttpActionResult> DeleteForm(Guid id)
        {
            Form form = await db.Forms.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }

            db.Forms.Remove(form);
            await db.SaveChangesAsync();

            return Ok(form);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FormExists(Guid id)
        {
            return db.Forms.Count(e => e.CrmFormId == id) > 0;
        }
    }
}