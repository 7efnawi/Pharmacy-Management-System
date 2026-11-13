using System.Collections.Generic;
using System.Linq;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Domain.Interfaces;

namespace PharmacyApp.Domain.Repositories
{
    public class InMemorySaleRepository : ISaleRepository
    {
        private readonly List<Sale> _sales = new();
        private readonly object _lock = new();

        public void Add(Sale sale)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(sale.SaleId))
                {
                    sale.SaleId = $"SAL-{_sales.Count + 1:D4}";
                }
                _sales.Add(sale);
            }
        }

        public Sale? GetById(string saleId)
        {
            lock (_lock)
            {
                return _sales.FirstOrDefault(s => s.SaleId == saleId);
            }
        }

        public IEnumerable<Sale> GetAll()
        {
            lock (_lock)
            {
                return _sales.ToList();
            }
        }
    }
}
