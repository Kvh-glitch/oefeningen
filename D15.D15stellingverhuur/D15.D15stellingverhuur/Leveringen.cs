using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D15.D15stellingverhuur
{
    internal class Levering
    {
        public int AfstandInKm { get; set; }
        public string Adres { get; set; }
        public Levering(string adres, int afstandinkm)
        {
            AfstandInKm = afstandinkm;
            Adres = adres;
        }
    }
}
