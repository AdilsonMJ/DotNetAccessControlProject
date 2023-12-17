using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ControllerDeAcesso.Authorization
{
    public class IdadeMinima : IAuthorizationRequirement
    {

        public int Idade {get; set;}
        public IdadeMinima(int idade)
        {
            Idade = idade;
        }
    }
}