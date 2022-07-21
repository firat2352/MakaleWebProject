using Makale.BusinessLayer;
using Makale.BusinessLayer.Result;
using Makale.Entities;
using Makale.Entities.Messages;
using Makale.Entities.ValueObjects;
using Makale.WebProject.Models;
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
       private NoteManager noteManager = new NoteManager();
       private CategoryManager cm = new CategoryManager();
       private NoteUserManager noteUserManager = new NoteUserManager();
        // GET: Home
        public ActionResult Index()
        {

            return View(noteManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           
            Category category = cm.Find(x=>x.Id==id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View("Index", category.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
          

            return View("Index", noteManager.ListQueryable().OrderByDescending(n => n.Likes).ToList());
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
              
                BusinessLayerResult<User> businessLayerResult = noteUserManager.LoginUser(loginViewModel);

                if (businessLayerResult.Errors.Count > 0)
                {
                    businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(loginViewModel);
                }

                CurrentSession.Set<User>("Login",businessLayerResult.Result);
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
              
                BusinessLayerResult<User> result = noteUserManager.RegisterUser(model);

                if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel okViewModel = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",
                };


                okViewModel.Items.Add("Lutfen e-posta adresinize gelen linke tıklayarak hesabınızı aktive ediniz");

                return View("Ok", okViewModel);
            }

            return View(model);

        }

        public ActionResult UserActivate(Guid activate_id)
        {
           
            BusinessLayerResult<User> res = noteUserManager.ActivateUser(activate_id);

            if (res.Errors.Count > 0)
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
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "Home/Login"
            };

            okViewModel.Items.Add(" Hesabınız Aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz.");
            return View("Ok", okViewModel);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult ShowProfile()
        {
           
          
            BusinessLayerResult<User> res = noteUserManager.GetUserByID(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
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
         
           
            BusinessLayerResult<User> res = noteUserManager.GetUserByID(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
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

        [HttpPost]
        public ActionResult EditProfile(User user, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername "); 
           if(ModelState.IsValid)
            {
                if (ProfileImage != null &&
                     (ProfileImage.ContentType == "image/jpeg" ||
                     ProfileImage.ContentType == "image/jpg" ||
                     ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{user.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    user.ProfileImageFileName = filename;
                }

              
                BusinessLayerResult<User> res = noteUserManager.UpdateProfile(user);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorViewModel = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorViewModel);
                }


                CurrentSession.Set<User>("Login",res.Result);

                return RedirectToAction("ShowProfile");
            }

            return View(user);
        }
        public ActionResult DeleteProfile(User user)
        {

            
            BusinessLayerResult<User> res = noteUserManager.RemoveUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
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

            return View("Error", model);
        }

    }
}
