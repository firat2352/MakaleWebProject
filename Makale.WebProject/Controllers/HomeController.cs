﻿using Makale.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makale.WebProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Test test = new Test();
            //test.Insert();
            //test.Update();
            //test.Delete();
            test.CommentTest();

            return View();
        }
    }
}