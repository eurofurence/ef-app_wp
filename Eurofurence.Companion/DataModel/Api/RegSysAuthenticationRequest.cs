using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eurofurence.Companion.DataModel.Api
{
    public class RegSysAuthenticationRequest
    {
        public int RegNo { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
