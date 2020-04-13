using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xmas.Areas.Membre.Models
{
    public class DetailsGroupModel
    {
        private EditGroupModel _leGroupe;
        private EventModel _lEvent;
        private List<ProfileModel> _Membres;

        public EditGroupModel LeGroupe
        {
            get
            {
                return _leGroupe;
            }

            set
            {
                _leGroupe = value;
            }
        }

        public EventModel LEvent
        {
            get
            {
                return _lEvent;
            }

            set
            {
                _lEvent = value;
            }
        }

        public List<ProfileModel> Membres
        {
            get
            {
                return _Membres;
            }

            set
            {
                _Membres = value;
            }
        }
    }
}