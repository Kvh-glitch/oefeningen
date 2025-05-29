using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D15.D15stellingverhuur
{
    internal class StellingVerhuring
    {
        public DateTime StartVerhuur { get; set; }
        public DateTime EindVerhuur { get; set; }
        public int AantalUurOpbouw { get; set; } = 8;
        public int AantalUurAfbraak { get; set; } = 4;
        public Levering Levering { get; set; }
        public StellingVerhuring(DateTime startVerhuur, DateTime eindVerhuur)
        {
            StartVerhuur = startVerhuur;
            EindVerhuur = eindVerhuur;
        }

        public Periode NettoVerhuurPeriode()
        {
            DateTime start = StartVerhuur.AddHours(AantalUurOpbouw);
            DateTime einde = EindVerhuur.AddHours(-AantalUurAfbraak);
            return new Periode(start, einde);
        }
        public decimal Prijs()
        {

            int nettoUren = NettoVerhuurPeriode().AantalUur();

            decimal basisPrijs = (nettoUren * 5) +
                                 (AantalUurOpbouw * 90) +
                                 (AantalUurAfbraak * 60);
            int leveringsKost = 0;
            if (Levering != null)
            { leveringsKost = Levering.AfstandInKm * 10; }
            return basisPrijs + leveringsKost;
        }
    }
}
