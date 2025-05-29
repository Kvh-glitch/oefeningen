using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D15.D15stellingverhuur
{
    internal class Periode
    {
        public DateTime Start { get; set; }
        public DateTime Einde { get; set; }
        public Periode(DateTime start, DateTime einde)
        {
            Start = start;
            Einde = einde;
        }
        public int AantalUur()
        {
            TimeSpan tijdsduur = Einde - Start;
            return (int)Math.Ceiling(tijdsduur.TotalHours);
        }
    }
}
