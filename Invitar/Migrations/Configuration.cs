namespace Invitar.Migrations
{
    using Invitar.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Invitar.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Invitar.Models.ApplicationDbContext context)
        {
            String userid = null;
            context.Database.ExecuteSqlCommand("Delete from Invitees");
            context.Database.ExecuteSqlCommand("Delete from Events");
            context.Events.AddOrUpdate(
                new Event { Title="Birthday", Location="1501 St Paul Street", StartDate= DateTime.Now.AddDays(-10), StartTime = DateTime.Now.ToString("hh:mm tt"), Description ="First Birthday", HideGuest=false, InviteOtherGuest= false , Image= new byte[]{}, IsSample =true, UserId = userid  },
                new Event { Title = "Baby Shower", Location = "123 Ram Street", StartDate = DateTime.Now.AddDays(-50), StartTime = DateTime.Now.ToString("hh:mm tt"), Description = "Baby Shower", HideGuest = false, InviteOtherGuest = false, Image = new byte[] { }, IsSample = true, UserId = userid },
                new Event { Title = "Graduation", Location = "77 Washington Street", StartDate = DateTime.Now.AddDays(-80), StartTime = DateTime.Now.ToString("hh:mm tt"), Description = "Graduation from Towson", HideGuest = false, InviteOtherGuest = false, Image = new byte[] { }, IsSample = true, UserId = userid },
                new Event { Title = "Farewell", Location = "098 Steele Rd", StartDate = DateTime.Now.AddDays(-50), StartTime = DateTime.Now.ToString("hh:mm tt"), Description = "Farwell from Office", HideGuest = false, InviteOtherGuest = false, Image = new byte[] { }, IsSample = true, UserId = userid }
                );  
        }
    }
}
