using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    
    public class Roles//Singleton
    {
        public static List<Role> AllRoles;
        public static List<Role> GetRoles()
        {
            if (AllRoles == null)
            {
                using (AppContext db = new AppContext())
                {
                    AllRoles = db.Roles.ToList();
                }
            }
            return AllRoles;
        }
        public static IEnumerable<string> GetRolesForFilters()
        {
            foreach (var role in AllRoles)
            {
                yield return role.Name;
            }  
        }
        public static Role GetRoleByID(int id)
        {
            return AllRoles.Where(r => r.ID == id).FirstOrDefault();
        }
        public static Role GetRoleByName(string name)
        {
            return AllRoles.Where(r => r.Name == name).FirstOrDefault();
        }
    }
}
