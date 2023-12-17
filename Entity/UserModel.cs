using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ControllerDeAcesso.Data
{
    public class UserModel : IdentityUser
    {
        public DateTime BirthDay { get; set; }

        public UserModel() : base(){ }

    }
}