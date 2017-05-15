using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DojoConnections.Models
{

    public class Invitation : Base{
        public int InvitationId{get;set; }
        public int NetworkId{get; set;}
        public Network Network{get; set; } 
        public int UserId{get; set; }
        public User User{get; set; }
        
    }
}