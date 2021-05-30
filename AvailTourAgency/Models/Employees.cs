using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public static class Employees
    {
        public static List<Employee> GetEmployees(bool showNotDeleted, bool showDeleted, int id, string fio,
           string passport, string phoneNumber, string position, string inn, string login,
           string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            List<Employee> employees = new List<Employee>();
            using (AppContext db = new AppContext())
            {
                var query = (from e in db.Employees
                            select e).AsEnumerable();

                if (showNotDeleted && !showDeleted)
                {
                    query = query.Where(e => !e.Deleted);
                }
                if (!showNotDeleted && showDeleted)
                {
                    query = query.Where(e => e.Deleted);
                }

                if (id != 0)
                {
                    query = query.Where(e => e.ID == id);
                }
                if (fio != null && fio != "")
                {
                    query = query.Where(e => e.FIO.ToLower().Contains(Validator.ValidateString(fio)));
                }
                if (passport != null && passport != "" && passport != "____ ______")
                {
                    query = query.Where(e => e.Passport == passport);
                }
                if (phoneNumber != null && phoneNumber != "" && phoneNumber != "+7(___)___-__-__")
                {
                    query = query.Where(e => e.PhoneNumber == phoneNumber);
                }
                if (position != null && position != "")
                {
                    query = query.Where(e => e.GetRole().Name == position);
                }
                if (inn != null && inn != "" && inn != "____________")
                {
                    query = query.Where(e => e.INN == inn.Trim());
                }
                if (login != null && login != "")
                {
                    query = query.Where(e => e.Login.ToLower().Contains(Validator.ValidateString(login)));
                }


                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(e => e.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(e => e.ID); }
                    if (sorting == "fio") { query = query.OrderBy(e => e.FIO); }
                    if (sorting == "passport") { query = query.OrderBy(e => e.Passport); }
                    if (sorting == "phonenumber") { query = query.OrderBy(e => e.PhoneNumber); }
                    if (sorting == "position") { query = query.OrderBy(e => e.GetRole().Name); }
                    if (sorting == "inn") { query = query.OrderBy(e => e.INN); }
                    if (sorting == "login") { query = query.OrderBy(e => e.Login); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(c => c.ID); }
                    if (sorting == "fio") { query = query.OrderByDescending(c => c.FIO); }
                    if (sorting == "passport") { query = query.OrderByDescending(c => c.Passport); }
                    if (sorting == "phonenumber") { query = query.OrderByDescending(c => c.PhoneNumber); }
                    if (sorting == "position") { query = query.OrderByDescending(e => e.GetRole().Name); }
                    if (sorting == "inn") { query = query.OrderByDescending(e => e.INN); }
                    if (sorting == "login") { query = query.OrderByDescending(e => e.Login); }
                }

                genCount = query.Count();

                query = query.Skip((page - 1) * count).Take(count);

                foreach (var employee in query)
                {
                    employees.Add(employee);
                }
                return employees;
            }
        }
        public static ObservableCollection<EmployeeItem> GetEmployeesForTable(bool showNotDeleted, bool showDeleted, int id, string fio,
           string passport, string phoneNumber, string position, string inn, string login,
           string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            ObservableCollection<EmployeeItem> tableEmployees = new ObservableCollection<EmployeeItem>();
            List<Employee> employees = GetEmployees(showNotDeleted, showDeleted, id, fio, passport, phoneNumber, position, inn, login,
                sorting, ascOrDesc, count, page, ref genCount);

            var appDir = System.AppDomain.CurrentDomain.BaseDirectory.Remove(AppDomain.CurrentDomain.BaseDirectory.Length - 11, 10);

            foreach (Employee employee in employees)
            {
                string imgUri = System.IO.Path.Combine(appDir, employee.Image);
                tableEmployees.Add(new EmployeeItem 
                {
                    ID = employee.ID,
                    FIO = employee.FIO,
                    Passport = employee.Passport,
                    PhoneNumber = employee.PhoneNumber,
                    Position = employee.GetRole().Name,
                    INN = employee.INN,
                    Login = employee.Login,
                    Image = imgUri
                });
            }
            return tableEmployees;
        }

        public static IEnumerable<int> GetIdsForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var employee in db.Employees)
                {
                    yield return employee.ID;
                }
            }
        }
        public static Employee GetEmployeeByID(int id)
        {
            using (AppContext db = new AppContext())
            {
                return db.Employees.Where(c => c.ID == id).FirstOrDefault();
            }
        }
    }
    public class EmployeeItem
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string INN { get; set; }
        public string Login { get; set; }
        public string Image { get; set; }
    }
}
