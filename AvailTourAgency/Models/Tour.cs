using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class Tour
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public City City { get; set; }
        public int CityID { get; set; }
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

        public Tour(){}
        public Tour(string name, int cityID, string description)
        {
            this.Name = name;
            this.CityID = cityID;
            this.Description = description;
            this.Deleted = false;
        }

        public List<TourDatesPrice> GetTourDatesPrices()
        {
            List<TourDatesPrice> tourDatesPrices = new List<TourDatesPrice>();
            using (AppContext db = new AppContext())
            {
                tourDatesPrices = db.TourDatesPrices.Where(tdp => tdp.TourID == this.ID).ToList();
                return tourDatesPrices;
            }
        }

        public List<Sale> GetSales()
        {
            List<Sale> sales = new List<Sale>();
            using (AppContext db = new AppContext())
            {
                return db.Sales.Where(s => s.GetTour() == this).ToList();
                //return sales = db.Sales.Where(s => s.Tour == this).ToList();
            }
        }
        public City GetCity()
        {
            using (AppContext db = new AppContext())
            {
                return db.Cities.Where(c => c.ID == this.CityID).FirstOrDefault();
            }
        }
        public bool CheckDate(DateTime? date, bool gt)
        {
            //bool result = false;
            foreach (var tdp in GetTourDatesPrices())
            {
                if (gt)
                {
                    if (tdp.FlyDateThere.Value.Date >= date.Value.Date) { return true; }
                }
                else
                {
                    if (tdp.FlyDateThere.Value.Date <= date.Value.Date) { return true; }
                }
            }
            return false;
        }
        public bool CheckDate(DateTime? fdate, DateTime? sdate)
        {
            //bool result = false;
            foreach (var tdp in GetTourDatesPrices())
            {
                if (tdp.FlyDateThere.Value.Date >= fdate.Value.Date && tdp.FlyDateThere.Value.Date <= sdate.Value.Date) { return true; }
            }
            return false;
        }

        public bool CheckPrice(decimal price, bool gt)
        {
            foreach (var tdp in GetTourDatesPrices())
            {
                if (gt)
                {
                    if (tdp.Price >= price) { return true; }
                }
                else
                {
                    if (tdp.Price <= price) { return true; }
                }
            }
            return false;
        }
        public bool CheckPrice(decimal fprice, decimal sprice)
        {
            foreach (var tdp in GetTourDatesPrices())
            {
                if (tdp.Price >= fprice && tdp.Price <= sprice) { return true; }
            }
            return false;
        }
        public Result AddTour()
        {
            Result result = Check(false);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.Tours.Add(this);
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result DelTour()
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
        public Result EditTour()
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

            if (!edit)
            {
                using (AppContext db = new AppContext())
                {
                    Tour Tour = db.Tours.Where(c => c.Name == this.Name &&
                                                    c.CityID == this.CityID && !c.Deleted).FirstOrDefault();
                    if (Tour != null) { result.Success = false; result.Description += "duplication;"; }
                }
            }
            return result;
        }
    }
}
