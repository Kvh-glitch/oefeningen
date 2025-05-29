using System;

namespace Oxo.ConsoleDemo
{
    class Duel
    {
        public Duel(ComputerSpeler van, ComputerSpeler tegen)
        {
            Speler = van;
            Tegenspeler = tegen;
        }
        public ComputerSpeler Speler { get; }
        public ComputerSpeler Tegenspeler { get; }

        public ComputerSpeler Winnaar()
        {
            ComputerSpeler s = null;
            if (AantalOverwinningenSpeler > AantalOverwinningenTegenspeler) s = Speler;
            else if (AantalOverwinningenSpeler < AantalOverwinningenTegenspeler) s = Tegenspeler;
            return s;
        }

        public void Speelt(Status status)
        {
            _aantalGespeeld++;
            if (status == Status.Gelijkspel) _aantalGelijkspelen++;
            else if (status == Status.SpelerOGewonnen)
            {
                if (Speler.Speler == Oxo.Speler.O)
                {
                    _aantalOverwinningenSpeler++;
                    _aantalVerliezenTegenspeler++;
                }
                else //if (Speler.Speler == Oxo.Speler.X)
                {
                    _aantalOverwinningenTegenspeler++;
                    _aantalVerliezenSpeler++;
                }
            }
            else if (status == Status.SpelerXGewonnen)
            {
                if (Speler.Speler == Oxo.Speler.X)
                {
                    _aantalOverwinningenSpeler++;
                    _aantalVerliezenTegenspeler++;
                }
                else //if (Speler.Speler == Oxo.Speler.O)
                {
                    _aantalOverwinningenTegenspeler++;
                    _aantalVerliezenSpeler++;
                }
            }
            else throw new InvalidOperationException("Het spel is nog bezig, deze method zou enkel mogen aangeroepen worden als het spel beëindigd is.");
        }

        protected int _aantalGespeeld;
        public int AantalGespeeld => _aantalGespeeld;

        protected int _aantalOverwinningenSpeler;
        public int AantalOverwinningenSpeler => _aantalOverwinningenSpeler;
        protected int _aantalOverwinningenTegenspeler;
        public int AantalOverwinningenTegenspeler => _aantalOverwinningenTegenspeler;

        protected int _aantalVerliezenSpeler;
        public int AantalVerliezenSpeler => _aantalVerliezenSpeler;
        protected int _aantalVerliezenTegenspeler;
        public int AantalVerliezenTegenspeler => _aantalVerliezenTegenspeler;

        protected int _aantalGelijkspelen;
        public int AantalGelijkspelen => _aantalGelijkspelen;

        public double GelijkspelenVsGespeeld() => AantalGespeeld != 0.0 ? (double)AantalGelijkspelen / AantalGespeeld : AantalGespeeld;
        public int Overwinningen() => AantalOverwinningenSpeler + AantalOverwinningenTegenspeler;
        public double OverwinningenVsGespeeld() => AantalGespeeld != 0.0 ? (double)(AantalOverwinningenSpeler + AantalOverwinningenTegenspeler) / AantalGespeeld : AantalGespeeld;
        public double OverwinningenSpelerVsGespeeld() => AantalGespeeld != 0.0 ? (double)AantalOverwinningenSpeler / AantalGespeeld : AantalGespeeld;
        public double OverwinningenTegenspelerVsGespeeld() => AantalGespeeld != 0.0 ? (double)AantalOverwinningenTegenspeler / AantalGespeeld : AantalGespeeld;
        public double OverwinningenSpelerVsOverwinningen() => Overwinningen() != 0 ? (double)AantalOverwinningenSpeler / Overwinningen() : Overwinningen();
        public double OverwinningenTegenspelerVsOverwinningen() => Overwinningen() != 0 ? (double)AantalOverwinningenTegenspeler / Overwinningen() : Overwinningen();
    }
}