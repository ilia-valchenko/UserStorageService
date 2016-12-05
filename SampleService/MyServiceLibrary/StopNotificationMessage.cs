using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServiceLibrary
{
    /// <summary>
    /// This message says that service should stop his work. 
    /// </summary>
    [Serializable]
    public class StopNotificationMessage : NotificationMessage
    {
        public StopNotificationMessage() : base(Commands.Stop) {}
    }
}
