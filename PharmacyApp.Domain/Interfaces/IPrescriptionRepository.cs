using System.Collections.Generic;
using PharmacyApp.Domain.Entities;

namespace PharmacyApp.Domain.Interfaces
{
    public interface IPrescriptionRepository
    {
        void Add(Prescription prescription);
        Prescription? GetById(string prescriptionId);
        IEnumerable<Prescription> GetAll();
    }
}
