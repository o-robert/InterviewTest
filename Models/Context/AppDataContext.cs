using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InterviewTest001.Models.Context
{
    public class AppDataContext : DbContext
    {
        public AppDataContext() : base("AppDataConnection")
        {
        }

        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<UserDocument> UserDocuments { get; set; }
    }
}