using System.Collections.Generic;
using System.Linq;
using PharmacyApp.Domain.Entities;
using PharmacyApp.Domain.Interfaces;

namespace PharmacyApp.Domain.Repositories
{
    public class InMemoryPrescriptionRepository : IPrescriptionRepository
    {
        private readonly List<Prescription> _prescriptions = new();
        private readonly object _lock = new();

        public void Add(Prescription prescription)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(prescription.PrescriptionId))
                {
                    prescription.PrescriptionId = $"RX-{_prescriptions.Count + 1:D4}";
                }
                _prescriptions.Add(prescription);
            }
        }

        public Prescription? GetById(string prescriptionId)
        {
            lock (_lock)
            {
                return _prescriptions.FirstOrDefault(p => p.PrescriptionId == prescriptionId);
            }
        }

        public IEnumerable<Prescription> GetAll()
        {
            lock (_lock)
            {
                return _prescriptions.ToList();
            }
        }
    }
}
