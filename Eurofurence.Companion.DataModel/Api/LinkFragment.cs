using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eurofurence.Companion.DataModel.Api
{
    public class LinkFragment
    {
        public enum FragmentTypeEnum
        {
            WebExternal,
            MapExternal,
            MapInternal
        }

        public FragmentTypeEnum FragmentType { get; set; }

        public string Name { get; set; }

        public string Target { get; set; }
    }
}
