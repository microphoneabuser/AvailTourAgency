using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class Hotel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //public City City { get; set; }
        public int CityID { get; set; }
        public string Features { get; set; }
        public string Entertainment { get; set; }
        public string Meals { get; set; }
        public string Infrastructure { get; set; }
        public string Description { get; set; }
        public int? DateOfConstruction { get; set; }
        public int? LastRenovation { get; set; }
        public int? NumberOfBuildings { get; set; }
        public int? NumberOfRooms { get; set; }
        public int? Rating { get; set; }
        public bool Deleted { get; set; }

        public int deleted
        {
            get
            {
                if (Deleted)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value == 1)
                {
                    Deleted = true;
                }
                else
                {
                    Deleted = false;
                }
            }
        }
        public Hotel() { }
        public Hotel(string name, int cityID, string features, string entertainment, string meals, string infrastructure, string description, 
            int? dateOfConstruction, int? lastRenovation, int? numberOfBuildings, int? numberOfRooms, int? rating)
        {
            this.Name = name;
            this.CityID = cityID;
            this.Features = features;
            this.Entertainment = entertainment;
            this.Meals = meals;
            this.Infrastructure = infrastructure;
            this.Description = description;
            this.DateOfConstruction = dateOfConstruction;
            this.LastRenovation = lastRenovation;
            this.NumberOfBuildings = numberOfBuildings;
            this.NumberOfRooms = numberOfRooms;
            this.Rating = rating;
            this.Deleted = false;
        }
        public City GetCity()
        {
            using (AppContext db = new AppContext())
            {
                return db.Cities.Where(c => c.ID == this.CityID).FirstOrDefault();
            }
        }
        public Result AddHotel()
        {
            Result result = Check(false);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.Hotels.Add(this);
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result DelHotel()
        {
            Result result = new Result();
            try
            {
                using (AppContext db = new AppContext())
                {
                    this.Deleted = true;
                    db.Entry(this).State = EntityState.Modified;
                    db.SaveChanges();
                    result.Success = true;
                    return result;
                }
            }
            catch
            {
                result.Success = false;
                result.Description = "delete_failed;";
                return result;
            }
        }
        public Result EditHotel()
        {
            Result result = Check(true);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.Entry(this).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result Check(bool edit)
        {
            Result result = new Result();
            result.Success = true;
            if (this.Name == "") { result.Success = false; result.Description = "empty_name;"; }
            if (this.CityID == 0) { result.Success = false; result.Description += "empty_city;"; }
            //if (this.Features == "") { result.Success = false; result.Description += "empty_features;"; }
            //if (this.Entertainment == "") { result.Success = false; result.Description += "empty_entertainment;"; }
            //if (this.Meals == "") { result.Success = false; result.Description += "empty_meals;"; }
            //if (this.Infrastructure == "") { result.Success = false; result.Description += "empty_infrastructure;"; }
            //if (this.DateOfConstruction == 0) { result.Success = false; result.Description += "empty_dateofconstruction"; }
            //if (this.LastRenovation == 0) { result.Success = false; result.Description += "empty_lastrenovation"; }
            //if (this.NumberOfBuildings == 0) { result.Success = false; result.Description += "empty_numberofbuildings"; }
            //if (this.NumberOfRooms == 0) { result.Success = false; result.Description += "empty_numberofrooms"; }
            //if (this.Rating == 0) { result.Success = false; result.Description += "empty_rating"; }

            if (!edit)
            {
                using (AppContext db = new AppContext())
                {
                    Hotel Hotel = db.Hotels.Where(c => c.Name == this.Name &&
                                                    c.CityID == this.CityID && !c.Deleted).FirstOrDefault();
                    if (Hotel != null) { result.Success = false; result.Description += "duplication;"; }
                }
            }
            return result;
        }
    }
}
