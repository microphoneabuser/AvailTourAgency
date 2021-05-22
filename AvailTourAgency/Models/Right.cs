using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class Right
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public int ActionID { get; set; }
        public DateTime? EndDate { get; set; }
        public static bool CheckAccess(string name)
        {
            Action action = Actions.AllActions.Where(a => a.Name == name).FirstOrDefault();
            if (action != null)
            {
                Right right = Rights.AllRights.Where(r => r.RoleID == User.GetUser().RoleID && r.ActionID == action.ID 
                                                && (r.EndDate == null || r.EndDate > DateTime.Now)).FirstOrDefault();
                if (right == null)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
    public class Rights
    {
        public static List<Right> AllRights;
        public static List<Right> GetRights()
        {
            if (AllRights == null)
            {
                using (AppContext db = new AppContext())
                {
                    AllRights = db.Rights.ToList();
                }
            }
            return AllRights;
        }
    }
}
