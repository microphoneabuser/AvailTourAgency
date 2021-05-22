using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public class AddServicesInSales
    {
        public int ID { get; set; }
        public int SaleID { get; set; }
        public int AddServiceID { get; set; }
        public int? Quantity { get; set; }
    }
}
