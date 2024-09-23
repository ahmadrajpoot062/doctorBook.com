
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    public class AppointmentHub : Hub
    {
        //// Method to update slot status for all connected clients
        //public async Task UpdateSlot(string doctorEmail, string date, string slot, bool isBooked)
        //{
        //    // Notify all clients except the sender about the slot update
        //    await Clients.All.SendAsync("ReceiveSlotUpdate", doctorEmail, date, slot, isBooked);
        //}
    }
}
