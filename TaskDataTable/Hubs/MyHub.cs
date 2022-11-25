using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskDataTable.Models;

namespace TaskDataTable.Hubs
{
    public class MyHub : Hub
    {
  
        public static void trysignalR( )
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.getupdated();
        }
    }
}