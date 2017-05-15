using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DojoConnections.Models
{

    public class Network : Base{
        public int NetworkId{get; set;} 
        public int UserId{get; set; }
        public User User{get; set; }
        public bool AcceptedInvite{get; set; }

        public bool IgnoredInvite{get; set; }

        public List<User> Users {get; set; }

        public List<Invitation> Invitations{get; set; }

        public Network()
        {
            Users = new List<User>();
            Invitations = new List<Invitation>();
        }
    }
}