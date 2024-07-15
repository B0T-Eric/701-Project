using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User_Auentication
    {
        public int UserID {get;set;}
        public int RolesID {get;set;}
        public string UserName {get;set;} = default!;
        public string Password {get;set;} = default!;
    }
}