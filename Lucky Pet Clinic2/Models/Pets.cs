using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky_Pet_Clinic2.Models
{
    public class Pets
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string Species { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public int AgeDays { get; set; }

        public void CalculateAge()
        {
            if (DateOfBirth.HasValue)
            {
                var today = DateTime.Today;
                var ageYears = today.Year - DateOfBirth.Value.Year;
                var ageMonths = today.Month - DateOfBirth.Value.Month;
                var ageDays = today.Day - DateOfBirth.Value.Day;

                // Adjust for negative days
                if (ageDays < 0)
                {
                    ageMonths--;
                    ageDays += DateTime.DaysInMonth(today.Year, today.Month == 1 ? 12 : today.Month - 1);
                }

                // Adjust for negative months
                if (ageMonths < 0)
                {
                    ageYears--;
                    ageMonths += 12;
                }

                AgeYears = ageYears;
                AgeMonths = ageMonths;
                AgeDays = ageDays;
            }
            else
            {
                AgeYears = 0;
                AgeMonths = 0;
                AgeDays = 0;
            }
        }

        //public bool Sterilized { get; set; }

        // Foreign key
        public int ClientId { get; set; }

        // Navigation property
        public Clients Client { get; set; }



        public ICollection<Examination> Examinations { get; set; } = new List<Examination>();
        public ICollection<Surgery> Surgeries { get; set; } = new List<Surgery>();
        public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
    }
}
