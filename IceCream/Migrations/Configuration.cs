﻿namespace IceCream.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq; 
     
    internal sealed class Configuration : DbMigrationsConfiguration<IceCream.DAL.DataEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
         
        protected override void Seed(IceCream.DAL.DataEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
