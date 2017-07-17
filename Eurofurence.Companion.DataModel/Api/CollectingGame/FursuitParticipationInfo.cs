using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eurofurence.Companion.DataModel.Api.CollectingGame
{
    public class FursuitParticipationInfo
    {
        public FursuitBadgeRecord Badge { get; set; }
        public bool IsParticipating { get; set; }
        public FursuitParticipationRecord Participation { get; set; }
    }
}
