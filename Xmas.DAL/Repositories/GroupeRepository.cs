using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xmas.DAL.Models;

namespace Xmas.DAL.Repositories
{
    public class GroupeRepository : BaseRepository<Groupe, int>
    {
        public GroupeRepository(string Cnstr) : base(Cnstr)
        { 
            SelectAllCommand = "SELECT * FROM Groupe";
            SelectOneCommand = "SELECT * FROM Groupe WHERE IdGroupe=@IdGroupe ";
            InsertCommand = "INSERT INTO Groupe(Nom,Description,IdEvenement) OUTPUT inserted.IdGroupe VALUES (@Nom,@Description,@IdEvenement)";
            UpdateCommand = "UPDATE Groupe SET Nom=@Nom,Description=@Description,IdEvenement=@IdEvenement WHERE IdGroupe=@IdGroupe";
            DeleteCommand = "DELETE FROM  Groupe WHERE IdGroupe=@IdGroupe";
        }

        public override IEnumerable<Groupe> GetAll()
        {
            return base.getAll(Map);
        }

      

        public override Groupe GetOne(int id)
        {
            Dictionary<string, object> QueryParameters = new Dictionary<string, object>();
            QueryParameters.Add("IdGroupe", id);
            return base.getOne(Map, QueryParameters);
        }

        public Groupe GetOneWithInfos(int id)
        {
            Dictionary<string, object> QueryParameters = new Dictionary<string, object>();
            QueryParameters.Add("IdGroupe", id);
            Groupe G = base.getOne(Map, QueryParameters);
            G.Evenement = LoadEvent(G.IdEvenement);
            G.MembreGroupe = LoadMembres(id);
            return G;
        }

        private IEnumerable<Membre> LoadMembres(int id)
        {
            MembreRepository mr = new MembreRepository(this._Cnstr);
            return mr.GetAllFromGroup(id);
        }

        private Evenement LoadEvent(int id)
        {
            EvenementRepository ev = new EvenementRepository(this._Cnstr);
            return ev.GetOne(id);
        }

        public override Groupe Insert(Groupe toInsert)
        {
            Dictionary<string, object> Parameters = MapToDico(toInsert);
            int id = base.Insert(Parameters);
            toInsert.IdGroupe = id;
            return toInsert;
        }


        public Groupe InsertWithAdmin(Groupe toInsert, int idAdmin)
        {
            InsertCommand = "Exec InsertGroupWithAdmin  @Nom, @Description, @IdEvenement, @IdAdmin";
            Dictionary<string, object> Parameters = MapToDico(toInsert);
            Parameters.Add("IdAdmin", idAdmin);
            int id = base.Insert(Parameters);
            toInsert.IdGroupe = id;
            return toInsert;
        }

        public override bool Update(Groupe toUpdate)
        {
            Dictionary<string, object> Parameters = MapToDico(toUpdate);
            return base.Update(Parameters);

        }

        public override bool Delete(int id)
        {
            Dictionary<string, object> QueryParameters = new Dictionary<string, object>();
            QueryParameters.Add("IdGroupe", id);
            return base.Delete( QueryParameters);
        }


        public IEnumerable<Groupe> GetAllFromMembre(int idMembre, bool isAdmin = false)
        {
             
            SelectAllCommand = @"SELECT        Groupe.*
FROM            Membre INNER JOIN
                         MembreGroupe ON Membre.IdMembre = MembreGroupe.IdMembre INNER JOIN
                         Groupe ON MembreGroupe.IdGroupe = Groupe.IdGroupe INNER JOIN
                         Evenement ON Groupe.IdEvenement = Evenement.IdEvenement
WHERE        (Membre.IdMembre = @IdMembre) ";
            if(isAdmin)
            {
                SelectAllCommand += " AND(MembreGroupe.Admin = 1)";
            }
            Dictionary<string, object> QueryParameters = new Dictionary<string, object>();
            QueryParameters.Add("IdMembre", idMembre); 

            return base.getAll(Map,QueryParameters);
        }


        #region Mappers
        private Dictionary<string, object> MapToDico(Groupe toInsert)
        {
            Dictionary<string, object> p = new Dictionary<string, object>();
            p["Nom"] =  toInsert.Nom;
            p["IdGroupe"] = toInsert.IdGroupe;

            p["Description"] = toInsert.Description;

            p["IdEvenement"] = toInsert.IdEvenement;
            return p;
        }

        private Groupe Map(SqlDataReader p)
        {
            return new Groupe()
            {
                Nom = p["Nom"].ToString(),
                IdEvenement =(int) p["IdEvenement"],
                IdGroupe = (int)p["IdGroupe"],
                Description = p["Description"].ToString()
                
            };
        }
        #endregion
    }
}
