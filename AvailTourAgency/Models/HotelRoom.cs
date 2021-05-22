using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class HotelRoom
    {
        public int ID { get; set; }
        //public Hotel Hotel { get; set; }
        public int HotelID { get; set; }
        public int FirstTypeOfRoom { get; set; }
        public int SecondTypeOfRoom { get; set; }
        public int Quantity { get; set; }
        public decimal Price24 { get; set; }
        public string Description { get; set; }
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


        public Hotel GetHotel()
        {
            using (AppContext db = new AppContext())
            {
                return db.Hotels.Where(h => h.ID == this.HotelID).FirstOrDefault();
            }
        }

        public Result AddHotelRoom()
        {
            Result result = Check(false);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.HotelRooms.Add(this);
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result DelHotelRoom()
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
        public Result EditHotelRoom()
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
            if (this.HotelID == 0) { result.Success = false; result.Description = "empty_hotel;"; }
            if (this.FirstTypeOfRoom == 0) { result.Success = false; result.Description += "empty_firsttypeofroom;"; }
            if (this.SecondTypeOfRoom == 0) { result.Success = false; result.Description += "empty_secondtypeofroom;"; }
            if (this.Quantity == 0) { result.Success = false; result.Description += "empty_quantity;"; }
            if (this.Price24 == 0) { result.Success = false; result.Description += "empty_price24;"; }

            if (!edit)
            {
                using (AppContext db = new AppContext())
                {
                    HotelRoom HotelRoom = db.HotelRooms.Where(c => c.HotelID == this.HotelID &&
                                                    c.FirstTypeOfRoom == this.FirstTypeOfRoom &&
                                                    c.SecondTypeOfRoom == this.SecondTypeOfRoom &&
                                                    !c.Deleted).FirstOrDefault();
                    if (HotelRoom != null) { result.Success = false; result.Description += "duplication;"; }
                }
            }
            return result;
        }
    }
    //public enum FirstTypeOfRoom
    //{
    //    SGL,
    //    DBL,
    //    TWN,
    //    TRPL,
    //    QDPL,
    //    ADL
    //}
    //public enum SecondTypeOfRoom
    //{
    //    Standard,
    //    Superior,
    //    Suite,
    //    King_Suite,
    //    Family_room,
    //    Deluxe,
    //    Apartments,
    //    Studio,
    //    Duplex
    //}
}
