using Makale.BusinessLayer;
using Makale.Entities;
using Makale.Entities.Messages;
using Makale.Entities.ValueObjects;
using Makale.WebProject.ViewModels;
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

                OkViewModel okViewModel = new OkViewModel()
                {
                    Title="Kayıt Başarılı",
                    RedirectingUrl="/Home/Login",
                };


                okViewModel.Items.Add("Lutfen e-posta adresinize gelen linke tıklayarak hesabınızı aktive ediniz");

                return View("Ok",okViewModel);
            }

            return View(model);

        }

      

        public ActionResult UserActivate(Guid activate_id)
        {
            NoteUserManager um = new NoteUserManager();
            BusinessLayerResult<User> res=um.ActivateUser(activate_id);

            if(res.Errors.Count>0)
            {
                ErrorViewModel errorViewModel = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors,
                };

                return View("Error", errorViewModel);
            }

            OkViewModel okViewModel = new OkViewModel()
            {
               Title="Hesap Aktifleştirildi",
               RedirectingUrl="Home/Login"
            };

            okViewModel.Items.Add(" Hesabınız Aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz.");
            return View("Ok",okViewModel);  
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
                ErrorViewModel errorViewModel = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors,
                };

                return View("Error", errorViewModel);
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

        public ActionResult TestNotify()
        {
            ErrorViewModel model = new ErrorViewModel()
            {
                Header = "Yönlendirme",
                Title = "oK Test",
                RedirectingTimeout = 3000,
                Items = new List<ErrorMessageObj>() {
                    new ErrorMessageObj() { Message = "Test Başarılı 1" }, 
                    new ErrorMessageObj() { Message = "Test Başarılı 2" } 
                }
            };

            return View("Error",model);
        }


    }
}