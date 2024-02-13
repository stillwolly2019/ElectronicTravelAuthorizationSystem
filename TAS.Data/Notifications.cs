using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAuthorizationSystem.Utility;
namespace Data
{
    public sealed class Notifications
    {
        public void SendEmail(string TO, string CC, string Subject, string Body, string Attachment)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Noti.SendEmail", TO, CC, Subject, Body, Attachment);
        }

        public void SendEmail(string TO, string Subject, string Body)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Noti.SendEmail", TO, Subject, Body);
        }


    }
}
