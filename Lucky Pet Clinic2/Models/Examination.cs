using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky_Pet_Clinic2.Models
{
   public class Examination
    {
        public int Id { get; set; }
        //public int PetId { get; set; }
        public string Temperature { get; set; }
        public string CaseHistory { get; set; }
        public string VitalSigns { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string XrayTests { get; set; }
        public DateTime NextVisit { get; set; }
        public DateTime Date { get; set; }
        public string Vet { get; set; }
        public bool Called { get; set; } // New property
        public string NextVisitType { get; set; }
        public string Weight { get; set; }


        // Foreign key
        public int PetId { get; set; }

        // Navigation property
        public Pets Pet { get; set; }
    }
}
