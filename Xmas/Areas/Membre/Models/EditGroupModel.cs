using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xmas.Areas.Membre.Models
{
    public class EditGroupModel
    {
        GroupModel _groupe;
        List<EventModel> _mesEvents;

        public GroupModel MonGroupe
        {
            get
            {
                return _groupe;
            }

            set
            {
                _groupe = value;
            }
        }

        public List<EventModel> MesEvents
        {
            get
            {
                return _mesEvents;
            }

            set
            {
                _mesEvents = value;
            }
        }
    }
}