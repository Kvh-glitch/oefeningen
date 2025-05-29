using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Oxo
{
    public class Spel 
    {
        private Bord _bord;
        public Bord Bord { get { return _bord; } }

        public event EventHandler<VolgendeSpelerAanZetEventArgs> VolgendeSpelerAanZet;
        protected virtual void OnRaiseVolgendeSpelerAanZetEvent(VolgendeSpelerAanZetEventArgs e)
        {
            EventHandler<VolgendeSpelerAanZetEventArgs> handler = VolgendeSpelerAanZet;
            if (handler != null) { handler(this, e); }
        }
        public event EventHandler<SpelBeëindigdEventArgs> SpelBeëindigd;
        protected virtual void OnRaiseSpelBeëindigdEvent(SpelBeëindigdEventArgs e)
        {
            EventHandler<SpelBeëindigdEventArgs> handler = SpelBeëindigd;
            if (handler != null) { handler(this, e); }
        }

        public Speler TeSpelenSpeler { get; private set; } = Speler.O;
        public Speler StartendeSpeler { get; }
        public Speler LaatstGespeeldeSpeler { get; private set; }

        public Spel(Speler startendeSpeler = Speler.O) : this(new Bord(), startendeSpeler) { }
        public Spel(Bord bord, Speler startendeSpeler = Speler.O)
        {
            if (bord != null)
            {
                StartendeSpeler = startendeSpeler;
                _bord = bord;
                Start();
            }
            else throw new ArgumentNullException();
        }

        protected bool _gestart = false;
        public void Start()
        {
            if (!_gestart)
            {
                TeSpelenSpeler = StartendeSpeler;
                VolgendeSpelerAanZetEventArgs e = new VolgendeSpelerAanZetEventArgs(TeSpelenSpeler);
                OnRaiseVolgendeSpelerAanZetEvent(e);
            }
            else throw new InvalidOperationException("Het spel is reeds gestart.");
        }

        public void Speel(Veld veld)
        {
            Status oudeStatus = Status();
            if (oudeStatus == Oxo.Status.Bezig)
            {
                if (!Bord.Contains(veld)) throw new ArgumentOutOfRangeException(nameof(veld), "Het doorgegeven veld corespondeert niet (is niet logisch gelijk) aan een veld uit het bord.");
                if (Bord.Count((v) => object.ReferenceEquals(v, veld)) <= 0) throw new InvalidOperationException($"Een wordt een fysiek verschillend veld bespeeld.");

                if (veld.IsVrij())
                {
                    veld.Speler = TeSpelenSpeler;
                    LaatstGespeeldeSpeler = TeSpelenSpeler;
                    if (TeSpelenSpeler == Speler.O) TeSpelenSpeler = Speler.X;
                    else if (TeSpelenSpeler == Speler.X) TeSpelenSpeler = Speler.O;

                    Status nieuweStatus = Status();
                    if (nieuweStatus == Oxo.Status.SpelerOGewonnen || nieuweStatus == Oxo.Status.SpelerXGewonnen)
                    {
                        SpelBeëindigdEventArgs e = new SpelBeëindigdEventArgs(gewonnenSpeler:LaatstGespeeldeSpeler);
                        OnRaiseSpelBeëindigdEvent(e);
                    }
                    else if (nieuweStatus == Oxo.Status.Gelijkspel)
                    {
                        SpelBeëindigdEventArgs e = new SpelBeëindigdEventArgs();
                        OnRaiseSpelBeëindigdEvent(e);
                    }
                    else if (nieuweStatus == Oxo.Status.Bezig)
                    {
                        VolgendeSpelerAanZetEventArgs e = new VolgendeSpelerAanZetEventArgs(TeSpelenSpeler);
                        OnRaiseVolgendeSpelerAanZetEvent(e);
                    }
                }
                else throw new InvalidOperationException($"Het bespeelde veld ({veld.ToString()}) is niet meer vrij.");
            }
            else throw new InvalidOperationException("Het spel is reeds beëindigd.");
        }
        public void Speel(int rij, int kolom)
        {
            if (rij >= 1 && rij <= 3 && kolom >= 1 && kolom <= 3)
            {
                Veld veld = Bord[rij - 1, kolom - 1];
                Speel(veld);
            }
            else throw new ArgumentOutOfRangeException($"Parameters {nameof(rij)} en {nameof(kolom)} mogen enkel een waarde van 1 tot en met 3 aannemen.");
        }

        public Status Status()
        {
            if (IsGelijkspel()) return Oxo.Status.Gelijkspel;
            else if (IsSpelerGewonnen(Speler.O)) return Oxo.Status.SpelerOGewonnen;
            else if (IsSpelerGewonnen(Speler.X)) return Oxo.Status.SpelerXGewonnen;
            else return Oxo.Status.Bezig;
        }

        public bool IsGelijkspel()
        {
            if (!IsSpelerGewonnen())
            {
                int aantalReeksenNogMogelijk = Bord.Lijnen.Count(lijn => lijn.IsReeksMogelijk());
                if (aantalReeksenNogMogelijk == 0)
                {
                    //Indien geen reeksen meer gemaakt kunnen worden, dan is het gelijkspel:
                    return true;
                }
                else
                {
                    List<Veld> legeVelden = Bord.LegeVelden();
                    int aantalLegeVelden = legeVelden.Count;
                    Lijn mogelijkeTeMakenReeks = Bord.Lijnen.First(lijn => lijn.IsReeksMogelijk());

                    if (aantalReeksenNogMogelijk == 1)
                    {
                        if (aantalLegeVelden >= 2)
                        {
                            //Indien er in totaal nog 2 of meer vrije plaasten zijn, en dat op 1 lijn, 
                            //zouden beide spelers elkaar hier op gaan blokkeren, en is het maw gelijkspel:
                            return legeVelden.TrueForAll(veld => mogelijkeTeMakenReeks.Contains(veld));
                        }
                        else //if (aantalLegeVelden == 1)
                        {
                            Veld veldUitMogelijkeTeMakenReeks = mogelijkeTeMakenReeks.Velden.First(Veld => !Veld.IsVrij());
                            return veldUitMogelijkeTeMakenReeks.Speler != TeSpelenSpeler;
                        }
                    }
                    else //if (aantalReeksenNogMogelijk > 1)
                    {
                        if (aantalLegeVelden == 1)
                        {
                            IEnumerable<Lijn> mogelijkeTeMakenReeksen = Bord.Lijnen.Where(lijn => lijn.IsReeksMogelijk());
                            bool kanDoorZelfdeSpeler = false;
                            foreach (Lijn reeks in mogelijkeTeMakenReeksen)
                            {
                                Veld veldUitMogelijkeTeMakenReeks = mogelijkeTeMakenReeks.Velden.First(Veld => !Veld.IsVrij());
                                kanDoorZelfdeSpeler = kanDoorZelfdeSpeler && (veldUitMogelijkeTeMakenReeks.Speler == TeSpelenSpeler);
                            }
                            return kanDoorZelfdeSpeler;
                        }
                        else //if (aantalLegeVelden > 1)
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }

        }

        public bool IsSpelerGewonnen()
        {
            return IsSpelerGewonnen(Speler.O) || IsSpelerGewonnen(Speler.X);
        }
        public bool IsSpelerGewonnen(Speler speler)
        {
            if (Bord.Lijnen.Count(lijn => lijn.IsReeks(speler)) >= 1) return true;
            return false;
        }
    }

    public class VolgendeSpelerAanZetEventArgs : EventArgs
    {
        public VolgendeSpelerAanZetEventArgs(Speler spelerAanZet)
        {
            SpelerAanZet = spelerAanZet;
        }
        public Speler SpelerAanZet { get; }
    }
    public class SpelBeëindigdEventArgs : EventArgs
    {
        public SpelBeëindigdEventArgs()
        {
            Status = Status.Gelijkspel;
        }
        public SpelBeëindigdEventArgs(Speler gewonnenSpeler)
        {
            if (gewonnenSpeler == Speler.O) Status = Status.SpelerOGewonnen;
            else if (gewonnenSpeler == Speler.X) Status = Status.SpelerXGewonnen;
        }

        public Status Status { get; }
    }
}