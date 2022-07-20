using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Makale.BusinessLayer;
using Makale.BusinessLayer.Result;
using Makale.Entities;


namespace Makale.WebProject.Controllers
{
    public class KullaniciController : Controller
    {
        private NoteUserManager _noteUserManager = new NoteUserManager();

        public ActionResult Index()
        {
            return View(_noteUserManager.List());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _noteUserManager.Find(x=>x.Id==id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<User> res = _noteUserManager.Insert(user);

                if(res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(user);
                }

                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _noteUserManager.Find(x => x.Id == id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
         
                //TODO
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _noteUserManager.Find(x => x.Id == id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = _noteUserManager.Find(x => x.Id == id);
            _noteUserManager.Delete(user);

            return RedirectToAction("Index");
        }

    }
}
