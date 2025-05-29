using System;
using System.Collections.Generic;

namespace Oxo.ConsoleDemo
{
    class ComputerSpelerScore : IComparable<ComputerSpelerScore>, IComparable
    {
        public ComputerSpelerScore(ComputerSpeler cs) { Speler = cs; }
        public ComputerSpeler Speler { get; }

        public List<Duel> Duels { get; set; } = new List<Duel>();

        public Duel this[ComputerSpeler cts]
        {
            get
            {
                foreach (Duel d in Duels)
                    if (Object.ReferenceEquals(d.Tegenspeler, cts) ||
                        Object.ReferenceEquals(d.Speler, cts)) return d;
                return null;
            }
        }

        public int AantalDuelsGewonnen()
        {
            int aantal = 0;
            foreach (Duel d in Duels)
            {
                if (object.ReferenceEquals(d.Speler, Speler) &&
                    d.AantalOverwinningenSpeler > d.AantalOverwinningenTegenspeler ||
                    object.ReferenceEquals(d.Tegenspeler, Speler) &&
                    d.AantalOverwinningenTegenspeler > d.AantalOverwinningenSpeler)
                {
                    aantal++;
                }
            }
            return aantal;
        }
        public int AantalDuelsVerloren()
        {
            int aantal = 0;
            foreach (Duel d in Duels)
            {
                if (object.ReferenceEquals(d.Speler, Speler) &&
                    d.AantalOverwinningenSpeler < d.AantalOverwinningenTegenspeler ||
                    object.ReferenceEquals(d.Tegenspeler, Speler) &&
                    d.AantalOverwinningenTegenspeler < d.AantalOverwinningenSpeler)
                {
                    aantal++;
                }
            }
            return aantal;
        }
        public int AantalDuelsGelijkgespeeld()
        {
            int aantal = 0;
            foreach (Duel d in Duels)
            {
                if (d.AantalOverwinningenSpeler == d.AantalOverwinningenTegenspeler)
                {
                    aantal++;
                }
            }
            return aantal;
        }

        public int AantalSpellenGewonnen()
        {
            int totaal = 0;
            foreach (Duel d in Duels)
            {
                if (object.ReferenceEquals(d.Speler, this.Speler)) totaal += d.AantalOverwinningenSpeler;
                else if (object.ReferenceEquals(d.Tegenspeler, this.Speler)) totaal += d.AantalOverwinningenTegenspeler;
            }
            return totaal;
        }
        public int AantalSpellenGelijkspel()
        {
            int totaal = 0;
            foreach (Duel d in Duels) totaal += d.AantalGelijkspelen;
            return totaal;
        }
        public int AantalSpellenVerloren()
        {
            int totaal = 0;
            foreach (Duel d in Duels)
            {
                if (object.ReferenceEquals(d.Speler, this.Speler)) totaal += d.AantalVerliezenSpeler;
                else if (object.ReferenceEquals(d.Tegenspeler, this.Speler)) totaal += d.AantalVerliezenTegenspeler;
            }
            return totaal;
        }

        int IComparable.CompareTo(object other)
        {
            if (other == null) return 1;
            else if (this is ComputerSpelerScore) return this.CompareTo(other as ComputerSpelerScore);
            else throw new ArgumentException("Een ongeledig argument was voorzien.  " +
                             $"Een argument van type {nameof(ComputerSpelerScore)} wordt verwacht.");
        }
        public int CompareTo(ComputerSpelerScore other)
        {
            if (other != null)
            {
                int compareTo = -this.AantalDuelsGewonnen().CompareTo(other.AantalDuelsGewonnen());
                if (compareTo == 0) compareTo = -this.AantalDuelsGelijkgespeeld().CompareTo(other.AantalDuelsGelijkgespeeld());
                if (compareTo == 0) compareTo = -this.AantalSpellenGewonnen().CompareTo(other.AantalSpellenGewonnen());
                if (compareTo == 0) compareTo = -this.AantalSpellenGelijkspel().CompareTo(other.AantalSpellenGelijkspel());
                return compareTo;
            }
            else return 1;
        }
    }
}