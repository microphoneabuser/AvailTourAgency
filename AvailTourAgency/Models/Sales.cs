using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public static class Sales
    {
        public static List<Sale> GetSales(bool showNotDeleted, bool showDeleted, int id, string client, 
            DateTime? fDateSale, DateTime? sDateSale, 
            string city, string tour, DateTime? fDateFly, DateTime? sDateFly, string hotel, string fPrice, string sPrice,
            string sorting, string ascOrDesc, int count, int page , ref int genCount)
        {
            List<Sale> sales = new List<Sale>();
            using (AppContext db = new AppContext())
            {
                var query = (from s in db.Sales
                            select s).AsEnumerable();
                //Проверка нужно ли показывать удаленные или не удаленные продажи
                if (showNotDeleted && !showDeleted)
                {
                    query = query.Where(s => !s.Deleted);
                }
                else if (!showNotDeleted && showDeleted)
                {
                    query = query.Where(s => s.Deleted);
                }
                
                if (id != 0)
                {
                    query = query.Where(s => s.ID == id);
                }

                if (client != null && client != "")
                {
                    string validClient = Validator.ValidateString(client);
                    query = query.Where(s => s.GetClient().FIO.ToLower().Contains(validClient));
                }

                //проверка даты продажи
                if (fDateSale != null && sDateSale != null)
                {
                    query = query.Where(s => s.CheckSaleDate(fDateSale, sDateSale));
                }
                else if (fDateSale != null && sDateSale == null)
                {
                    query = query.Where(s => s.CheckSaleDate(fDateSale, true));
                }
                else if (fDateSale == null && sDateSale != null)
                {
                    query = query.Where(s => s.CheckSaleDate(sDateSale, false));
                }


                if (city != null && city != "")
                {
                    query = query.Where(s => s.GetCity().Name.ToLower() == Validator.ValidateString(city));
                }

                if (tour != null && tour != "")
                {
                    query = query.Where(s => s.GetTour().Name.ToLower().Contains(Validator.ValidateString(tour)));
                }

                //проверка даты продажи
                if (fDateFly != null && sDateFly != null)
                {
                    query = query.Where(s => s.CheckFlyDate(fDateFly, sDateFly));
                }
                else if (fDateFly != null && sDateFly == null)
                {
                    query = query.Where(s => s.CheckFlyDate(fDateFly, true));
                }
                else if (fDateFly == null && sDateFly != null)
                {
                    query = query.Where(s => s.CheckFlyDate(sDateFly, false));
                }



                if (hotel != null && hotel != "")
                {
                    query = query.Where(s => s.GetHotel().Name.ToLower().Contains(Validator.ValidateString(hotel)));
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


                //sorting
                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(s => s.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(s => s.ID); }
                    if (sorting == "client") { query = query.OrderBy(s => s.GetClient().FIO); }
                    if (sorting == "saledate") { query = query.OrderBy(s => s.SaleDate); }
                    if (sorting == "city") { query = query.OrderBy(s => s.GetCity().Name); }
                    if (sorting == "tour") { query = query.OrderBy(s => s.GetTour().Name); }
                    if (sorting == "flydate") { query = query.OrderBy(s => s.GetTourDatesPrice().FlyDateThere); }
                    if (sorting == "hotel") { query = query.OrderBy(s => s.GetHotel().Name); }
                    if (sorting == "employee") { query = query.OrderBy(s => s.GetEmployee().FIO); }
                    if (sorting == "price") { query = query.OrderBy(s => s.Price); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(s => s.ID); }
                    if (sorting == "client") { query = query.OrderByDescending(s => s.GetClient().FIO); }
                    if (sorting == "saledate") { query = query.OrderByDescending(s => s.SaleDate); }
                    if (sorting == "city") { query = query.OrderByDescending(s => s.GetCity().Name); }
                    if (sorting == "tour") { query = query.OrderByDescending(s => s.GetTour().Name); } 
                    if (sorting == "flydate") { query = query.OrderByDescending(s => s.GetTourDatesPrice().FlyDateThere); }
                    if (sorting == "hotel") { query = query.OrderByDescending(s => s.GetHotel().Name);}
                    if (sorting == "employee") { query = query.OrderByDescending(s => s.GetEmployee().FIO); }
                    if (sorting == "price") { query = query.OrderByDescending(s => s.Price); }
                }

                genCount = query.Count();

                query = query.Skip((page - 1) * count).Take(count);

                foreach (Sale sale in query)
                {
                    sales.Add(sale);
                }
                return sales;
            }
        }

        public static ObservableCollection<SaleItem> GetSalesForTable(bool showNotDeleted, bool showDeleted, int id, string client,
            DateTime? fDateSale, DateTime? sDateSale,
            string city, string tour, DateTime? fDateFly, DateTime? sDateFly, string hotel, string fPrice, string sPrice,
            string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            ObservableCollection<SaleItem> tableSales = new ObservableCollection<SaleItem>();
            List<Sale> sales = GetSales(showNotDeleted, showDeleted, id, client, fDateSale, sDateSale, city, tour, fDateFly, sDateFly,
                hotel, fPrice, sPrice, sorting, ascOrDesc, count, page, ref genCount);
            foreach (Sale sale in sales)
            {
                TourDatesPrice currTDP = sale.GetTourDatesPrice();
                string flyDateThere = "";
                if (currTDP.FlyDateThere != null) { flyDateThere = currTDP.FlyDateThere.Value.Date.ToShortDateString(); }

                //TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;

                ////var dateTime = DateTime.SpecifyKind(sale.SaleDate, DateTimeKind.Unspecified);

                //string saleDate = TimeZoneInfo.ConvertTimeFromUtc(sale.SaleDate, systemTimeZone).ToString();

                tableSales.Add(new SaleItem
                {
                    ID = sale.ID,
                    Client = sale.GetClient().FIO,
                    SaleDate = sale.GetSaleDate().ToString(),
                    City = sale.GetCity().Name,
                    Tour = sale.GetTour().Name,
                    FlyDate = flyDateThere,
                    Hotel = sale.GetHotel().Name,
                    Employee = sale.GetEmployee().FIO,
                    Price = sale.Price.ToString()
                });
            }
            return tableSales;
        }





        public static List<Sale> GetSalesEmployee(bool showNotDeleted, bool showDeleted, int id, string client,
            DateTime? fDateSale, DateTime? sDateSale,
            string city, string tour, DateTime? fDateFly, DateTime? sDateFly, string hotel, string fPrice, string sPrice,
            string sorting, string ascOrDesc, int count, int page, ref int genCount, int employeeID)
        {
            List<Sale> sales = new List<Sale>();
            using (AppContext db = new AppContext())
            {
                var query = (from s in db.Sales
                             select s).AsEnumerable();
                //Проверка нужно ли показывать удаленные или не удаленные продажи
                if (showNotDeleted && !showDeleted)
                {
                    query = query.Where(s => !s.Deleted);
                }
                else if (!showNotDeleted && showDeleted)
                {
                    query = query.Where(s => s.Deleted);
                }

                if (id != 0)
                {
                    query = query.Where(s => s.ID == id);
                }

                if (client != null && client != "")
                {
                    string validClient = Validator.ValidateString(client);
                    query = query.Where(s => s.GetClient().FIO.ToLower().Contains(validClient));
                }

                query = query.Where(s => s.EmployeeID == employeeID);


                //проверка даты продажи
                if (fDateSale != null && sDateSale != null)
                {
                    query = query.Where(s => s.CheckSaleDate(fDateSale, sDateSale));
                }
                else if (fDateSale != null && sDateSale == null)
                {
                    query = query.Where(s => s.CheckSaleDate(fDateSale, true));
                }
                else if (fDateSale == null && sDateSale != null)
                {
                    query = query.Where(s => s.CheckSaleDate(sDateSale, false));
                }


                if (city != null && city != "")
                {
                    query = query.Where(s => s.GetCity().Name.ToLower() == Validator.ValidateString(city));
                }

                if (tour != null && tour != "")
                {
                    query = query.Where(s => s.GetTour().Name.ToLower().Contains(Validator.ValidateString(tour)));
                }

                //проверка даты продажи
                if (fDateFly != null && sDateFly != null)
                {
                    query = query.Where(s => s.CheckFlyDate(fDateFly, sDateFly));
                }
                else if (fDateFly != null && sDateFly == null)
                {
                    query = query.Where(s => s.CheckFlyDate(fDateFly, true));
                }
                else if (fDateFly == null && sDateFly != null)
                {
                    query = query.Where(s => s.CheckFlyDate(sDateFly, false));
                }



                if (hotel != null && hotel != "")
                {
                    query = query.Where(s => s.GetHotel().Name.ToLower().Contains(Validator.ValidateString(hotel)));
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


                //sorting
                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(s => s.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(s => s.ID); }
                    if (sorting == "client") { query = query.OrderBy(s => s.GetClient().FIO); }
                    if (sorting == "saledate") { query = query.OrderBy(s => s.SaleDate); }
                    if (sorting == "city") { query = query.OrderBy(s => s.GetCity().Name); }
                    if (sorting == "tour") { query = query.OrderBy(s => s.GetTour().Name); }
                    if (sorting == "flydate") { query = query.OrderBy(s => s.GetTourDatesPrice().FlyDateThere); }
                    if (sorting == "hotel") { query = query.OrderBy(s => s.GetHotel().Name); }
                    if (sorting == "employee") { query = query.OrderBy(s => s.GetEmployee().FIO); }
                    if (sorting == "price") { query = query.OrderBy(s => s.Price); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(s => s.ID); }
                    if (sorting == "client") { query = query.OrderByDescending(s => s.GetClient().FIO); }
                    if (sorting == "saledate") { query = query.OrderByDescending(s => s.SaleDate); }
                    if (sorting == "city") { query = query.OrderByDescending(s => s.GetCity().Name); }
                    if (sorting == "tour") { query = query.OrderByDescending(s => s.GetTour().Name); }
                    if (sorting == "flydate") { query = query.OrderByDescending(s => s.GetTourDatesPrice().FlyDateThere); }
                    if (sorting == "hotel") { query = query.OrderByDescending(s => s.GetHotel().Name); }
                    if (sorting == "employee") { query = query.OrderByDescending(s => s.GetEmployee().FIO); }
                    if (sorting == "price") { query = query.OrderByDescending(s => s.Price); }
                }

                genCount = query.Count();

                query = query.Skip((page - 1) * count).Take(count);

                foreach (Sale sale in query)
                {
                    sales.Add(sale);
                }
                return sales;
            }
        }

        public static ObservableCollection<SaleItem> GetSalesForTableEmployee(bool showNotDeleted, bool showDeleted, int id, string client,
            DateTime? fDateSale, DateTime? sDateSale,
            string city, string tour, DateTime? fDateFly, DateTime? sDateFly, string hotel, string fPrice, string sPrice,
            string sorting, string ascOrDesc, int count, int page, ref int genCount, int employeeID)
        {
            ObservableCollection<SaleItem> tableSales = new ObservableCollection<SaleItem>();
            List<Sale> sales = GetSalesEmployee(showNotDeleted, showDeleted, id, client, fDateSale, sDateSale, city, tour, fDateFly, sDateFly,
                hotel, fPrice, sPrice, sorting, ascOrDesc, count, page, ref genCount, employeeID);
            foreach (Sale sale in sales)
            {
                TourDatesPrice currTDP = sale.GetTourDatesPrice();
                string flyDateThere = "";
                if (currTDP.FlyDateThere != null) { flyDateThere = currTDP.FlyDateThere.Value.Date.ToShortDateString(); }

                //TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;

                ////var dateTime = DateTime.SpecifyKind(sale.SaleDate, DateTimeKind.Unspecified);

                //string saleDate = TimeZoneInfo.ConvertTimeFromUtc(sale.SaleDate, systemTimeZone).ToString();

                tableSales.Add(new SaleItem
                {
                    ID = sale.ID,
                    Client = sale.GetClient().FIO,
                    SaleDate = sale.GetSaleDate().ToString(),
                    City = sale.GetCity().Name,
                    Tour = sale.GetTour().Name,
                    FlyDate = flyDateThere,
                    Hotel = sale.GetHotel().Name,
                    Employee = sale.GetEmployee().FIO,
                    Price = sale.Price.ToString()
                });
            }
            return tableSales;
        }







        public static List<Sale> GetSalesAddService(bool showNotDeleted, bool showDeleted, int id, string client,
            DateTime? fDateSale, DateTime? sDateSale,
            string city, string tour, DateTime? fDateFly, DateTime? sDateFly, string hotel, string fPrice, string sPrice,
            string sorting, string ascOrDesc, int count, int page, ref int genCount, int addServiceID)
        {
            List<Sale> sales = new List<Sale>();
            using (AppContext db = new AppContext())
            {
                var query = (from s in db.Sales
                             select s).AsEnumerable();
                //Проверка нужно ли показывать удаленные или не удаленные продажи
                if (showNotDeleted && !showDeleted)
                {
                    query = query.Where(s => !s.Deleted);
                }
                else if (!showNotDeleted && showDeleted)
                {
                    query = query.Where(s => s.Deleted);
                }

                if (id != 0)
                {
                    query = query.Where(s => s.ID == id);
                }

                if (client != null && client != "")
                {
                    string validClient = Validator.ValidateString(client);
                    query = query.Where(s => s.GetClient().FIO.ToLower().Contains(validClient));
                }

                query = query.Where(s => s.GetAddServices().Contains(AddServices.GetAddServiceByID(addServiceID)));

                //проверка даты продажи
                if (fDateSale != null && sDateSale != null)
                {
                    query = query.Where(s => s.CheckSaleDate(fDateSale, sDateSale));
                }
                else if (fDateSale != null && sDateSale == null)
                {
                    query = query.Where(s => s.CheckSaleDate(fDateSale, true));
                }
                else if (fDateSale == null && sDateSale != null)
                {
                    query = query.Where(s => s.CheckSaleDate(sDateSale, false));
                }


                if (city != null && city != "")
                {
                    query = query.Where(s => s.GetCity().Name.ToLower() == Validator.ValidateString(city));
                }

                if (tour != null && tour != "")
                {
                    query = query.Where(s => s.GetTour().Name.ToLower().Contains(Validator.ValidateString(tour)));
                }

                //проверка даты продажи
                if (fDateFly != null && sDateFly != null)
                {
                    query = query.Where(s => s.CheckFlyDate(fDateFly, sDateFly));
                }
                else if (fDateFly != null && sDateFly == null)
                {
                    query = query.Where(s => s.CheckFlyDate(fDateFly, true));
                }
                else if (fDateFly == null && sDateFly != null)
                {
                    query = query.Where(s => s.CheckFlyDate(sDateFly, false));
                }



                if (hotel != null && hotel != "")
                {
                    query = query.Where(s => s.GetHotel().Name.ToLower().Contains(Validator.ValidateString(hotel)));
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


                //sorting
                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(s => s.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(s => s.ID); }
                    if (sorting == "client") { query = query.OrderBy(s => s.GetClient().FIO); }
                    if (sorting == "saledate") { query = query.OrderBy(s => s.SaleDate); }
                    if (sorting == "city") { query = query.OrderBy(s => s.GetCity().Name); }
                    if (sorting == "tour") { query = query.OrderBy(s => s.GetTour().Name); }
                    if (sorting == "flydate") { query = query.OrderBy(s => s.GetTourDatesPrice().FlyDateThere); }
                    if (sorting == "hotel") { query = query.OrderBy(s => s.GetHotel().Name); }
                    if (sorting == "employee") { query = query.OrderBy(s => s.GetEmployee().FIO); }
                    if (sorting == "price") { query = query.OrderBy(s => s.Price); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(s => s.ID); }
                    if (sorting == "client") { query = query.OrderByDescending(s => s.GetClient().FIO); }
                    if (sorting == "saledate") { query = query.OrderByDescending(s => s.SaleDate); }
                    if (sorting == "city") { query = query.OrderByDescending(s => s.GetCity().Name); }
                    if (sorting == "tour") { query = query.OrderByDescending(s => s.GetTour().Name); }
                    if (sorting == "flydate") { query = query.OrderByDescending(s => s.GetTourDatesPrice().FlyDateThere); }
                    if (sorting == "hotel") { query = query.OrderByDescending(s => s.GetHotel().Name); }
                    if (sorting == "employee") { query = query.OrderByDescending(s => s.GetEmployee().FIO); }
                    if (sorting == "price") { query = query.OrderByDescending(s => s.Price); }
                }

                genCount = query.Count();

                query = query.Skip((page - 1) * count).Take(count);

                foreach (Sale sale in query)
                {
                    sales.Add(sale);
                }
                return sales;
            }
        }

        public static ObservableCollection<SaleItem> GetSalesForTableAddService(bool showNotDeleted, bool showDeleted, int id, string client,
            DateTime? fDateSale, DateTime? sDateSale,
            string city, string tour, DateTime? fDateFly, DateTime? sDateFly, string hotel, string fPrice, string sPrice,
            string sorting, string ascOrDesc, int count, int page, ref int genCount, int addServiceID)
        {
            ObservableCollection<SaleItem> tableSales = new ObservableCollection<SaleItem>();
            List<Sale> sales = GetSalesEmployee(showNotDeleted, showDeleted, id, client, fDateSale, sDateSale, city, tour, fDateFly, sDateFly,
                hotel, fPrice, sPrice, sorting, ascOrDesc, count, page, ref genCount, addServiceID);
            foreach (Sale sale in sales)
            {
                TourDatesPrice currTDP = sale.GetTourDatesPrice();
                string flyDateThere = "";
                if (currTDP.FlyDateThere != null) { flyDateThere = currTDP.FlyDateThere.Value.Date.ToShortDateString(); }

                //TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;

                ////var dateTime = DateTime.SpecifyKind(sale.SaleDate, DateTimeKind.Unspecified);

                //string saleDate = TimeZoneInfo.ConvertTimeFromUtc(sale.SaleDate, systemTimeZone).ToString();

                tableSales.Add(new SaleItem
                {
                    ID = sale.ID,
                    Client = sale.GetClient().FIO,
                    SaleDate = sale.GetSaleDate().ToString(),
                    City = sale.GetCity().Name,
                    Tour = sale.GetTour().Name,
                    FlyDate = flyDateThere,
                    Hotel = sale.GetHotel().Name,
                    Employee = sale.GetEmployee().FIO,
                    Price = sale.Price.ToString()
                });
            }
            return tableSales;
        }








        public static IEnumerable<int> GetIdsForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var sale in db.Sales)
                {
                    yield return sale.ID;
                }
            }
        }

        public static Sale GetSaleByID(int id)
        {
            using (AppContext db = new AppContext())
            {
                return db.Sales.Where(s => s.ID == id).FirstOrDefault();
            }
        }

    }
    public class SaleItem
    {
        public int ID { get; set; }
        public string Client { get; set; }
        public string SaleDate { get; set; }
        public string City { get; set; }
        public string Tour { get; set; }
        public string FlyDate { get; set; }
        public string Hotel { get; set; }
        public string Employee { get; set; }
        public string Price { get; set; }
    }
}
