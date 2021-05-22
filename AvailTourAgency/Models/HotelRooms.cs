using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AvailTourAgency.Models
{
    public static class HotelRooms
    {
        public static List<HotelRoom> GetHotelRooms(bool showDeleted, int hotelID, string sorting, string ascOrDesc)
        {
            List<HotelRoom> hotelrooms = new List<HotelRoom>();
            using (AppContext db = new AppContext())
            {
                var query = (from hr in db.HotelRooms
                             select hr).AsEnumerable();

                if (!showDeleted)
                {
                    query = query.Where(hr => !hr.Deleted);
                }

                if (hotelID != 0)
                {
                    query = query.Where(hr => hr.HotelID == hotelID);
                }

                //sorting
                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(hr => hr.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(hr =>hr.ID); }
                    if (sorting == "firsttype") { query = query.OrderBy(hr => hr.FirstTypeOfRoom); }
                    if (sorting == "secondtype") { query = query.OrderBy(hr => hr.SecondTypeOfRoom); }
                    if (sorting == "quantity") { query = query.OrderBy(hr => hr.Quantity); }
                    if (sorting == "price24") { query = query.OrderBy(hr => hr.Price24); }
                    if (sorting == "description") { query = query.OrderBy(hr => hr.Description); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(hr => hr.ID); }
                    if (sorting == "firsttype") { query = query.OrderByDescending(hr => hr.FirstTypeOfRoom); }
                    if (sorting == "secondtype") { query = query.OrderByDescending(hr => hr.SecondTypeOfRoom); }
                    if (sorting == "quantity") { query = query.OrderByDescending(hr => hr.Quantity); }
                    if (sorting == "price24") { query = query.OrderByDescending(hr => hr.Price24); }
                    if (sorting == "description") { query = query.OrderByDescending(hr => hr.Description); }
                }

                foreach (var hotelroom in query)
                {
                    hotelrooms.Add(hotelroom);
                }
                return hotelrooms;
            }
        }

        public static ObservableCollection<HotelRoomItem> GetHotelRoomsForTable(bool showDeleted, int hotelID, string sorting, string ascOrDesc)
        {
            ObservableCollection<HotelRoomItem> tableHotelRooms = new ObservableCollection<HotelRoomItem>();
            List<HotelRoom> hotelrooms = GetHotelRooms(showDeleted, hotelID, sorting, ascOrDesc);
            foreach (HotelRoom hotelroom in hotelrooms)
            {
                string description = "";
                if (hotelroom.Description != null && hotelroom.Description.Length < 31)
                {
                    description = hotelroom.Description;
                }
                else if (hotelroom.Description != null)
                {
                    description = hotelroom.Description.Substring(0, 30) + "...";
                }

                Dictionary<int, string> FirstTypesOfRoom = new Dictionary<int, string>();
                string[] ft = ConfigurationManager.AppSettings["FirstTypes"].Split(';');
                foreach (string t in ft)
                {
                    FirstTypesOfRoom.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
                }

                Dictionary<int, string> SecondTypesOfRoom = new Dictionary<int, string>();
                string[] st = ConfigurationManager.AppSettings["SecondTypes"].Split(';');
                foreach (string t in st)
                {
                    SecondTypesOfRoom.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
                }

                tableHotelRooms.Add(new HotelRoomItem
                {
                    ID = hotelroom.ID,
                    FirstTypeOfRoom = FirstTypesOfRoom[hotelroom.FirstTypeOfRoom],
                    SecondTypeOfRoom = SecondTypesOfRoom[hotelroom.SecondTypeOfRoom],
                    Quantity = hotelroom.Quantity,
                    Price24 = hotelroom.Price24,
                    Description = description,
                    Hotel = hotelroom.GetHotel().Name,
                    FirstTypesOfRoom = FirstTypesOfRoom,
                    SecondTypesOfRoom = SecondTypesOfRoom
                });
            }
            return tableHotelRooms;
        }

        public static List<HotelRoom> GetHotelRoomsInSale(bool showDeleted, int saleID, string sorting, string ascOrDesc)
        {
            List<HotelRoom> hotelRooms = new List<HotelRoom>();
            using (AppContext db = new AppContext())
            {
                var query = (from hr in db.HotelRooms
                             join hris in db.HotelRoomsInSales on hr.ID equals hris.HotelRoomID
                             into hrHrisTemp
                             from hrHris in hrHrisTemp.DefaultIfEmpty()
                             join s in db.Sales on hrHris.SaleID equals s.ID
                             into sTemp
                             from s in sTemp.DefaultIfEmpty()
                             select new
                             {
                                 ID = hr.ID,
                                 FirstTypeOfRoom = hr.FirstTypeOfRoom,
                                 SecondTypeOfRoom = hr.SecondTypeOfRoom,
                                 Quantity = hr.Quantity,
                                 Price24 = hr.Price24,
                                 Description = hr.Description,
                                 Deleted = hr.Deleted,
                                 HotelID = hr.HotelID,
                                 SaleID = (s == null ? 0 : s.ID)
                             }).AsEnumerable();

                if (!showDeleted)
                {
                    query = query.Where(hr => !hr.Deleted);
                }

                if (saleID != 0)
                {
                    query = query.Where(hr => hr.SaleID == saleID);
                }

                //sorting
                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(hr => hr.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(s => s.ID); }
                    if (sorting == "firsttypeofroom") { query = query.OrderBy(s => s.FirstTypeOfRoom); }
                    if (sorting == "secondtypeofroom") { query = query.OrderBy(s => s.SecondTypeOfRoom); }
                    if (sorting == "quantity") { query = query.OrderBy(s => s.Quantity); }
                    if (sorting == "price24") { query = query.OrderBy(s => s.Price24); }
                    if (sorting == "description") { query = query.OrderBy(s => s.Description); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(s => s.ID); }
                    if (sorting == "firsttypeofroom") { query = query.OrderByDescending(s => s.FirstTypeOfRoom); }
                    if (sorting == "secondtypeofroom") { query = query.OrderByDescending(s => s.SecondTypeOfRoom); }
                    if (sorting == "quantity") { query = query.OrderByDescending(s => s.Quantity); }
                    if (sorting == "price24") { query = query.OrderByDescending(s => s.Price24); }
                    if (sorting == "description") { query = query.OrderByDescending(s => s.Description); }
                }

                foreach (var hotelRoom in query)
                {
                    hotelRooms.Add(new HotelRoom
                    {
                        ID = hotelRoom.ID,
                        FirstTypeOfRoom = hotelRoom.FirstTypeOfRoom,
                        SecondTypeOfRoom = hotelRoom.SecondTypeOfRoom,
                        Quantity = hotelRoom.Quantity,
                        Price24 = hotelRoom.Price24,
                        Description = hotelRoom.Description,
                        HotelID = hotelRoom.HotelID,
                        Deleted = hotelRoom.Deleted
                    });
                }
                return hotelRooms;
            }
        }
        public static ObservableCollection<HotelRoomItem> GetHotelRoomsInSaleForTable(bool showDeleted, int saleID, string sorting, string ascOrDesc)
        {
            ObservableCollection<HotelRoomItem> tableHotelRooms = new ObservableCollection<HotelRoomItem>();
            List<HotelRoom> hotelrooms = GetHotelRoomsInSale(showDeleted, saleID, sorting, ascOrDesc);
            foreach (HotelRoom hotelroom in hotelrooms)
            {
                string description = "";
                if (hotelroom.Description != null && hotelroom.Description.Length < 31)
                {
                    description = hotelroom.Description;
                }
                else if (hotelroom.Description != null)
                {
                    description = hotelroom.Description.Substring(0, 30) + "...";
                }

                Dictionary<int, string> FirstTypesOfRoom = new Dictionary<int, string>();
                string[] ft = ConfigurationManager.AppSettings["FirstTypes"].Split(';');
                foreach (string t in ft)
                {
                    FirstTypesOfRoom.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
                }

                Dictionary<int, string> SecondTypesOfRoom = new Dictionary<int, string>();
                string[] st = ConfigurationManager.AppSettings["SecondTypes"].Split(';');
                foreach (string t in st)
                {
                    SecondTypesOfRoom.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
                }

                tableHotelRooms.Add(new HotelRoomItem
                {
                    ID = hotelroom.ID,
                    FirstTypeOfRoom = FirstTypesOfRoom[hotelroom.FirstTypeOfRoom],
                    SecondTypeOfRoom = SecondTypesOfRoom[hotelroom.SecondTypeOfRoom],
                    Quantity = hotelroom.Quantity,
                    Price24 = hotelroom.Price24,
                    Description = description,
                    Hotel = hotelroom.GetHotel().Name,
                    FirstTypesOfRoom = FirstTypesOfRoom,
                    SecondTypesOfRoom = SecondTypesOfRoom
                });
            }
            return tableHotelRooms;
        }







        public static List<HotelRoom> GetHotelRoomsForSale(bool showDeleted, int hotelID, string sorting, string ascOrDesc)
        {
            List<HotelRoom> hotelrooms = new List<HotelRoom>();
            using (AppContext db = new AppContext())
            {
                var query = (from hr in db.HotelRooms
                             select hr).AsEnumerable();

                if (!showDeleted)
                {
                    query = query.Where(hr => !hr.Deleted);
                }

                if (hotelID != 0)
                {
                    query = query.Where(hr => hr.HotelID == hotelID);
                }

                query = query.Where(hr => hr.Quantity > 0);

                //sorting
                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(hr => hr.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(hr => hr.ID); }
                    if (sorting == "firsttype") { query = query.OrderBy(hr => hr.FirstTypeOfRoom); }
                    if (sorting == "secondtype") { query = query.OrderBy(hr => hr.SecondTypeOfRoom); }
                    if (sorting == "quantity") { query = query.OrderBy(hr => hr.Quantity); }
                    if (sorting == "price24") { query = query.OrderBy(hr => hr.Price24); }
                    if (sorting == "description") { query = query.OrderBy(hr => hr.Description); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(hr => hr.ID); }
                    if (sorting == "firsttype") { query = query.OrderByDescending(hr => hr.FirstTypeOfRoom); }
                    if (sorting == "secondtype") { query = query.OrderByDescending(hr => hr.SecondTypeOfRoom); }
                    if (sorting == "quantity") { query = query.OrderByDescending(hr => hr.Quantity); }
                    if (sorting == "price24") { query = query.OrderByDescending(hr => hr.Price24); }
                    if (sorting == "description") { query = query.OrderByDescending(hr => hr.Description); }
                }

                foreach (var hotelroom in query)
                {
                    hotelrooms.Add(hotelroom);
                }
                return hotelrooms;
            }
        }

        public static ObservableCollection<HotelRoomItem> GetHotelRoomsForTableForSale(bool showDeleted, int hotelID, string sorting, string ascOrDesc)
        {
            ObservableCollection<HotelRoomItem> tableHotelRooms = new ObservableCollection<HotelRoomItem>();
            List<HotelRoom> hotelrooms = GetHotelRoomsForSale(showDeleted, hotelID, sorting, ascOrDesc);
            foreach (HotelRoom hotelroom in hotelrooms)
            {
                string description = "";
                if (hotelroom.Description != null && hotelroom.Description.Length < 31)
                {
                    description = hotelroom.Description;
                }
                else if (hotelroom.Description != null)
                {
                    description = hotelroom.Description.Substring(0, 30) + "...";
                }

                Dictionary<int, string> FirstTypesOfRoom = new Dictionary<int, string>();
                string[] ft = ConfigurationManager.AppSettings["FirstTypes"].Split(';');
                foreach (string t in ft)
                {
                    FirstTypesOfRoom.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
                }

                Dictionary<int, string> SecondTypesOfRoom = new Dictionary<int, string>();
                string[] st = ConfigurationManager.AppSettings["SecondTypes"].Split(';');
                foreach (string t in st)
                {
                    SecondTypesOfRoom.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
                }

                tableHotelRooms.Add(new HotelRoomItem
                {
                    ID = hotelroom.ID,
                    FirstTypeOfRoom = FirstTypesOfRoom[hotelroom.FirstTypeOfRoom],
                    SecondTypeOfRoom = SecondTypesOfRoom[hotelroom.SecondTypeOfRoom],
                    Quantity = hotelroom.Quantity,
                    Price24 = hotelroom.Price24,
                    Description = description,
                    Hotel = hotelroom.GetHotel().Name,
                    FirstTypesOfRoom = FirstTypesOfRoom,
                    SecondTypesOfRoom = SecondTypesOfRoom
                });
            }
            return tableHotelRooms;
        }





        public static HotelRoom GetHotelRoomByID(int id)
        {
            using (AppContext db = new AppContext())
            {
                return db.HotelRooms.Where(hr => hr.ID == id).FirstOrDefault();
            }
        }
    }

    public class HotelRoomItem
    {
        public int ID { get; set; }
        public string FirstTypeOfRoom { get; set; }
        public string SecondTypeOfRoom { get; set; }
        public int Quantity { get; set; }
        public decimal Price24 { get; set; }
        public string Description { get; set; }
        public string Hotel { get; set; }
        public Dictionary<int, string> FirstTypesOfRoom { get; set; }
        public Dictionary<int, string> SecondTypesOfRoom { get; set; }

    }
}
