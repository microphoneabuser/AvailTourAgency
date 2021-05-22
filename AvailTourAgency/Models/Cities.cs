using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public static class Cities
    {
        public static List<City> GetCities(bool showNotDeleted, bool showDeleted, int id, string name, string country,
            string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            List<City> cities = new List<City>();
            using (AppContext db = new AppContext())
            {
                var query = (from c in db.Cities
                             select c).AsEnumerable();

                if (showNotDeleted && !showDeleted)
                {
                    query = query.Where(c => !c.Deleted);
                }
                if (!showNotDeleted && showDeleted)
                {
                    query = query.Where(c => c.Deleted);
                }

                if (id != 0)
                {
                    query = query.Where(c => c.ID == id);
                }
                if (name != null & name != "")
                {
                    string validName = Validator.ValidateString(name);
                    query = query.Where(c => c.Name.ToLower().Contains(validName));
                }
                if (country != null & country != "")
                {
                    string validCountry = Validator.ValidateString(country);
                    query = query.Where(c => c.Country.ToLower() == validCountry);
                }

                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(c => c.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(c => c.ID); }
                    if (sorting == "name") { query = query.OrderBy(c => c.Name); }
                    if (sorting == "country") { query = query.OrderBy(c => c.Country); }
                    if (sorting == "description") { query = query.OrderBy(c => c.Description); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(h => h.ID); }
                    if (sorting == "name") { query = query.OrderByDescending(h => h.Name); }
                    if (sorting == "country") { query = query.OrderByDescending(h => h.Country); }
                    if (sorting == "description") { query = query.OrderByDescending(e => e.Description); }
                }

                genCount = query.Count();

                query = query.Skip((page - 1) * count).Take(count);

                foreach (var city in query)
                {
                    cities.Add(city);
                }
                return cities;
            }
        }

        public static ObservableCollection<CityItem> GetCitiesForTable(bool showNotDeleted, bool showDeleted, int id, string name, string country,
            string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            ObservableCollection<CityItem> tableCities = new ObservableCollection<CityItem>();
            List<City> cities = GetCities(showNotDeleted, showDeleted, id, name, country, sorting, ascOrDesc, count, page, ref genCount);
            foreach (City city in cities)
            {
                string description = "";
                if (city.Description != null && city.Description.Length < 31)
                {
                    description = city.Description;
                }
                else if (city.Description != null)
                {
                    description = city.Description.Substring(0, 30) + "...";
                }
                tableCities.Add(new CityItem
                {
                    ID = city.ID,
                    Name = city.Name,
                    Country = city.Country,
                    Description = description
                });
            }
            return tableCities;
        }

        public static IEnumerable<int> GetIdsForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var city in db.Cities)
                {
                    yield return city.ID;
                }
            }
        }



        public static IEnumerable<string> GetCitiesForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var city in db.Cities)
                {
                    yield return city.Name;
                }
            }
        }
        public static List<string> GetCountriesForFilters()
        {
            using (AppContext db = new AppContext())
            {
                List<string> countries = new List<string>();

                foreach (var city in db.Cities)
                {
                    if (!countries.Contains(city.Country))
                    {
                        countries.Add(city.Country);
                    }
                }
                return countries;
            }
        }
        public static City GetCityByName(string name)
        {
            using (AppContext db = new AppContext())
            {
                return db.Cities.Where(c => c.Name == name).FirstOrDefault();
            }
        }
        public static City GetCityByID(int id)
        {
            using (AppContext db = new AppContext())
            {
                return db.Cities.Where(c => c.ID == id).FirstOrDefault();
            }
        }
    }

    public class CityItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
    }
}
