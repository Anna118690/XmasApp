using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xmas.Areas.Membre.Models;
using Xmas.DAL.Models;
using Xmas.DAL.Repositories;
using Xmas.Tools;
using Xmas.Tools.Filters;
using Xmas.Tools.Mappers;

namespace Xmas.Areas.Membre.Controllers
{
    [CustomAuthorize]
    public class GroupController : Controller
    {
        // GET: Membre/Group
        public ActionResult Index()
        {
            
            GroupeRepository GR = new GroupeRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
            int id = SessionUtils.ConnectedUser.Id;
            List<GroupModel> Lgm = GR.GetAllFromMembre(id).Select(g => MapToDBModel.GroupToGroupModel(g)).ToList();
            ViewBag.Current = "Groupe";
            return View(Lgm);
        }

        public ActionResult Admin()
        {
             
            GroupeRepository GR = new GroupeRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
            int id = SessionUtils.ConnectedUser.Id;
            List<GroupModel> Lgm = GR.GetAllFromMembre(id,true).Select(g => MapToDBModel.GroupToGroupModel(g)).ToList();
            ViewBag.Current = "Groupe Admin";
            return View(Lgm);
        }

        #region Edition
        public ActionResult Edit(int id)
        {
            GroupeRepository Gr = new GroupeRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
            Groupe G = Gr.GetOne(id); //Entity
            EditGroupModel Gm = MapToDBModel.GroupToEditGroupModel(G);
            ViewBag.Current = "Groupe";
            return View(Gm);
        }

        [HttpPost]
        public ActionResult Edit(SaveGroupModel Gm)
        {
            GroupeRepository Gr = new GroupeRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
            Groupe G = MapToDBModel.SaveGroupModelToGroup(Gm);
            if (Gr.Update(G))
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Current = "Groupe";
                return View(); //++ message d'erreur au besoin
            }
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            EvenementRepository er = new EvenementRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);

            List<Evenement> ev = er.GetAll().ToList();
            List<EventModel> MesEvents = new List<EventModel>();
            foreach (Evenement item in ev)
            {
                MesEvents.Add(MapToDBModel.EvenementToEventModel(item));
            }
            ViewBag.Current = "Groupe";
            return View(MesEvents);
        }

        [HttpPost]
        public ActionResult Create(SaveGroupModel SG)
        { 
            GroupeRepository Gr = new GroupeRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
            Groupe G = MapToDBModel.SaveGroupModelToGroup(SG);
            G = Gr.InsertWithAdmin(G, SessionUtils.ConnectedUser.Id);
            if (G.Id != 0)
            {
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Current = "Groupe";
                return View(); //++ message d'erreur au besoin
            }
        } 
        #endregion

        public ActionResult Details(int id)
        {
            GroupeRepository Gr = new GroupeRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
            DetailsGroupModel Gwm =MapToDBModel.GroupToDetails(Gr.GetOneWithInfos(id));
            ViewBag.Current = "Groupe";
            return View();
        }
    }
}