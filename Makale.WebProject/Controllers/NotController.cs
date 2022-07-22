using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Makale.BusinessLayer;
using Makale.Entities;
using Makale.WebProject.Models;

namespace Makale.WebProject.Controllers
{
    public class NotController : Controller
    {
        private NoteManager _noteManager = new NoteManager();
        private CategoryManager _categoryManager = new CategoryManager();
        private LikedManager  _likedManager= new LikedManager();

      
        public ActionResult Index()
        {
            
            var notes = _noteManager.ListQueryable().Include("Category").Include("Owner").Where(x=>x.Owner.Id==CurrentSession.User.Id).OrderByDescending(x=>x.ModifiedOn);

            return View(notes.ToList());
        }


        public ActionResult MyLikedNotes()
        {
            var notes = _likedManager.ListQueryable().Include("LikedUser").Include("Note").Where(
               x => x.LikedUser.Id == CurrentSession.User.Id).Select(
               x => x.Note).Include("Category").Include("Owner").OrderByDescending(
               x => x.ModifiedOn);

            return View("Index", notes.ToList());
        }
      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = _noteManager.Find(x=>x.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

       
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categoryManager.List(), "Id", "Title");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                _noteManager.Insert(note);

                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_categoryManager.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

      
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = _noteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_categoryManager.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                
                Note db_note = _noteManager.Find(x => x.Id == note.Id);
                db_note.IsDraft = note.IsDraft;
                db_note.CategoryId = note.CategoryId;
                db_note.Text = note.Text;
                db_note.Title = note.Title;

                _noteManager.Update(db_note);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_categoryManager.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

     
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = _noteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = _noteManager.Find(x => x.Id == id);
            _noteManager.Delete(note);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (CurrentSession.User != null)
            {
                int userid = CurrentSession.User.Id;
                List<int> likedNoteIds = new List<int>();

                if (ids != null)
                {
                    likedNoteIds = _likedManager.List(
                        x => x.LikedUser.Id == userid && ids.Contains(x.Note.Id)).Select(
                        x => x.Note.Id).ToList();
                }
                else
                {
                    likedNoteIds = _likedManager.List(
                        x => x.LikedUser.Id == userid).Select(
                        x => x.Note.Id).ToList();
                }

                return Json(new { result = likedNoteIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }

        public ActionResult SetLikeState(int noteid, bool liked)
        {
            int res = 0;

            if (CurrentSession.User == null)
                return Json(new { hasError = true, errorMessage = "Beğenme işlemi için giriş yapmalısınız.", result = 0 });

            Liked like = _likedManager.Find(x => x.Note.Id == noteid && x.LikedUser.Id == CurrentSession.User.Id);

            Note note = _noteManager.Find(x => x.Id == noteid);

            if (like != null && liked == false)
            {
                res = _likedManager.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = _likedManager.Insert(new Liked()
                {
                    LikedUser = CurrentSession.User,
                    Note = note
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    note.LikeCount++;
                }
                else
                {
                    note.LikeCount--;
                }

                res = _noteManager.Update(note);

                return Json(new { hasError = false, errorMessage = string.Empty, result = note.LikeCount });
            }

            return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi.", result = note.LikeCount });
        }
    }
}
