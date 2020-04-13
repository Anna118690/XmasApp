using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Xmas.Areas.Membre.Models;
using Xmas.DAL.Models;
using Xmas.DAL.Repositories;
using Xmas.Models;

namespace Xmas.Tools.Mappers
{

    /// <summary>
    /// Use to define function for mapping models
    /// </summary>
    public static class MapToDBModel
    {
        public static Membre LoginToMembre(LoginModel lm)
        {
            return new Membre()
            {
                Courriel = lm.Email,
                MotDePasse = lm.Password
            };
        }

        public static MembreModel MembreToMembreModel(Membre membre)
        {
            if (membre == null) return null;
            return new MembreModel()
            {
                Nom = membre.Nom
            };
        }

        public static CadeauxModel CadeauxToCadeauxModel(Cadeau c)
        {
            return new CadeauxModel()
            {
                Description = c.Description,
                IdCadeau = c.Id,
                IdMembre = c.IdMembre,
                Magasin = c.Magasin,
                Nom = c.Nom,
                Prix = c.Prix

            };
        }

        public static GroupModel GroupToGroupModel(Groupe g)
        {
            EvenementRepository ev = new EvenementRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
            return new GroupModel()
                {
                      IdGroupe= g.IdGroupe,
                       Description =g.Description,
                        Nom =g.Nom,
                         NomEvenement = ev.GetOne(g.IdEvenement).Nom
            };
        }

        public static Membre RegisterToMembre(RegisterModel rm)
        {
            return new Membre()
            { Nom=rm.Nom,
             Prenom=rm.Prenom,
              Surnom=rm.Surnom,
                Courriel = rm.Email,
                MotDePasse = rm.MotDePasse
            };
        }

        internal static ProfileModel MemberToProfile(Membre mmodel)
        {if (mmodel == null) return null;
            return new ProfileModel()
            {
                Id= mmodel.Id,  
                Nom = mmodel.Nom,
                Prenom = mmodel.Prenom,
                Surnom = mmodel.Surnom,
                Email = mmodel.Courriel
            };
        }

        public static Groupe SaveGroupModelToGroup(SaveGroupModel gm)
        {
            return new Groupe()
            {
                IdGroupe = gm.IdGroupe,
                Nom=gm.Nom,
                Description = gm.Description,
                IdEvenement = gm.IdEvenement
            };
        }

        public static EditGroupModel GroupToEditGroupModel(Groupe g)
        {
            EditGroupModel Em = new EditGroupModel();

            Em.MonGroupe= new GroupModel()
            {
                IdGroupe = g.IdGroupe,
                Description = g.Description,
                Nom = g.Nom
            };
            EvenementRepository er = new EvenementRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
             
             List<Evenement> ev =    er.GetAll().ToList();
            Em.MesEvents = new List<EventModel>();
            //Mapping
            foreach (Evenement item in ev)
            {
                Em.MesEvents.Add(MapToDBModel.EvenementToEventModel(item));
            }
            return Em;
        }

        public static EventModel EvenementToEventModel(Evenement item)
        {
            return new EventModel()
            {
                 IdEvenement = item.IdEvenement,
                  Nom = item.Nom
            };
        }

        public static DetailsGroupModel GroupToDetails(Groupe groupe)
        {
            return new DetailsGroupModel()
            {
                LeGroupe = MapToDBModel.GroupToEditGroupModel(groupe),
                LEvent = MapToDBModel.EvenementToEventModel(groupe.Evenement),
                Membres = groupe.MembreGroupe.Select(e => MapToDBModel.MemberToProfile(e)).ToList()

            };
        }
    }
}