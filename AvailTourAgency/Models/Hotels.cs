using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public static class Hotels
    {
        public static List<Hotel> GetHotels(bool showNotDeleted, bool showDeleted, int id, string name, string city,
            string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            List<Hotel> hotels = new List<Hotel>();
            using (AppContext db = new AppContext())
            {
                var query = (from h in db.Hotels
                            select h).AsEnumerable();

                if (showNotDeleted && !showDeleted)
                {
                    query = query.Where(h => !h.Deleted);
                }
                if (!showNotDeleted && showDeleted)
                {
                    query = query.Where(h => h.Deleted);
                }

                if (id != 0)
                {
                    query = query.Where(h => h.ID == id);
                }
                if (name != null & name != "")
                {
                    string validName = Validator.ValidateString(name);
                    query = query.Where(h => h.Name.ToLower().Contains(validName));
                }
                if (city != null & city != "")
                {
                    string validCity = Validator.ValidateString(city);
                    query = query.Where(h => h.GetCity().Name.ToLower() == validCity);
                }

                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(h => h.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(h => h.ID); }
                    if (sorting == "name") { query = query.OrderBy(h => h.Name); }
                    if (sorting == "city") { query = query.OrderBy(h => h.GetCity().Name); }
                    if (sorting == "numberofbuildings") { query = query.OrderBy(e => e.NumberOfBuildings); }
                    if (sorting == "numberofrooms") { query = query.OrderBy(e => e.NumberOfRooms); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(h => h.ID); }
                    if (sorting == "name") { query = query.OrderByDescending(h => h.Name); }
                    if (sorting == "city") { query = query.OrderByDescending(h => h.GetCity().Name); }
                    if (sorting == "numberofbuildings") { query = query.OrderByDescending(e => e.NumberOfBuildings); }
                    if (sorting == "numberofrooms") { query = query.OrderByDescending(e => e.NumberOfRooms); }
                }

                genCount = query.Count();

                query = query.Skip((page - 1) * count).Take(count);

                foreach (var hotel in query)
                {
                    hotels.Add(hotel);
                }
                return hotels;
            }
        }

        public static ObservableCollection<HotelItem> GetHotelsForTable(bool showNotDeleted, bool showDeleted, int id, string name, string city,
            string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            ObservableCollection<HotelItem> tableHotels = new ObservableCollection<HotelItem>();
            List<Hotel> hotels = GetHotels(showNotDeleted, showDeleted, id, name, city, sorting, ascOrDesc, count, page, ref genCount);
            foreach (Hotel hotel in hotels)
            {
                tableHotels.Add(new HotelItem
                {
                    ID = hotel.ID,
                    Name = hotel.Name,
                    City = hotel.GetCity().Name,
                    NumberOfBuildings = hotel.NumberOfBuildings,
                    NumberOfRooms = hotel.NumberOfRooms
                });
            }
            return tableHotels;
        }

        public static Hotel GetHotelByID(int id)
        {
            using (AppContext db = new AppContext())
            {
                return db.Hotels.Where(t => t.ID == id).FirstOrDefault();
            }
        }

        public static IEnumerable<int> GetIdsForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var hotel in db.Hotels)
                {
                    yield return hotel.ID;
                }
            }
        }
        public static IEnumerable<string> GetHotelsForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var hotel in db.Hotels)
                {
                    yield return hotel.Name;
                }
            }
        }
    }

    public class HotelItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int? NumberOfBuildings { get; set; }
        public int? NumberOfRooms { get; set; }
    }
}
