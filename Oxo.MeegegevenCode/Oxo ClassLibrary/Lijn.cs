using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Oxo
{
    public class Lijn 
    {
        private readonly List<Veld> _velden = new List<Veld>();
        public ReadOnlyCollection<Veld> Velden
        {
            get { return new ReadOnlyCollection<Veld>(_velden); }
        }

        public Lijn(Veld veld1, Veld veld2, Veld veld3)
        {
            if (veld1 != null && veld2 != null && veld3 != null)
            {
                _velden.Add(veld1); _velden.Add(veld2); _velden.Add(veld3);
            }
            else { throw new ArgumentNullException($"Parameters {nameof(veld1)}, {nameof(veld2)} en {nameof(veld3)} mogen niet null zijn."); }
        }

        public string Naam { get; set; }

        public int AantalVelden(Speler speler)
        {
            int aantalVelden = 0;
            foreach (Veld v in _velden) if (v.Speler == speler) aantalVelden++;
            return aantalVelden;
            //of: return _velden.Count(veld => veld.Speler == speler);
        }

        public int AantalVrijeVelden()
        {
            int aantalVrijeVelden = 0;
            foreach (Veld veld in Velden) if (veld.IsVrij()) aantalVrijeVelden += 1;
            return aantalVrijeVelden;
        }
        public bool IsVol()
        {
            return (AantalVrijeVelden() == 0);
        }

        public bool IsReeksMogelijk()
        {
            return (IsReeksMogelijk(Speler.O) || IsReeksMogelijk(Speler.X));
        }
        public bool IsReeksMogelijk(Speler speler)
        {
            return _velden.TrueForAll(veld => veld.IsVrij() || veld.Speler == speler);
        }

        public bool IsReeks()
        {
            return (IsReeks(Speler.O) || IsReeks(Speler.X));
        }
        public bool IsReeks(Speler speler)
        {
            return _velden.TrueForAll(veld => veld.Speler == speler);
        }

        public bool Contains(Veld veld)
        {
            return _velden.Exists(v => v == veld);
        }
    }
}