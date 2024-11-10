using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky_Pet_Clinic2.Models
{
   public class Vaccination
    {
        public int VaccinationID { get; set; }
        public string VaccinationType { get; set; }
        public DateTime NextVisit { get; set; }
        public string NextVisitType { get; set; }

        public DateTime Date { get; set; }
        public string Vet { get; set; }
        public bool Called { get; set; } // New property



        // Foreign key
        public int PetId { get; set; }

        // Navigation property
        public Pets Pet { get; set; }

    }
}
