using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Cryptography;

namespace AvailTourAgency.Models
{
    class User//Singleton
    {
        static private Employee currentUser;
        public User() { }
        public static Employee GetUser()
        {
            if (currentUser == null)
            {
                currentUser = new Employee();
            }
            return currentUser;
        }
        public static Result Login(string login, string password)
        {
            Result result = new Result();
            using (AppContext db = new AppContext())
            {
                password = ComputeHash(password, ConfigurationManager.AppSettings["HashSalt"]);

                currentUser = db.Employees.Where(e => e.Login == login && e.Pass == password).FirstOrDefault();
                if (currentUser != null)
                {
                    result.Success = true;
                    //заполняем неизменяемые на протяжении действия программы сущности
                    Roles.GetRoles();
                    Actions.GetActions();
                    Rights.GetRights();
                }
                else
                {
                    result.Success = false;
                    result.Description = "failed";
                }
                return result;
            }
        }
        public static string ComputeHash(string input, string salt)
        {
            var md5 = MD5.Create();

            Byte[] inputBytes = Encoding.UTF8.GetBytes(input + salt);

            Byte[] hashedBytes = md5.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }
    }
}
