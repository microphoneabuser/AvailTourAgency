using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class Client : Person
    {
        public string Comment { get; set; }

        public Client() { }
        public Client(string fio, string passport, string phoneNumber, string comment) 
        {
            this.FIO = fio;
            this.Passport = passport;
            this.PhoneNumber = phoneNumber;
            this.Comment = comment;
        }

        public Result AddClient()
        {
            Result result = Check(false);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.Clients.Add(this);
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result DelClient()
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
        public Result EditClient()
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

        public List<Sale> GetSales()
        {
            using (AppContext db = new AppContext())
            {
                return db.Sales.Where(s => s.ClientID == this.ID && !s.Deleted).OrderByDescending(s => s.ID).ToList();
            }
        }

        public Result Check(bool edit)
        {
            Result result = new Result();
            result.Success = true;
            if (this.FIO == "") { result.Success = false; result.Description = "empty_fio;"; }
            
            if (this.Passport == "") { result.Success = false; result.Description += "empty_passport;"; }

            Regex passregex = new Regex(@"^\d{4}\s\d{6}$");
            if (!passregex.IsMatch(this.Passport)) { result.Success = false; result.Description += "wrong_format_passport;"; }

            if (this.PhoneNumber == "") { result.Success = false; result.Description += "empty_phonenumber;"; }

            Regex phoneregex = new Regex(@"^\+\d{1}\(\d{3}\)\d{3}\-\d{2}\-\d{2}");
            if (!phoneregex.IsMatch(this.PhoneNumber)) { result.Success = false; result.Description += "wrong_format_phonenumber;"; }

            if (!edit)
            {
                using (AppContext db = new AppContext())
                {
                    Client Client = db.Clients.Where(c => c.Passport == this.Passport && !c.Deleted).FirstOrDefault();
                    if (Client != null) { result.Success = false; result.Description += "duplication;"; }
                }
            }
            return result;
        }
    }
}
