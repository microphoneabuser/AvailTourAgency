using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public static class Clients
    {
        public static List<Client> GetClients(bool showNotDeleted, bool showDeleted, int id, string fio,
           string passport, string phoneNumber, string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            List<Client> clients = new List<Client>();
            using (AppContext db = new AppContext())
            {
                var query = (from c in db.Clients
                            select c).AsEnumerable();

                if (showNotDeleted && !showDeleted)
                {
                    query = query.Where(c => !c.Deleted);
                }
                if (!showNotDeleted && showDeleted)
                {
                    query = query.Where(c => c.Deleted);
                }

                if (id != 0)
                {
                    query = query.Where(c => c.ID == id);
                }
                if (fio != null && fio != "")
                {
                    query = query.Where(c => c.FIO.ToLower().Contains(Validator.ValidateString(fio)));
                }
                if (passport != null && passport != "____ ______" && passport != "")
                {
                    query = query.Where(c => c.Passport == passport);
                }
                if (phoneNumber != null && phoneNumber != "+7(___)___-__-__" && phoneNumber != "")
                {
                    query = query.Where(c => c.PhoneNumber == phoneNumber);
                }

                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(c => c.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(c => c.ID); }
                    if (sorting == "fio") { query = query.OrderBy(c => c.FIO); }
                    if (sorting == "passport") { query = query.OrderBy(c => c.Passport); }
                    if (sorting == "phonenumber") { query = query.OrderBy(c => c.PhoneNumber); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(c => c.ID); }
                    if (sorting == "fio") { query = query.OrderByDescending(c => c.FIO); }
                    if (sorting == "passport") { query = query.OrderByDescending(c => c.Passport); }
                    if (sorting == "phonenumber") { query = query.OrderByDescending(c => c.PhoneNumber); }
                }

                genCount = query.Count();

                query = query.Skip((page - 1) * count).Take(count);

                foreach (var client in query)
                {
                    clients.Add(client);
                }
                return clients;
            }
        }
        public static ObservableCollection<ClientItem> GetClientsForTable(bool showNotDeleted, bool showDeleted, int id, string fio,
           string passport, string phoneNumber, string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            ObservableCollection<ClientItem> tableClients = new ObservableCollection<ClientItem>();
            List<Client> clients = GetClients(showNotDeleted, showDeleted, id, fio, passport, 
                phoneNumber, sorting, ascOrDesc, count, page, ref genCount);
            foreach (Client client in clients)
            {
                tableClients.Add(new ClientItem
                {
                    ID = client.ID, 
                    FIO = client.FIO,
                    Passport = client.Passport,
                    PhoneNumber = client.PhoneNumber
                });
            }
            return tableClients;
        }

        public static IEnumerable<int> GetIdsForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var client in db.Clients)
                {
                    yield return client.ID;
                }
            }
        }

        public static Client GetClientByID(int id)
        {
            using (AppContext db = new AppContext())
            {
                return db.Clients.Where(c => c.ID == id).FirstOrDefault();
            }
        }
    }

    public class ClientItem
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
    }

}
