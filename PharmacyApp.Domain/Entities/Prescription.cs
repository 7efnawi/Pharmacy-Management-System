using System;
using System.Collections.Generic;

namespace PharmacyApp.Domain.Entities
{
    public class Prescription
    {
        public string PrescriptionId { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Today;
        public List<SaleItem> Items { get; set; } = new();
        public string Notes { get; set; } = string.Empty;
    }
}
