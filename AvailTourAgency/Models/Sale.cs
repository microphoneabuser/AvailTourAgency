using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AvailTourAgency.Models
{
    public class Sale
    {
        public int ID { get; set; }
        public DateTime SaleDate { get; set; }
        
        public DateTime GetSaleDate()//преобразовывает время из UTC в локальное (наконец-тооооо!!!!!!!!!)
        {
            TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;

            var dateTime = DateTime.SpecifyKind(SaleDate, DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, systemTimeZone);
        }
        public DateTime EditDate { get; set; }
        public int ClientID { get; set; }
        public int TourID { get; set; }
        public int TourDatesPriceID { get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
        public int EmployeeID { get; set; }
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

        public List<AddService> GetAddServices()
        {
            List<AddService> currentAddServices = new List<AddService>();
            using (AppContext db = new AppContext())
            {
                var query = from s in db.Sales.Where(s => s.ID == this.ID)
                            join sas in db.AddServicesInSales on s.ID equals sas.SaleID
                            into addServicesSalesTemp
                            from addServicesSales in addServicesSalesTemp.DefaultIfEmpty()
                            join a in db.AddServices on addServicesSales.AddServiceID equals a.ID
                            into addServicesTemp
                            from addServices in addServicesTemp.DefaultIfEmpty()
                            select addServices;
                foreach (var a in query)
                {
                    currentAddServices.Add(a);
                }
                return currentAddServices;
            }
        }

        public List<HotelRoom> GetHotelRooms()
        {
            List<HotelRoom> currentHotelRooms = new List<HotelRoom>();
            using (AppContext db = new AppContext())
            {
                var query = from s in db.Sales.Where(s => s.ID == this.ID)
                            join shr in db.HotelRoomsInSales on s.ID equals shr.SaleID
                            into hotelRoomsSalesTemp
                            from hotelRoomsSales in hotelRoomsSalesTemp.DefaultIfEmpty()
                            join hr in db.HotelRooms on hotelRoomsSales.HotelRoomID equals hr.ID
                            into hotelRoomsTemp
                            from hotelRooms in hotelRoomsTemp.DefaultIfEmpty()
                            select hotelRooms;
                foreach (var hr in query)
                {
                    currentHotelRooms.Add(hr);
                }
                return currentHotelRooms;
            }
        }
        public Hotel GetHotel()
        {
            Hotel hotel = new Hotel { Name = "" };
            using (AppContext db = new AppContext())
            {
                List<Hotel> hotels = db.Hotels.ToList();
                List<HotelRoom> hotelRooms = GetHotelRooms();
                if (hotelRooms.Count != 0 && hotelRooms[0] != null)
                {
                    hotel = hotels.Where(h => h.ID == hotelRooms[0].HotelID).FirstOrDefault();
                    if (hotel == null)
                    {
                        return new Hotel { Name = "" };
                    }
                    else
                    {
                        return hotel;
                    }
                }
                else
                {
                    return new Hotel { Name = ""};
                }
            }
            
        }
        public City GetCity()
        {
            City city = new City { Name = "" };
            using (AppContext db = new AppContext())
            {
                List<City> cities = db.Cities.ToList();
                Hotel hotel = GetHotel();

                if (hotel != null)
                {
                    city = cities.Where(c => c.ID == hotel.CityID).FirstOrDefault();
                    if (city == null)
                    {
                        return new City { Name = "" };
                    }
                    else
                    {
                        return city;
                    }
                }
                else
                {
                    return new City { Name = "" };
                }
            }
        }
        public Client GetClient()
        {
            Client client = new Client { FIO = "" };
            using (AppContext db = new AppContext())
            {
                client = db.Clients.Where(c => c.ID == this.ClientID).FirstOrDefault();
                if (client == null)
                {
                    return new Client { FIO = "" };
                }
                else
                {
                    return client;
                }
            }
        }
        public TourDatesPrice GetTourDatesPrice()
        {
            TourDatesPrice tourDatesPrice = new TourDatesPrice { FlyDateThere = null };
            using (AppContext db = new AppContext())
            {
                tourDatesPrice = db.TourDatesPrices.Where(tdp => tdp.ID == this.TourDatesPriceID).FirstOrDefault();
                if (tourDatesPrice == null)
                {
                    return new TourDatesPrice { FlyDateThere = null };
                }
                else
                {
                    return tourDatesPrice;
                }
            }
        }
        public Tour GetTour()
        {
            Tour tour = new Tour { Name = "" };
            using (AppContext db = new AppContext())
            {
                if (GetTourDatesPrice() != null)
                {
                    var tourID = GetTourDatesPrice().TourID;
                    tour = db.Tours.Where(t => t.ID == tourID).FirstOrDefault();
                    if (tour == null)
                    {
                        return new Tour { Name = "" };
                    }
                    else
                    {
                        return tour;
                    }
                }
                else
                {
                    return new Tour { Name = ""};
                }
            }
        }
        public Employee GetEmployee()
        {
            using (AppContext db = new AppContext())
            {
                return db.Employees.Where(c => c.ID == this.EmployeeID).FirstOrDefault();
            }
        }
        public bool CheckSaleDate(DateTime? date, bool gt)
        {
            if (gt)
            {
                if (this.GetSaleDate().Date >= date.Value.Date) { return true; }
                else return false;
            }
            else
            {
                if (this.GetSaleDate().Date <= date.Value.Date) { return true; }
                else return false;
            }
        }
        public bool CheckSaleDate(DateTime? fdate, DateTime? sdate)
        {
            if (this.GetSaleDate().Date >= fdate.Value.Date && this.GetSaleDate().Date <= sdate.Value.Date) { return true; }
            else return false;
        }

        public bool CheckFlyDate(DateTime? date, bool gt)
        {
            using (AppContext db = new AppContext())
            {
                var tdp = GetTourDatesPrice();
                if (tdp != null)
                {
                    if (gt)
                    {
                        if (tdp.FlyDateThere.Value.Date >= date.Value.Date) { return true; }
                        else return false;
                    }
                    else
                    {
                        if (tdp.FlyDateThere.Value.Date <= date.Value.Date) { return true; }
                        else return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public bool CheckFlyDate(DateTime? fdate, DateTime? sdate)
        {
            using (AppContext db = new AppContext())
            {
                var tdp = GetTourDatesPrice();
                if (tdp != null)
                {

                    if (tdp.FlyDateThere.Value.Date >= fdate.Value.Date && tdp.FlyDateThere.Value.Date <= sdate.Value.Date) { return true; }
                    else return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CheckPrice(decimal price, bool gt)
        {
            if (gt)
            {
                if (Price >= price) { return true; }
                else return false;
            }
            else
            {
                if (Price <= price) { return true; }
                else return false;
            }
        }
        public bool CheckPrice(decimal fprice, decimal sprice)
        {
            if (Price >= fprice && Price <= sprice) { return true; }
            else return false;
        }



        public Result AddSale()
        {
            Result result = Check(false);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.Sales.Add(this);
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result DelSale()
        {
            Result result = new Result();
            //try
            //{
                using (AppContext db = new AppContext())
                {
                    this.Deleted = true;
                    db.Entry(this).State = EntityState.Modified;
                    db.SaveChanges();
                    result.Success = true;
                    return result;
                }
            //}
            //catch
            //{
            //    result.Success = false;
            //    result.Description = "delete_failed;";
            //    return result;
            //}
        }
        public Result EditSale()
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
            if (!edit)
            {
                using (AppContext db = new AppContext())
                {
                    Sale sale = db.Sales.Where(c => c.ClientID == this.ClientID &&
                                                    c.SaleDate == this.SaleDate &&
                                                    c.TourID == this.TourID &&
                                                    c.TourDatesPriceID == this.TourDatesPriceID &&
                                                    !c.Deleted).FirstOrDefault();
                    if (sale != null) { result.Success = false; result.Description += "duplication;"; }
                }
            }
            return result;
        }




        public void EditAddServicesInSale(ObservableCollection<AddServiceItem> newAddServiceItems, ObservableCollection<AddServiceItem> oldAddServiceItems)
        {
            foreach (AddServiceItem addServiceItem in oldAddServiceItems)
            {
                DelAddService(addServiceItem.ID);
            }
            foreach (AddServiceItem newAddServiceItem in newAddServiceItems)
            {
                AddAddService(newAddServiceItem.ID, (int)newAddServiceItem.Quantity);
            }
        }
        public Result AddAddService(int addServiceID, int quantity)
        {
            //try
            //{
                AddServicesInSales addServicesInSales = new AddServicesInSales();
                addServicesInSales.AddServiceID = addServiceID;
                addServicesInSales.SaleID = this.ID;
                addServicesInSales.Quantity = quantity;

                using (AppContext db = new AppContext())
                {
                    db.AddServicesInSales.Add(addServicesInSales);
                    db.SaveChanges();
                    return new Result { Success = true };
                }
            //}
            //catch
            //{
            //    return new Result { Success = false, Description = "add_failed" };
            //}
        }

        public Result DelAddService(int addServiceID)
        {
            using (AppContext db = new AppContext())
            {
                var asis = db.AddServicesInSales.Where(a => a.AddServiceID == addServiceID && a.SaleID == this.ID).FirstOrDefault();
                if (asis == null) { return new Result { Success = false, Description = "addservice_notfound" }; }
                else
                {
                    db.AddServicesInSales.Remove(asis);
                    db.SaveChanges();
                    return new Result { Success = true };
                }
            }
        }



        public void EditHotelRoomsInSale(ObservableCollection<HotelRoomItem> newHotelRoomsItems, ObservableCollection<HotelRoomItem> oldHotelRoomsItems)
        {
            foreach (HotelRoomItem hotelRoomItem in oldHotelRoomsItems)
            {
                DelHotelRoom(hotelRoomItem.ID);
            }
            foreach (HotelRoomItem newHotelRoomItem in newHotelRoomsItems)
            {
                AddHotelRoom(newHotelRoomItem.ID);
            }
        }
        public Result AddHotelRoom(int hotelRoomID)
        {
            //try
            //{
            HotelRoomsInSales hotelRoomsInSales = new HotelRoomsInSales();
            hotelRoomsInSales.HotelRoomID = hotelRoomID;
            hotelRoomsInSales.SaleID = this.ID;

            using (AppContext db = new AppContext())
            {
                db.HotelRoomsInSales.Add(hotelRoomsInSales);
                db.HotelRooms.Where(hr => hr.ID == hotelRoomID).FirstOrDefault().Quantity--;
                db.SaveChanges();
                return new Result { Success = true };
            }
            //}
            //catch
            //{
            //    return new Result { Success = false, Description = "add_failed" };
            //}
        }

        public Result DelHotelRoom(int hotelRoomID)
        {
            using (AppContext db = new AppContext())
            {
                var hris = db.HotelRoomsInSales.Where(a => a.HotelRoomID == hotelRoomID && a.SaleID == this.ID).FirstOrDefault();
                if (hris == null) { return new Result { Success = false, Description = "hotelroom_notfound" }; }
                else
                {
                    db.HotelRoomsInSales.Remove(hris);
                    db.HotelRooms.Where(hr => hr.ID == hotelRoomID).FirstOrDefault().Quantity++;
                    db.SaveChanges();
                    return new Result { Success = true };
                }
            }
        }






        public void EditTouristsInSale(ObservableCollection<TouristItem> newTouristsItems, ObservableCollection<TouristItem> oldTouristsItems)
        {
            foreach (TouristItem touristItem in oldTouristsItems)
            {
                DelTourist(touristItem.ID);
            }
            foreach (TouristItem newTouristItem in newTouristsItems)
            {
                AddTourist(newTouristItem.ID);
            }
        }
        public Result AddTourist(int touristID)
        {
            //try
            //{
                TouristsInSales touristsInSales = new TouristsInSales();
                touristsInSales.TouristID = touristID;
                touristsInSales.SaleID = this.ID;

                using (AppContext db = new AppContext())
                {
                    db.TouristsInSales.Add(touristsInSales);
                    db.SaveChanges();
                    return new Result { Success = true };
                }
            //}
            //catch
            //{
            //    return new Result { Success = false, Description = "add_failed" };
            //}
        }

        public Result DelTourist(int touristID)
        {
            using (AppContext db = new AppContext())
            {
                var tis = db.TouristsInSales.Where(a => a.TouristID == touristID && a.SaleID == this.ID).FirstOrDefault();
                if (tis == null) { return new Result { Success = false, Description = "tourist_notfound" }; }
                else
                {
                    db.TouristsInSales.Remove(tis);
                    db.SaveChanges();
                    return new Result { Success = true };
                }
            }
        }
    }
}
