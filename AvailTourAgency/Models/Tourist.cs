using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class Tourist: Person
    {
        public Nullable<DateTime> DateOfBirth { get; set; }
        public int DocumentType { get; set; }
        public string DocumentData { get; set; }
        public Tourist() { }
        public Tourist(string fio, string phoneNumber, DateTime? dateOfBirth, int documentType, string documentData)
        {
            this.FIO = fio;
            this.PhoneNumber = phoneNumber;
            this.DateOfBirth = dateOfBirth;
            this.DocumentType = documentType;
            this.DocumentData = documentData;
        }
        public Result AddTourist()
        {
            Result result = Check(false);
            if (result.Success)
            {
                using (AppContext db = new AppContext())
                {
                    db.Tourists.Add(this);
                    db.SaveChanges();
                }
            }
            return result;
        }
        public Result DelTourist()
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
        public Result EditTourist()
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
        public Result Check(bool edit)
        {
            Result result = new Result();
            result.Success = true;
            if (this.FIO == "") { result.Success = false; result.Description = "empty_fio;"; }
            if (this.DateOfBirth == null) { result.Success = false; result.Description += "empty_dateofbirth;"; }
            if (this.DocumentType == 0) { result.Success = false; result.Description += "empty_documenttype;"; }
            if (this.DocumentData == "") { result.Success = false; result.Description += "empty_documentdata;"; }

            //Regex passregex = new Regex(@"^\d");
            //if (!passregex.IsMatch(this.DocumentData)) { result.Success = false; result.Description += "wrong_format_documentdata;"; }

            if (this.PhoneNumber == "") { result.Success = false; result.Description += "empty_phonenumber;"; }

            Regex phoneregex = new Regex(@"^\+\d{1}\(\d{3}\)\d{3}\-\d{2}\-\d{2}");
            if (!phoneregex.IsMatch(this.PhoneNumber)) { result.Success = false; result.Description += "wrong_format_phonenumber;"; }

            if (!edit)
            {
                using (AppContext db = new AppContext())
                {
                    Tourist Tourist = db.Tourists.Where(c => c.DocumentType == c.DocumentType &&
                                                        c.DocumentData == this.DocumentData && !c.Deleted).FirstOrDefault();
                    if (Tourist != null) { result.Success = false; result.Description += "duplication;"; }
                }
            }
            return result;
        }

    }
    //public enum DocumentType
    //{
    //    Паспорт,
    //    Свидетельство_о_рождении,
    //    Заграничный_паспорт
    //}
}
