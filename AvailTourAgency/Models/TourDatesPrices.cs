using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AvailTourAgency.Models
{
    public static class TourDatesPrices
    {
        public static List<TourDatesPrice> GetTourDatesPrices(bool showDeleted, int tourID, string sorting, string ascOrDesc)
        {
            List<TourDatesPrice> tdps = new List<TourDatesPrice>();
            using (AppContext db = new AppContext())
            {
                var query = (from t in db.TourDatesPrices
                             select t).AsEnumerable();

                if (!showDeleted)
                {
                    query = query.Where(t => !t.Deleted);
                }

                if (tourID != 0)
                {
                    query = query.Where(t => t.TourID == tourID);
                }
                
                //sorting
                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(t => t.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(t => t.ID); }
                    if (sorting == "flydatethere") { query = query.OrderBy(t => t.FlyDateThere); }
                    if (sorting == "flydateback") { query = query.OrderBy(t => t.FlyDateBack); }
                    if (sorting == "length") { query = query.OrderBy(t => t.Length); }
                    if (sorting == "price") { query = query.OrderBy(t => t.Price); }
                    if (sorting == "airline") { query = query.OrderBy(t => t.Airline); }
                    if (sorting == "plane") { query = query.OrderBy(t => t.Plane); }
                    if (sorting == "flightclass") { query = query.OrderBy(t => t.FlightClass); }
                    if (sorting == "luggage") { query = query.OrderBy(t => t.Luggage); }
                    if (sorting == "meals") { query = query.OrderBy(t => t.Meals); }
                    if (sorting == "quantity") { query = query.OrderBy(t => t.Quantity); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(t => t.ID); }
                    if (sorting == "flydatethere") { query = query.OrderByDescending(t => t.FlyDateThere); }
                    if (sorting == "flydateback") { query = query.OrderByDescending(t => t.FlyDateBack); }
                    if (sorting == "length") { query = query.OrderByDescending(t => t.Length); }
                    if (sorting == "price") { query = query.OrderByDescending(t => t.Price); }
                    if (sorting == "airline") { query = query.OrderByDescending(t => t.Airline); }
                    if (sorting == "plane") { query = query.OrderByDescending(t => t.Plane); }
                    if (sorting == "flightclass") { query = query.OrderByDescending(t => t.FlightClass); }
                    if (sorting == "luggage") { query = query.OrderByDescending(t => t.Luggage); }
                    if (sorting == "quantity") { query = query.OrderByDescending(t => t.Quantity); }
                }

                foreach (var tdp in query)
                {
                    tdps.Add(tdp);
                }
                return tdps;
            }
        }

        public static ObservableCollection<TourDatesPriceItem> GetTourDatesPricesForTable(bool showDeleted, int tourID, string sorting, string ascOrDesc)
        {
            ObservableCollection<TourDatesPriceItem> tableTourDatesPrices = new ObservableCollection<TourDatesPriceItem>();
            List<TourDatesPrice> tdps = GetTourDatesPrices(showDeleted, tourID, sorting, ascOrDesc);
            Dictionary<int, string> FlightClasses = new Dictionary<int, string>();
            string[] fc = ConfigurationManager.AppSettings["FlightClass"].Split(';');
            foreach (string c in fc)
            {
                FlightClasses.Add(int.Parse(c.Split(':')[0]),c.Split(':')[1]);
            }
            foreach (TourDatesPrice tdp in tdps)
            {
                tableTourDatesPrices.Add(new TourDatesPriceItem
                {
                    ID = tdp.ID,
                    FlyDateThere = tdp.FlyDateThere.Value.ToShortDateString(),
                    FlyDateBack = tdp.FlyDateBack.Value.ToShortDateString(),
                    Length = tdp.Length,
                    Price = tdp.Price,
                    Airline = tdp.Airline,
                    Plane = tdp.Plane,
                    FlightClass = FlightClasses[tdp.FlightClass],
                    Luggage = tdp.Luggage,
                    Meals = tdp.Meals,
                    Quantity = tdp.Quantity,
                    FlightClasses = FlightClasses
                });
            }
            return tableTourDatesPrices;
        }

        public static TourDatesPrice GetTDPByID(int id)
        {
            using (AppContext db = new AppContext())
            {
                return db.TourDatesPrices.Where(t => t.ID == id).FirstOrDefault();
            }
        }
    }
    public class TourDatesPriceItem
    {
        public int ID { get; set; }
        public string FlyDateThere { get; set; }
        public string FlyDateBack { get; set; }
        public int Length { get; set; }
        public decimal Price { get; set; }
        public string Airline { get; set; }
        public string Plane { get; set; }
        public string FlightClass { get; set; }
        public string Luggage { get; set; }
        public string Meals { get; set; }
        public int Quantity { get; set; }
        public Dictionary<int,string> FlightClasses { get; set; }
    }
}
