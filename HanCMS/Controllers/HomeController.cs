﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HanSoft.HelperLib;

namespace HanCMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Logger.InstanceInfo.Write(new Exception("a"),"admin");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}