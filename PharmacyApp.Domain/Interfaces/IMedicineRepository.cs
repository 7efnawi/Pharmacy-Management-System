using System.Collections.Generic;
using PharmacyApp.Domain.Entities;

namespace PharmacyApp.Domain.Interfaces
{
    public interface IMedicineRepository
    {
        void Add(Medicine medicine);
        void Update(Medicine medicine);
        void Delete(string medicineId);
        Medicine? GetById(string medicineId);
        IEnumerable<Medicine> GetAll();
        IEnumerable<Medicine> Search(string query);
        IEnumerable<Medicine> GetLowStock();
        IEnumerable<Medicine> GetExpiringSoon(int withinDays = 30);
    }
}
