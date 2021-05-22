using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class Action
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Actions
    {
        public static List<Action> AllActions;
        public static List<Action> GetActions()
        {
            if (AllActions == null)
            {
                using (AppContext db = new AppContext())
                {
                    AllActions = db.Actions.ToList();
                }
            }
            return AllActions;
        }
    }
}
