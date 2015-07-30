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
    public class ReceipeOutsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: ReceipeOuts/CreateUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUpdate(int? id, string name)
        {
            SessionTool.SetReceipeIdSession(id);
            SessionTool.SetReceipeNameSession(name);

            ViewBag.ActionItems = new SelectList(await db.ActionItems.ToListAsync(), "Id", "Name");

            ReceipeOutSessionModel sessionModel = SessionTool.GetReceipeOutSessionModel();

            if (sessionModel != null)
            {
                return View(new ReceipeOutCreateUpdateModel() { ActionItemId = sessionModel.ActionItemId, Value = sessionModel.Value });
            }

            return View();
        }

        // POST: ReceipeOuts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetInSession(ReceipeOutCreateUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                SessionTool.SetReceipeOutSessionModel(new ReceipeOutSessionModel() { ActionItemId = model.ActionItemId, Value = model.Value, Description = model.Value });

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
