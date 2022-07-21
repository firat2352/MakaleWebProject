using Makale.BusinessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Makale.WebProject.Controllers
{
    public class YorumController : Controller
    {
        private NoteManager _noteManager = new NoteManager();
        private CommentManager commentManager = new CommentManager();
        public ActionResult ShowNoteComments(int? id)
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

            return PartialView("_PartialComments", note.Comments.ToList());
            
        }
    }
}