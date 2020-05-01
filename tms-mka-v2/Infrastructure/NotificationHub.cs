﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Security;
using tms_mka_v2.Security;

namespace tms_mka_v2.Infrastructure.Monitoring
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        public void SendNotifications(string message, string id)
        {
            Clients.All.receiveNotification(message, id);
        }

        public void SendNotificationSound(string id)
        {
            Clients.All.receiveNotificationSound(id);
        }
        
        public void SendNotificationEmail(string EmailReceive, string SubjectEmail, string StringBody)
        {
            EmailHelper.SendEmail(EmailReceive, SubjectEmail, StringBody);
        }
    }
}