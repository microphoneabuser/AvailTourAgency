using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public abstract class Person
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
        public bool Deleted { get; set; }

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
    }
}
