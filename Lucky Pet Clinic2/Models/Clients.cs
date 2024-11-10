using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky_Pet_Clinic2.Models
{
    public class Clients
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }

        public ICollection<Pets> Pets { get; set; } = new List<Pets>();
    }


}
