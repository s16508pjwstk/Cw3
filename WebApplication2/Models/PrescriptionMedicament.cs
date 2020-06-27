using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class PrescriptionMedicament
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPrescriptionMedicament { get; set; }

        public int Dose { get; set; }

        public string Details { get; set; }

        [ForeignKey("Prescription")]
        public int? IdPrescription { get; set; }

        public virtual Prescription Prescription { get; set; }

        [ForeignKey("Medicament")]
        public int? IdMedicament { get; set; }

        public virtual Medicament Medicament { get; set; }
    }
}
