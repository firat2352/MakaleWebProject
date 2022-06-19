using Makale.BusinessLayer;
using Makale.Entities;
using Makale.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Makale.WebProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            NoteManager noteManager = new NoteManager();
            return View(noteManager.GetAllNotes().OrderByDescending(x=>x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category category = cm.GetCategoryByID(id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View("Index", category.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager note = new NoteManager();

            return View("Index", note.GetAllNotes().OrderByDescending(n => n.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                NoteUserManager note = new NoteUserManager();
                BusinessLayerResult<User> result=note.RegisterUser(model);

                if(result.Errors.Count>0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }

                return RedirectToAction("RegisterOk");
            }

            return View(model);

        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid activate_id)
        {
            return View();
        }
    }
}