using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventoryPOS.api.StaticModels
{
    public class ClaimsStore
    {
        public List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("AdminRole","AdminRole"),
            new Claim("WorkerRole", "WokerRole")
        };
    }
}
