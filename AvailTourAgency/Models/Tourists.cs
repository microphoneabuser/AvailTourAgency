using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailTourAgency.Models
{
    public static class Tourists
    {
        public static List<Tourist> GetTourists(bool showDeleted, int saleID, string sorting, string ascOrDesc)
        {
            List<Tourist> tourists = new List<Tourist>();
            using (AppContext db = new AppContext())
            {
                var query = (from t in db.Tourists
                             join tis in db.TouristsInSales on t.ID equals tis.TouristID
                             into tTisTemp
                             from tTis in tTisTemp.DefaultIfEmpty()
                             join s in db.Sales on tTis.SaleID equals s.ID
                             into sTemp
                             from s in sTemp.DefaultIfEmpty()
                             select new
                             {
                                 ID = t.ID,
                                 FIO = t.FIO,
                                 PhoneNumber = t.PhoneNumber,
                                 DateOfBirth = t.DateOfBirth,
                                 DocumentType = t.DocumentType,
                                 DocumentData = t.DocumentData,
                                 Deleted = t.Deleted,
                                 SaleID = (s == null ? 0 : s.ID)
                             }).AsEnumerable();

                if (!showDeleted)
                {
                    query = query.Where(t => !t.Deleted);
                }

                if (saleID != 0)
                {
                    query = query.Where(s => s.SaleID == saleID);
                }

                //sorting
                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(hr => hr.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(s => s.ID); }
                    if (sorting == "fio") { query = query.OrderBy(s => s.FIO); }
                    if (sorting == "phonenumber") { query = query.OrderBy(s => s.PhoneNumber); }
                    if (sorting == "dateofbitrh") { query = query.OrderBy(s => s.DateOfBirth); }
                    if (sorting == "documenttype") { query = query.OrderBy(s => s.DocumentType); }
                    if (sorting == "documentdata") { query = query.OrderBy(s => s.DocumentData); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(s => s.ID); }
                    if (sorting == "fio") { query = query.OrderByDescending(s => s.FIO); }
                    if (sorting == "phonenumber") { query = query.OrderByDescending(s => s.PhoneNumber); }
                    if (sorting == "dateofbitrh") { query = query.OrderByDescending(s => s.DateOfBirth); }
                    if (sorting == "documenttype") { query = query.OrderByDescending(s => s.DocumentType); }
                    if (sorting == "documentdata") { query = query.OrderByDescending(s => s.DocumentData); }
                }

                foreach (var tourist in query)
                {
                    tourists.Add(new Tourist
                    {
                        ID = tourist.ID,
                        FIO = tourist.FIO,
                        PhoneNumber = tourist.PhoneNumber,
                        DateOfBirth = tourist.DateOfBirth,
                        DocumentType = tourist.DocumentType,
                        DocumentData = tourist.DocumentData,
                        Deleted = tourist.Deleted
                    });
                }
                return tourists;
            }
        }

        public static ObservableCollection<TouristItem> GetTouristsForTable(bool showDeleted, int saleID, string sorting, string ascOrDesc)
        {
            ObservableCollection<TouristItem> tableTourists = new ObservableCollection<TouristItem>();
            List<Tourist> tourists = GetTourists(showDeleted, saleID, sorting, ascOrDesc);

            Dictionary<int, string> DocumentTypes = new Dictionary<int, string>();
            string[] ft = ConfigurationManager.AppSettings["DocumentTypes"].Split(';');
            foreach (string t in ft)
            {
                DocumentTypes.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
            }

            foreach (Tourist tourist in tourists)
            {
                tableTourists.Add(new TouristItem
                {
                    ID = tourist.ID,
                    FIO = tourist.FIO,
                    PhoneNumber = tourist.PhoneNumber,
                    DateOfBirth = tourist.DateOfBirth.Value.ToShortDateString(),
                    DocumentType = DocumentTypes[tourist.DocumentType],
                    DocumentData = tourist.DocumentData
                });
            }
            return tableTourists;
        }






        public static List<Tourist> GetAllTourists(bool showNotDeleted, bool showDeleted, int id, string fio, 
            string phoneNumber, DateTime? dateOfBirth, string documentData, string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            List<Tourist> tourists = new List<Tourist>();
            using (AppContext db = new AppContext())
            {
                var query = (from c in db.Tourists
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
                if (phoneNumber != null && phoneNumber != "+7(___)___-__-__" && phoneNumber != "")
                {
                    query = query.Where(c => c.PhoneNumber == phoneNumber);
                }
                if (dateOfBirth != null)
                {
                    query = query.Where(c => c.DateOfBirth.Value.ToShortDateString() == dateOfBirth.Value.ToShortDateString());
                }
                if (documentData != null && documentData != "____ ______" && documentData != "")
                {
                    query = query.Where(c => c.DocumentData == documentData);
                }

                if (sorting == null || sorting == "")
                {
                    query = query.OrderByDescending(c => c.ID);
                }
                if (ascOrDesc == "asc")
                {
                    if (sorting == "id") { query = query.OrderBy(s => s.ID); }
                    if (sorting == "fio") { query = query.OrderBy(s => s.FIO); }
                    if (sorting == "phonenumber") { query = query.OrderBy(s => s.PhoneNumber); }
                    if (sorting == "dateofbitrh") { query = query.OrderBy(s => s.DateOfBirth); }
                    if (sorting == "documenttype") { query = query.OrderBy(s => s.DocumentType); }
                    if (sorting == "documentdata") { query = query.OrderBy(s => s.DocumentData); }
                }
                if (ascOrDesc == "desc")
                {
                    if (sorting == "id") { query = query.OrderByDescending(s => s.ID); }
                    if (sorting == "fio") { query = query.OrderByDescending(s => s.FIO); }
                    if (sorting == "phonenumber") { query = query.OrderByDescending(s => s.PhoneNumber); }
                    if (sorting == "dateofbitrh") { query = query.OrderByDescending(s => s.DateOfBirth); }
                    if (sorting == "documenttype") { query = query.OrderByDescending(s => s.DocumentType); }
                    if (sorting == "documentdata") { query = query.OrderByDescending(s => s.DocumentData); }
                }

                genCount = query.Count();

                query = query.Skip((page - 1) * count).Take(count);

                foreach (var tourist in query)
                {
                    tourists.Add(tourist);
                }
                return tourists;
            }
        }
        public static ObservableCollection<TouristItem> GetAllTouristsForTable(bool showNotDeleted, bool showDeleted, int id, string fio,
            string phoneNumber, DateTime? dateOfBirth, string documentData, string sorting, string ascOrDesc, int count, int page, ref int genCount)
        {
            ObservableCollection<TouristItem> tableTourists = new ObservableCollection<TouristItem>();
            List<Tourist> tourists = GetAllTourists(showNotDeleted, showDeleted, id, fio, phoneNumber,
                dateOfBirth, documentData, sorting, ascOrDesc, count, page, ref genCount);

            Dictionary<int, string> DocumentTypes = new Dictionary<int, string>();
            string[] ft = ConfigurationManager.AppSettings["DocumentTypes"].Split(';');
            foreach (string t in ft)
            {
                DocumentTypes.Add(int.Parse(t.Split(':')[0]), t.Split(':')[1]);
            }

            foreach (Tourist tourist in tourists)
            {
                tableTourists.Add(new TouristItem
                {
                    ID = tourist.ID,
                    FIO = tourist.FIO,
                    PhoneNumber = tourist.PhoneNumber,
                    DateOfBirth = tourist.DateOfBirth.Value.ToShortDateString(),
                    DocumentType = DocumentTypes[tourist.DocumentType],
                    DocumentData = tourist.DocumentData
                });
            }
            return tableTourists;
        }



        public static IEnumerable<int> GetIdsForFilters()
        {
            using (AppContext db = new AppContext())
            {
                foreach (var tourist in db.Tourists)
                {
                    yield return tourist.ID;
                }
            }
        }
        public static Tourist GetTouristByID(int id)
        {
            using (AppContext db = new AppContext())
            {
                return db.Tourists.Where(t => t.ID == id).FirstOrDefault();
            }
        }
    }
    public class TouristItem
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string DocumentType { get; set; }
        public string DocumentData { get; set; }
    }
}
