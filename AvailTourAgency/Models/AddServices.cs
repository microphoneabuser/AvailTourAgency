using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public static class AddServices
    {
        public static List<AddService> GetAddServices(bool showNotDeleted, bool showDeleted, int id, string name, 
            string fPrice, string sPrice, string tour,
            string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            List<AddService> addServices = new List<AddService>();
            using (AppContext db = new AppContext())
            {
                var query = (from s in db.AddServices
                             select s).AsEnumerable();

                if (showNotDeleted && !showDeleted)
                {
                    query = query.Where(s => !s.Deleted);
                }
                if (!showNotDeleted && showDeleted)
                {
                    query = query.Where(s => s.Deleted);
                }

                if (id != 0)
                {
                    query = query.Where(s => s.ID == id);
                }
                if (name != null & name != "")
                {
                    string validName = Validator.ValidateString(name);
                    query = query.Where(s => s.Name.ToLower().Contains(validName));
                }

                if (fPrice != null && fPrice != "" && sPrice != null && sPrice != "")
                {
                    decimal valFirstPrice = Validator.ValidateDecimal(fPrice);
                    decimal valSecondPrice = Validator.ValidateDecimal(sPrice);
                    if (valFirstPrice != 0 && valSecondPrice != 0)
                    {
                        query = query.Where(s => s.CheckPrice(valFirstPrice, valSecondPrice));
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
                        query = query.Where(s => s.CheckPrice(valFirstPrice, true));
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
                        query = query.Where(s => s.CheckPrice(valSecondPrice, false));
                    }
                    else
                    {
                        //что то придумать при ошибке парсинга. Возможно записывать в логи, но не учитывать этот параметр в фильтре
                    }
                }

                if (tour != null & tour != "")
                {
                    string validTour = Validator.ValidateString(tour);
                    query = query.Where(h => h.GetTour().Name.ToLower().Contains(validTour));
                }

                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(h => h.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(h => h.ID); }
                    if (sorting == "name") { query = query.OrderBy(h => h.Name); }
                    if (sorting == "price") { query = query.OrderBy(h => h.Price); }
                    if (sorting == "description") { query = query.OrderBy(e => e.Description); }
                    if (sorting == "tour") { query = query.OrderBy(e => e.GetTour().Name); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(h => h.ID); }
                    if (sorting == "name") { query = query.OrderByDescending(h => h.Name); }
                    if (sorting == "price") { query = query.OrderByDescending(h => h.Price); }
                    if (sorting == "description") { query = query.OrderByDescending(e => e.Description); }
                    if (sorting == "tour") { query = query.OrderByDescending(e => e.GetTour().Name); }
                }

                genCount = query.Count();

                query = query.Skip((page - 1) * count).Take(count);

                foreach (var addService in query)
                {
                    addServices.Add(addService);
                }
                return addServices;
            }
        }

        public static ObservableCollection<AddServiceItem> GetAddServicesForTable(bool showNotDeleted, bool showDeleted, int id, string name,
            string fPrice, string sPrice, string tour,
            string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            ObservableCollection<AddServiceItem> tableAddServices = new ObservableCollection<AddServiceItem>();
            List<AddService> addServices = GetAddServices(showNotDeleted, showDeleted, id, name, fPrice, sPrice, tour, sorting, ascOrDesc, count, page, ref genCount);
            foreach (AddService service in addServices)
            {
                string description = "";
                if (service.Description != null && service.Description.Length < 31)
                {
                    description = service.Description;
                }
                else if (service.Description != null)
                {
                    description = service.Description.Substring(0, 30) + "...";
                }
                if (service.GetTour() != null)
                {
                    tableAddServices.Add(new AddServiceItem
                    {
                        ID = service.ID,
                        Name = service.Name,
                        Price = service.Price.ToString(),
                        Description = description,
                        Tour = service.GetTour().Name
                    });
                }
                else
                {
                    tableAddServices.Add(new AddServiceItem
                    {
                        ID = service.ID,
                        Name = service.Name,
                        Price = service.Price.ToString(),
                        Description = description,
                        Tour = ""
                    });
                }
            }
            return tableAddServices;
        }

        public static ObservableCollection<AddServiceItem> GetAddServicesInSaleForTable(bool showDeleted, int saleID, string sorting, string ascOrDesc)
        {
            ObservableCollection<AddServiceItem> addServices = new ObservableCollection<AddServiceItem>();
            using (AppContext db = new AppContext())
            {
                var query = (from a in db.AddServices
                             join ais in db.AddServicesInSales on a.ID equals ais.AddServiceID
                             into aAisTemp
                             from aAis in aAisTemp.DefaultIfEmpty()
                             join s in db.Sales on aAis.SaleID equals s.ID
                             into sTemp
                             from s in sTemp.DefaultIfEmpty()
                             select new
                             {
                                 ID = a.ID,
                                 Name = a.Name,
                                 Price = a.Price,
                                 Description = a.Description,
                                 Deleted = a.Deleted,
                                 Quantity = aAis.Quantity,
                                 SaleID = (s == null ? 0 : s.ID)
                             }).AsEnumerable();

                if (!showDeleted)
                {
                    query = query.Where(a => !a.Deleted);
                }

                if (saleID != 0)
                {
                    query = query.Where(a => a.SaleID == saleID);
                }

                //sorting
                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(a => a.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(a => a.ID); }
                    if (sorting == "name") { query = query.OrderBy(a => a.Name); }
                    if (sorting == "price") { query = query.OrderBy(a => a.Price); }
                    if (sorting == "quantity") { query = query.OrderBy(a => a.Quantity); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderBy(a => a.ID); }
                    if (sorting == "name") { query = query.OrderBy(a => a.Name); }
                    if (sorting == "price") { query = query.OrderBy(a => a.Price); }
                    if (sorting == "quantity") { query = query.OrderBy(a => a.Quantity); }
                }

                if (query.ToList().Count != 0)
                {
                    foreach (var addService in query)
                    {
                        addServices.Add(new AddServiceItem
                        {
                            ID = addService.ID,
                            Name = addService.Name,
                            Price = addService.Price.ToString(),
                            Quantity = addService.Quantity
                        });
                    }
                }
                return addServices;
            }
        }


        public static IEnumerable<int> GetIdsForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var service in db.AddServices)
                {
                    yield return service.ID;
                }
            }
        }
        public static AddService GetAddServiceByID(int id)
        {
            using (AppContext db = new AppContext())
            {
                return db.AddServices.Where(s => s.ID == id).FirstOrDefault();
            }
        }
    }

    public class AddServiceItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public string Tour { get; set; }
    }
}
