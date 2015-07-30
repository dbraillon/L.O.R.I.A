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
    public class ReceipesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Receipes
        public async Task<ActionResult> Index()
        {
            List<Receipe> entities = await db.Receipes.ToListAsync();

            return View(entities);
        }

        public async Task<ActionResult> PrepareCreateUpdate(int? id)
        {
            SessionTool.ClearSession();

            if (id.HasValue)
            {
                Receipe receipe = await db.Receipes.FirstOrDefaultAsync(x => x.Id == id);

                if (receipe != null)
                {
                    ReceipeIn receipeIn = receipe.ReceipeIns.FirstOrDefault();
                    ReceipeOut receipeOut = receipe.ReceipeOuts.FirstOrDefault();
                    
                    if (receipeIn != null)
                    {
                        SessionTool.SetReceipeInSessionModel(new ReceipeInSessionModel() { Description = receipeIn.ToString(), TriggerItemId = receipeIn.TriggerItemId, Value = receipeIn.Value });
                    }

                    if (receipeOut != null)
                    {
                        SessionTool.SetReceipeOutSessionModel(new ReceipeOutSessionModel() { Description = receipeOut.ToString(), ActionItemId = receipeOut.ActionItemId, Value = receipeOut.Value });
                    }

                    SessionTool.SetReceipeIdSession(receipe.Id);
                    SessionTool.SetReceipeNameSession(receipe.Name);
                }
            }

            return RedirectToAction("CreateUpdate", new { id = id });
        }

        // GET: Receipes/CreateUpdate
        public ActionResult CreateUpdate(int? id)
        {
            ReceipeCreateUpdateModel model = new ReceipeCreateUpdateModel()
            {
                Id = SessionTool.GetReceipeIdSession(),
                Name = SessionTool.GetReceipeNameSession()
            };

            ReceipeInSessionModel receipeInSessionModel = SessionTool.GetReceipeInSessionModel();
            ReceipeOutSessionModel receipeOutSessionModel = SessionTool.GetReceipeOutSessionModel();

            ViewBag.ReceipeInDescription = receipeInSessionModel != null ? receipeInSessionModel.Description : "Click to choose a trigger";
            ViewBag.ReceipeOutDescription = receipeOutSessionModel != null ? receipeOutSessionModel.Description : "Click to choose an action";
            
            return View(model);
        }

        // POST: Receipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUpdate(ReceipeCreateUpdateModel model)
        {
            ReceipeInSessionModel receipeInSessionModel = SessionTool.GetReceipeInSessionModel();
            ReceipeOutSessionModel receipeOutSessionModel = SessionTool.GetReceipeOutSessionModel();

            if (receipeInSessionModel == null)
            {
                ModelState.AddModelError("", "You must select at least one trigger");
            }

            if (receipeOutSessionModel == null)
            {
                ModelState.AddModelError("", "You must select at least one action");
            }

            if (ModelState.IsValid)
            {
                if (model.Id == null)
                {
                    Receipe receipe = new Receipe()
                    {
                        Name = model.Name,
                        ReceipeIns = new ReceipeIn[]
                        {
                            new ReceipeIn()
                            {
                                TriggerItemId = receipeInSessionModel.TriggerItemId,
                                Value = receipeInSessionModel.Value
                            }
                        },
                        ReceipeOuts = new ReceipeOut[]
                        {
                            new ReceipeOut()
                            {
                                ActionItemId = receipeOutSessionModel.ActionItemId,
                                Value = receipeOutSessionModel.Value
                            }
                        }
                    };

                    db.Receipes.Add(receipe);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                else
                {
                    Receipe receipe = await db.Receipes.FirstOrDefaultAsync(x => x.Id == model.Id);
                    ReceipeIn receipeIn = receipe.ReceipeIns.FirstOrDefault();
                    ReceipeOut receipeOut = receipe.ReceipeOuts.FirstOrDefault();

                    if (receipe != null)
                    {
                        receipe.Name = model.Name;

                        if (receipeIn != null)
                        {
                            receipeIn.TriggerItemId = receipeInSessionModel.TriggerItemId;
                            receipeIn.Value = receipeInSessionModel.Value;
                        }
                        else
                        {
                            receipeIn = new ReceipeIn() { Receipe = receipe, TriggerItemId = receipeInSessionModel.TriggerItemId, Value = receipeInSessionModel.Value };
                        }
                        
                        if (receipeOut != null)
                        {
                            receipeOut.ActionItemId = receipeOutSessionModel.ActionItemId;
                            receipeOut.Value = receipeOutSessionModel.Value;
                        }
                        else
                        {
                            receipeOut = new ReceipeOut() { Receipe = receipe, ActionItemId = receipeOutSessionModel.ActionItemId, Value = receipeOutSessionModel.Value };
                        }

                        db.Entry(receipe).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                }
            }


            ViewBag.ReceipeInDescription = receipeInSessionModel != null ? receipeInSessionModel.Description : "Click to choose a trigger";
            ViewBag.ReceipeOutDescription = receipeOutSessionModel != null ? receipeOutSessionModel.Description : "Click to choose an action";

            return View(model);
        }

        // GET: Receipes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            Receipe receipe = await db.Receipes.FindAsync(id);

            if (receipe != null)
            {
                db.Receipes.Remove(receipe);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
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
