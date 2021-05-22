using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class City
    {
        //attribs
        public int ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }

        public City() { }
        public City(string name, string country, string description) 
        {
            this.Name = name;
            this.Country = country;
            this.Description = description;
        }
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

        //methods

        public Result AddCity()
        {
            Result result = Check(false);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.Cities.Add(this);
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result DelCity()
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
        public Result EditCity()
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
            if (this.Country == "") { result.Success = false; result.Description = "empty_country;"; }
            if (!edit)
            {
                using (AppContext db = new AppContext())
                {
                    City city = db.Cities.Where(c => c.Name == this.Name && !c.Deleted).FirstOrDefault();
                    if (city != null) { result.Success = false; result.Description += "duplication;"; }
                }
            }
            return result;
        }
    }
}
