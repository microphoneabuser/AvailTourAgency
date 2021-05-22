using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AvailTourAgency.Models;

namespace AvailTourAgency
{
    class AppContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Tourist> Tourists { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourDatesPrice> TourDatesPrices { get; set; }
        public DbSet<AddService> AddServices { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<Sale> Sales { get; set; }

        //
        public DbSet<AddServicesInSales> AddServicesInSales { get; set; }
        public DbSet<HotelRoomsInSales> HotelRoomsInSales { get; set; }
        public DbSet<TouristsInSales> TouristsInSales { get; set; }
        //
        public DbSet<Role> Roles { get; set; }
        public DbSet<Models.Action> Actions { get; set; }
        public DbSet<Right> Rights { get; set; }

        public AppContext() : base("DefaultConnection") { }
    }
}
