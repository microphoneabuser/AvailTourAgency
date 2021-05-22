//using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class AddService
    {
        //attribs
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        //public Tour Tour { get; set; }
        public int? TourID { get; set; }
        public bool Deleted { get; set; }

        public AddService() { }
        public AddService(string name, decimal price, int tourID, string description) 
        {
            this.Name = name;
            this.Price = price;
            this.TourID = tourID;
            this.Description = description;
        }
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

        public Tour GetTour()
        {
            using (AppContext db = new AppContext())
            {
                Tour tour = db.Tours.Where(t => t.ID == this.TourID).FirstOrDefault();
                if (tour != null)
                {
                    return tour;
                }
                else
                {
                    tour = new Tour();
                    tour.Name = "";
                    return tour;
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

        //methods
        public Result AddAddService()
        {
            Result result = Check(false);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.AddServices.Add(this);
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result DelAddService()
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
        public Result EditAddService()
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

        //public static List<Sale> GetSales(bool showNotDeleted, bool showDeleted, int id, string client,
        //    DateTime? fDateSale, DateTime? sDateSale,
        //    string city, string tour, DateTime? fDateFly, DateTime? sDateFly, string hotel, string fPrice, string sPrice,
        //    string sorting, string ascOrDesc, int count, int page, ref int genCount)
        //{
        //    List<Sale> sales = new List<Sale>();
        //    using (AppContext db = new AppContext())
        //    {
        //        var query = from s in db.Sales
        //                    join ais in db.AddServicesInSales on s.ID equals ais.SaleID
        //                    into sAisTemp
        //                    from sAis in sAisTemp.DefaultIfEmpty()
        //                    join a in db.AddServices on sAis.AddServiceID equals a.ID
        //                    into aTemp
        //                    from a in aTemp.DefaultIfEmpty()
        //                    select new
        //                    {
        //                        ID = s.ID,
        //                        ClientID = s.ClientID,
        //                        SaleDate = s.SaleDate,
        //                        TourDatesPriceID = s.TourDatesPriceID,
        //                        Price = s.Price,
        //                        PaymentMethod = s.PaymentMethod,
        //                        EmployeeID = s.EmployeeID,
        //                        AddServiceID = a.ID,
        //                        Deleted = s.Deleted,
        //                        GetClient = s.GetClient(),

        //                    };
        //        //(from a in db.AddServices
        //        //            join ais in db.AddServicesInSales on a.ID equals ais.AddServiceID
        //        //            join s in db.Sales on ais.SaleID equals s.ID 
        //        //            select new { }
        //        //            ).AsEnumerable();
        //        if (showNotDeleted && !showDeleted)
        //        {
        //            query = query.Where(s => !s.Deleted);
        //        }
        //        else if (!showNotDeleted && showDeleted)
        //        {
        //            query = query.Where(s => s.Deleted);
        //        }

        //        if (id != 0)
        //        {
        //            query = query.Where(s => s.ID == id);
        //        }

        //        if (client != null && client != "")
        //        {
        //            string validClient = Validator.ValidateString(client);
        //            query = query.Where(s => s.GetClient().FIO.ToLower().Contains(validClient));
        //        }

        //        //проверка даты продажи
        //        if (fDateSale != null && sDateSale != null)
        //        {
        //            query = query.Where(s => s.CheckSaleDate(fDateSale, sDateSale));
        //        }
        //        else if (fDateSale != null && sDateSale == null)
        //        {
        //            query = query.Where(s => s.CheckSaleDate(fDateSale, true));
        //        }
        //        else if (fDateSale == null && sDateSale != null)
        //        {
        //            query = query.Where(s => s.CheckSaleDate(sDateSale, false));
        //        }


        //        if (city != null && city != "")
        //        {
        //            query = query.Where(s => s.GetCity().Name.ToLower() == Validator.ValidateString(city));
        //        }

        //        if (tour != null && tour != "")
        //        {
        //            query = query.Where(s => s.GetTour().Name.ToLower() == Validator.ValidateString(tour));
        //        }

        //        //проверка даты продажи
        //        if (fDateFly != null && sDateFly != null)
        //        {
        //            query = query.Where(s => s.CheckFlyDate(fDateFly, sDateFly));
        //        }
        //        else if (fDateFly != null && sDateFly == null)
        //        {
        //            query = query.Where(s => s.CheckFlyDate(fDateFly, true));
        //        }
        //        else if (fDateFly == null && sDateFly != null)
        //        {
        //            query = query.Where(s => s.CheckFlyDate(sDateFly, false));
        //        }



        //        if (hotel != null && hotel != "")
        //        {
        //            query = query.Where(s => s.GetHotel().Name.ToLower() == Validator.ValidateString(hotel));
        //        }



        //        if (fPrice != null && fPrice != "" && sPrice != null && sPrice != "")
        //        {
        //            decimal valFirstPrice = Validator.ValidateDecimal(fPrice);
        //            decimal valSecondPrice = Validator.ValidateDecimal(sPrice);
        //            if (valFirstPrice != 0 && valSecondPrice != 0)
        //            {
        //                query = query.Where(s => s.CheckPrice(valFirstPrice, valSecondPrice));
        //            }
        //            else
        //            {
        //                //что то придумать при ошибке парсинга. Возможно записывать в логи, но не учитывать этот параметр в фильтре
        //            }

        //        }
        //        else if (fPrice != null && (sPrice == null || sPrice == ""))
        //        {
        //            decimal valFirstPrice = Validator.ValidateDecimal(fPrice);
        //            if (valFirstPrice != 0)
        //            {
        //                query = query.Where(s => s.CheckPrice(valFirstPrice, true));
        //            }
        //            else
        //            {
        //                //что то придумать при ошибке парсинга. Возможно записывать в логи, но не учитывать этот параметр в фильтре
        //            }
        //        }
        //        else if ((fPrice == null || fPrice == "") && sPrice != null)
        //        {
        //            decimal valSecondPrice = Validator.ValidateDecimal(sPrice);
        //            if (valSecondPrice != 0)
        //            {
        //                query = query.Where(s => s.CheckPrice(valSecondPrice, false));
        //            }
        //            else
        //            {
        //                //что то придумать при ошибке парсинга. Возможно записывать в логи, но не учитывать этот параметр в фильтре
        //            }
        //        }


        //        //sorting
        //        if (sorting == null || sorting == "")
        //        {
        //            query = query.OrderByDescending(s => s.ID);
        //        }
        //        if (ascOrDesc == "asc")
        //        {
        //            if (sorting == "id") { query = query.OrderBy(s => s.ID); }
        //            if (sorting == "client") { query = query.OrderBy(s => s.GetClient().FIO); }
        //            if (sorting == "saledate") { query = query.OrderBy(s => s.SaleDate); }
        //            if (sorting == "city") { query = query.OrderBy(s => s.GetCity().Name); }
        //            if (sorting == "tour") { query = query.OrderBy(s => s.GetTour().Name); }
        //            if (sorting == "flydate") { query = query.OrderBy(s => s.GetTourDatesPrice().FlyDateThere); }
        //            if (sorting == "hotel") { query = query.OrderBy(s => s.GetHotel().Name); }
        //            if (sorting == "employee") { query = query.OrderBy(s => s.GetEmployee().FIO); }
        //            if (sorting == "price") { query = query.OrderBy(s => s.Price); }
        //        }
        //        if (ascOrDesc == "desc")
        //        {
        //            if (sorting == "id") { query = query.OrderByDescending(s => s.ID); }
        //            if (sorting == "client") { query = query.OrderByDescending(s => s.GetClient().FIO); }
        //            if (sorting == "saledate") { query = query.OrderByDescending(s => s.SaleDate); }
        //            if (sorting == "city") { query = query.OrderByDescending(s => s.GetCity().Name); }
        //            if (sorting == "tour") { query = query.OrderByDescending(s => s.GetTour().Name); }
        //            if (sorting == "flydate") { query = query.OrderByDescending(s => s.GetTourDatesPrice().FlyDateThere); }
        //            if (sorting == "hotel") { query = query.OrderByDescending(s => s.GetHotel().Name); }
        //            if (sorting == "employee") { query = query.OrderByDescending(s => s.GetEmployee().FIO); }
        //            if (sorting == "price") { query = query.OrderByDescending(s => s.Price); }
        //        }

        //        genCount = query.Count();

        //        query = query.Skip((page - 1) * count).Take(count);

        //        foreach (Sale sale in query)
        //        {
        //            sales.Add(sale);
        //        }
        //        return sales;
        //    }
        //}
        public Result Check(bool edit)
        {
            Result result = new Result();
            result.Success = true;
            if (this.Name == "") { result.Success = false; result.Description = "empty_name;"; }
            if (this.Price == 0) { result.Success = false; result.Description += "empty_price;"; }
            if (!edit)
            {
                using (AppContext db = new AppContext())
                {
                    AddService serv = db.AddServices.Where(s => s.Name == this.Name && !s.Deleted).FirstOrDefault();
                    if (serv != null) { result.Success = false; result.Description += "duplication;"; }
                }
            }
            return result;
        }
    }
}
