using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Loria.Dal;
using Loria.Dal.Entities;
using Loria.Configuration.Tools;
using Loria.Configuration.Models;

namespace Loria.Configuration.Controllers
{
    public class ReceipeInsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: ReceipeIns/CreateUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUpdate(int? id, string name)
        {
            SessionTool.SetReceipeIdSession(id);
            SessionTool.SetReceipeNameSession(name);

            ViewBag.TriggerItems = new SelectList(await db.TriggerItems.ToListAsync(), "Id", "Name");
            
            ReceipeInSessionModel sessionModel = SessionTool.GetReceipeInSessionModel();

            if (sessionModel != null)
            {
                return View(new ReceipeInCreateUpdateModel() { TriggerItemId = sessionModel.TriggerItemId, Value = sessionModel.Value });
            }

            return View();
        }

        // POST: ReceipeIns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetInSession(ReceipeInCreateUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                SessionTool.SetReceipeInSessionModel(new ReceipeInSessionModel() { TriggerItemId = model.TriggerItemId, Value = model.Value, Description = model.Value });

                return RedirectToAction("CreateUpdate", "Receipes");
            }


            ViewBag.TriggerItems = new SelectList(await db.TriggerItems.ToListAsync(), "Id", "Name");

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
