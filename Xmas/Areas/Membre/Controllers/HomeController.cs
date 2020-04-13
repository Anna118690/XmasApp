using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xmas.Tools;
using Xmas.Tools.Filters;

namespace Xmas.Areas.Membre.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        // GET: Membre/Home
        public ActionResult Index()
        {
           
             
                ViewBag.Nom = SessionUtils.ConnectedUser.Nom;
                ViewBag.Current = "Profil";
                return View(SessionUtils.ConnectedUser);
             
             
            
        }
    }
}