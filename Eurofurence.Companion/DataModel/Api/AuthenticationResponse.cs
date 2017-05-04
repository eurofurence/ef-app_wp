using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eurofurence.Companion.DataModel.Api
{

    public class AuthenticationResponse
    {
        public bool IsSuccessful { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime TokenValidUntil { get; set; }
    }
}
