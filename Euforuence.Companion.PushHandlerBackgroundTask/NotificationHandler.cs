using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euforuence.Companion.PushHandlerBackgroundTask
{
    public class NotificationHandler
    {
        private readonly bool _isApplicationRunning;

        public NotificationHandler(bool isApplicationRunning)
        {
            _isApplicationRunning = isApplicationRunning;
        }

        public async Task HandleRawAsync(string message)
        {

        }
    }
}
