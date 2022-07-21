﻿using System;
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

     
    }
}