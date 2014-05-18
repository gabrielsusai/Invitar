using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Invitar.Models
{
    public class Invitee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public InviteResponse? Response { get; set; }
        public int? Count { get; set; }
        public string Comment { get; set; }
    }
    public enum InviteResponse
    {
        Yes = 1,
        No = 2,
        Maybe = 3
    }
}