using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xmas.Areas.Membre.Models;
using Xmas.DAL.Repositories;
using Xmas.Tools;
using Xmas.Tools.Filters;
using Xmas.Tools.Mappers;

namespace Xmas.Areas.Membre.Controllers
{
    [CustomAuthorize]
    public class CadeauxController : Controller
    {
        // GET: Membre/Cadeaux
        public ActionResult Index()
        {
            ViewBag.Current = "Cadeaux";
            CadeauRepository CR = new CadeauRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
            List<CadeauxModel> Lc = CR.GetCadeauxFromMembre(SessionUtils.ConnectedUser.Id).Select(c => MapToDBModel.CadeauxToCadeauxModel(c)).ToList();
            return View(Lc);
        }

        
      
    }
}