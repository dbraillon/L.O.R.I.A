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
using Loria.Configuration.Models;
using Loria.Configuration.Models.Abilities;

namespace Loria.Configuration.Controllers
{
    public class AbilitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Abilities
        public async Task<ActionResult> Index()
        {
            return View(await db.Abilities.ToListAsync());
        }

        // GET: Abilities/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipe ability = await db.Abilities.FindAsync(id);
            if (ability == null)
            {
                return HttpNotFound();
            }
            return View(ability);
        }

        // GET: Abilities/Create
        public ActionResult Create()
        {
            string[] stringAbilities = Enum.GetNames(typeof(Sense));
            ViewBag.StringAbilities = new MultiSelectList(stringAbilities);

            return View();
        }

        // POST: Abilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Sense senses = default(Sense);
                foreach (string senseString in model.Senses)
                {
                    senses = senses | (Sense)Enum.Parse(typeof(Sense), senseString);
                }

                Receipe ability = new Receipe()
                {
                    Name = model.Name,
                    Type = model.Type,
                    Senses = senses
                };

                db.Abilities.Add(ability);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            string[] stringAbilities = Enum.GetNames(typeof(Sense));
            ViewBag.StringAbilities = new MultiSelectList(stringAbilities);

            return View(model);
        }

        // GET: Abilities/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipe ability = await db.Abilities.FindAsync(id);
            if (ability == null)
            {
                return HttpNotFound();
            }
            return View(ability);
        }

        // POST: Abilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Type,Senses")] Receipe ability)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ability).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ability);
        }

        // GET: Abilities/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipe ability = await db.Abilities.FindAsync(id);
            if (ability == null)
            {
                return HttpNotFound();
            }
            return View(ability);
        }

        // POST: Abilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Receipe ability = await db.Abilities.FindAsync(id);
            db.Abilities.Remove(ability);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
