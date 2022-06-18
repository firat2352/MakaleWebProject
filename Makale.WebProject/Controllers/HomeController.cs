﻿using Makale.BusinessLayer;
using Makale.Entities;
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
    }
}