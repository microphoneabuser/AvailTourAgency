using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AvailTourAgency.Models
{
    public static class Tours
    {
        public static List<Tour> GetTours(bool showNotDeleted, bool showDeleted, int id, string name, string city,
            DateTime? fDate, DateTime? sDate, string fPrice, string sPrice,
            string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            List<Tour> tours = new List<Tour>();
            using (AppContext db = new AppContext())
            {
                var query = (from t in db.Tours
                            select t).AsEnumerable();

                if (showNotDeleted && !showDeleted)
                {
                    query = query.Where(t => !t.Deleted);
                }
                if (!showNotDeleted && showDeleted)
                {
                    query = query.Where(t => t.Deleted);
                }

                if (id != 0)
                {
                    query = query.Where(t => t.ID == id);
                }
                if (name != null && name != "")
                {
                    query = query.Where(t => t.Name.ToLower().Contains(Validator.ValidateString(name)));
                }
                if (city != null && city != "")
                {
                    query = query.Where(t => t.GetCity().Name == city);
                }

                if (fDate != null && sDate != null)
                {
                    query = query.Where(t => t.CheckDate(fDate, sDate));
                    //query = query.Where(t => CheckDate(Fdate, Sdate, t));
                }
                else if (fDate != null && sDate == null)
                {
                    query = query.Where(t => t.CheckDate(fDate, true));
                    //query = query.Where(t => CheckDate(Fdate, t));
                }
                else if (fDate == null && sDate != null)
                {
                    query = query.Where(t => t.CheckDate(sDate, false));
                    //query = query.Where(t => CheckDate(Fdate, t));
                }

                if (fPrice != null && fPrice != "" && sPrice != null && sPrice != "")
                {
                    decimal valFirstPrice = Validator.ValidateDecimal(fPrice);
                    decimal valSecondPrice = Validator.ValidateDecimal(sPrice);
                    if (valFirstPrice != 0 && valSecondPrice != 0)
                    {
                        query = query.Where(t => t.CheckPrice(valFirstPrice, valSecondPrice));
                    }
                    else
                    {
                        //что то придумать при ошибке парсинга. Возможно записывать в логи, но не учитывать этот параметр в фильтре
                    }

                }
                else if (fPrice != null && (sPrice == null || sPrice == ""))
                {
                    decimal valFirstPrice = Validator.ValidateDecimal(fPrice);
                    if (valFirstPrice != 0)
                    {
                        query = query.Where(t => t.CheckPrice(valFirstPrice, true));
                    }
                    else
                    {
                        //что то придумать при ошибке парсинга. Возможно записывать в логи, но не учитывать этот параметр в фильтре
                    }
                }
                else if ((fPrice == null || fPrice == "") && sPrice != null)
                {
                    decimal valSecondPrice = Validator.ValidateDecimal(sPrice);
                    if (valSecondPrice != 0)
                    {
                        query = query.Where(t => t.CheckPrice(valSecondPrice, false));
                    }
                    else
                    {
                        //что то придумать при ошибке парсинга. Возможно записывать в логи, но не учитывать этот параметр в фильтре
                    }
                }

                //sorting
                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(t => t.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(t => t.ID); }
                    if (sorting == "name") { query = query.OrderBy(t => t.Name); }
                    if (sorting == "city") { query = query.OrderBy(t => t.GetCity().Name); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(t => t.ID); }
                    if (sorting == "name") { query = query.OrderByDescending(t => t.Name); }
                    if (sorting == "city") { query = query.OrderByDescending(t => t.GetCity().Name); }
                }

                genCount = query.Count();

                query = query.Skip((page - 1) * count).Take(count);

                foreach (var tour in query)
                {
                    tours.Add(tour);
                }
                return tours;
            }
        }

        public static ObservableCollection<TourItem> GetToursForTable(bool showNotDeleted, bool showDeleted, int id, string name, string city,
            DateTime? fDate, DateTime? sDate, string fPrice, string sPrice,
            string sorting, string ascOrDesc, int count, int Page, ref int genCount)
        {
            ObservableCollection<TourItem> tableTours = new ObservableCollection<TourItem>();
            List<Tour> tours = GetTours(showNotDeleted, showDeleted, id, name, city, fDate, sDate,
                fPrice, sPrice, sorting, ascOrDesc, count, Page, ref genCount);
            foreach (Tour tour in tours)
            {
                tableTours.Add(new TourItem
                {
                    ID = tour.ID,
                    Name = tour.Name,
                    City = tour.GetCity().Name
                });
            }
            return tableTours;
        }
        public static IEnumerable<int> GetIdsForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var tour in db.Tours)
                {
                    yield return tour.ID;
                }
            }
        }
        public static IEnumerable<string> GetToursForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var tour in db.Tours)
                {
                    yield return tour.Name;
                }
            }
        }
        public static IEnumerable<string> GetCitiesForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var tour in db.Tours)
                {
                    yield return tour.GetCity().Name;
                }
            }
        }

        public static Tour GetTourByName(string name)
        {
            using (AppContext db = new AppContext())
            {
                return db.Tours.Where(c => c.Name == name).FirstOrDefault();
            }
        }

        public static List<TourDatesPrice> GetTourDatesPrices(Tour tour)
        {
            List<TourDatesPrice> tourDatesPrices = new List<TourDatesPrice>();
            using (AppContext db = new AppContext())
            {
                foreach (TourDatesPrice tourDatesPrice in db.TourDatesPrices)
                {
                    if (tourDatesPrice.TourID == tour.ID)
                    {
                        tourDatesPrices.Add(tourDatesPrice);
                    }
                }
                return tourDatesPrices;
            }
        }

        public static bool CheckDate(DateTime? date, Tour tour)
        {
            bool result = false;
            foreach (var tdp in GetTourDatesPrices(tour))
            {
                if (tdp.FlyDateThere.Value.Date == date) { result = true; return result; }
            }
            return result;
        }
        public static bool CheckDate(DateTime? fdate, DateTime? sdate, Tour tour)
        {
            bool result = false;
            foreach (var tdp in GetTourDatesPrices(tour))
            {
                if (tdp.FlyDateThere.Value.Date >= fdate && tdp.FlyDateThere.Value.Date <= sdate) { result = true; return result; }
            }
            return result;
        }

        public static Tour GetTourByID(int id)
        {
            using (AppContext db = new AppContext())
            {
                return db.Tours.Where(t => t.ID == id).FirstOrDefault();
            }
        }
    }

    public class TourItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }
}
