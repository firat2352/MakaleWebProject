using Makale.BusinessLayer;
using Makale.Entities;
using Makale.Entities.Messages;
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
            if (ModelState.IsValid)
            {
                 NoteUserManager noteUserManager = new NoteUserManager();
                 BusinessLayerResult<User> businessLayerResult=noteUserManager.LoginUser(loginViewModel);

                if (businessLayerResult.Errors.Count > 0)
                {
                businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(loginViewModel);
                }

                Session["login"] = businessLayerResult.Result;
                return RedirectToAction("Index");

            }
           

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
                    result.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
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
            NoteUserManager um = new NoteUserManager();
            BusinessLayerResult<User> res=um.ActivateUser(activate_id);

            if(res.Errors.Count>0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            }


            return RedirectToAction("UserActivateOk");  
        }

        public ActionResult UserActivateOk()
        {
            return View();
        }

        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
                
            if(TempData["errors"] !=null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;

            }
            return View(errors);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult ShowProfile()
        {
            User currentUser=Session["login"] as User;
            NoteUserManager noteUserManager = new NoteUserManager();
            BusinessLayerResult<User> res=noteUserManager.GetUserByID(currentUser.Id);

            if(res.Errors.Count>0)
            {

            }

            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditProfile(User user)
        {
            return View();
        }

        public ActionResult RemoveProfile(User user)
        {
            return View();
        }




    }
}