using System.Collections.Generic;
using PharmacyApp.Domain.Entities;

namespace PharmacyApp.Domain.Interfaces
{
    public interface ISaleRepository
    {
        void Add(Sale sale);
        Sale? GetById(string saleId);
        IEnumerable<Sale> GetAll();
    }
}
