using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class TourDatesPrice
    {
        public int ID { get; set; }
        //public Tour Tour { get; set; }
        public int TourID{ get; set; }
        public Nullable<DateTime> FlyDateThere { get; set; }
        public Nullable<DateTime> FlyDateBack { get; set; }
        public int Length { get; set; }
        public decimal Price { get; set; }
        public string Airline { get; set; }
        public string Plane { get; set; }
        public int FlightClass { get; set; }
        public string Luggage { get; set; }
        public string Meals { get; set; }
        public int Quantity { get; set; }
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


        public Result AddTourDatesPrice()
        {
            Result result = Check(false);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.TourDatesPrices.Add(this);
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result DelTourDatesPrice()
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
        public Result EditTourDatesPrice()
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
            if (this.TourID == 0) { result.Success = false; result.Description += "empty_tour;"; }
            if (this.FlyDateThere == null) { result.Success = false; result.Description += "empty_flydatethere;"; }
            if (this.FlyDateBack == null) { result.Success = false; result.Description += "empty_flydateback;"; }
            if (this.Length == 0) { result.Success = false; result.Description += "empty_length;"; }
            if (this.Price == 0) { result.Success = false; result.Description += "empty_price;"; }
            if (this.Airline == "") { result.Success = false; result.Description += "empty_airline"; }
            if (this.Plane == "") { result.Success = false; result.Description += "empty_plane"; }
            if (this.FlightClass == 0) { result.Success = false; result.Description += "empty_flightclass"; }
            if (this.Luggage == "") { result.Success = false; result.Description += "empty_luggage"; }
            if (this.Meals == "") { result.Success = false; result.Description += "empty_meals"; }
            if (this.Quantity == 0) { result.Success = false; result.Description += "empty_quantity"; }
            if (this.FlyDateThere != null && this.FlyDateBack != null && this.FlyDateThere >= this.FlyDateBack) 
                { result.Success = false; result.Description += "wrong_dates"; }

            if (!edit)
            {
                using (AppContext db = new AppContext())
                {
                    TourDatesPrice TourDatesPrice = db.TourDatesPrices.Where(c => c.TourID == this.TourID &&
                                                    c.FlyDateThere == this.FlyDateThere &&
                                                    c.FlyDateBack == this.FlyDateBack &&
                                                    c.Length == this.Length &&
                                                    c.Price == this.Price &&
                                                    c.Airline == this.Airline &&
                                                    c.FlightClass == this.FlightClass &&
                                                    !c.Deleted).FirstOrDefault();
                    if (TourDatesPrice != null) { result.Success = false; result.Description += "duplication;"; }
                }
            }
            return result;
        }
    }
    //public enum FlightClass
    //{
    //    Эконом,
    //    Бизнес,
    //    Первый,
    //}
}
