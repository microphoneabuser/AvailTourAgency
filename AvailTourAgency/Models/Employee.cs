using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace AvailTourAgency.Models
{
    public class Employee: Person
    {
        public string INN { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        //public Role Role { get; set; }
        public int RoleID { get; set; }
        public string Image { get; set; }
        public Employee() { }
        public Employee(string fio, string passport, string phonenumber, string inn, int roleID, string login) 
        {
            this.FIO = fio;
            this.Passport = passport;
            this.PhoneNumber = phonenumber;
            this.INN = inn;
            this.RoleID = roleID;
            this.Login = login;
        }
        public Role GetRole()
        {
            using (AppContext db = new AppContext())
            {
                return db.Roles.Where(r => r.ID == this.RoleID).FirstOrDefault();
            }
        }
        public Result AddEmployee()
        {
            Result result = Check(false);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.Employees.Add(this);
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result DelEmployee()
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
        public Result EditEmployee()
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

        public Result SetPass(string pass)
        {
            Result result = new Result();

            if (pass == "")
            {
                result.Success = false;
                result.Description = "empty_pass";
                return result;
            }
            else
            {
                pass = User.ComputeHash(pass, ConfigurationManager.AppSettings["HashSalt"]);
                using (AppContext db = new AppContext())
                {
                    db.Employees.Where(e => e.ID == this.ID).FirstOrDefault().Pass = pass;
                    db.SaveChanges();
                }
                this.Pass = pass;
                result.Success = true;
                return result;
            }

        }
        public Result SetImage(string path)
        {
            Result result = new Result();

            if (path == "")
            {
                result.Success = false;
                result.Description = "empty_path";
                return result;
            }
            else
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string fileName = new string(Enumerable.Repeat(chars, 10)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                string newPath = $"Assets/{fileName}.jpg";

                var appDir = System.AppDomain.CurrentDomain.BaseDirectory.Remove(AppDomain.CurrentDomain.BaseDirectory.Length - 11, 10);
                var relativePath = $"Assets\\{fileName}.jpg";
                var fullPath = Path.Combine(appDir, relativePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                File.Copy(path, fullPath);

                using (AppContext db = new AppContext())
                {
                    db.Employees.Where(e => e.ID == this.ID).FirstOrDefault().Image = newPath;
                    db.SaveChanges();
                }
                this.Image = newPath;
                result.Success = true;
                return result;
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

            if (this.RoleID == 0) { result.Success = false; result.Description += "empty_role;"; }
            if (this.INN == "") { result.Success = false; result.Description += "empty_inn"; }

            Regex innregex = new Regex(@"^\d{12}");
            if (!innregex.IsMatch(this.INN)) { result.Success = false; result.Description += "wrong_format_inn;"; }


            if (!edit)
            {
                using (AppContext db = new AppContext())
                {
                    Employee Employee = db.Employees.Where(c => c.FIO == this.FIO &&
                                                    c.Passport == this.Passport &&
                                                    c.PhoneNumber == this.PhoneNumber && !c.Deleted).FirstOrDefault();
                    if (Employee != null) { result.Success = false; result.Description += "duplication;"; }
                }
            }
            return result;
        }
    }
}
