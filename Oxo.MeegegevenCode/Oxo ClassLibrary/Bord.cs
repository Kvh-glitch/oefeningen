using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Oxo
{
    public class Bord : ReadOnlyObservableCollection<Veld>
    {
        public Bord() : base(new ObservableCollection<Veld>())
        {
            this.Items.Add(new Veld(1, 1));
            this.Items.Add(new Veld(1, 2));
            this.Items.Add(new Veld(1, 3));
            this.Items.Add(new Veld(2, 1));
            this.Items.Add(new Veld(2, 2));
            this.Items.Add(new Veld(2, 3));
            this.Items.Add(new Veld(3, 1));
            this.Items.Add(new Veld(3, 2));
            this.Items.Add(new Veld(3, 3));

            _lijnen.Add(new Lijn(this[0], this[1], this[2]) { Naam = "Rij 1" });
            _lijnen.Add(new Lijn(this[3], this[4], this[5]) { Naam = "Rij 2" });
            _lijnen.Add(new Lijn(this[6], this[7], this[8]) { Naam = "Rij 3" });
            _lijnen.Add(new Lijn(this[0], this[3], this[6]) { Naam = "Kolom 1" });
            _lijnen.Add(new Lijn(this[1], this[4], this[7]) { Naam = "Kolom 2" });
            _lijnen.Add(new Lijn(this[2], this[5], this[8]) { Naam = "Kolom 3" });
            _lijnen.Add(new Lijn(this[0], this[4], this[8]) { Naam = "Linksboven-rechtsonder" });
            _lijnen.Add(new Lijn(this[6], this[4], this[2]) { Naam = "Linksonder-rechtsboven" });
        }

        public Veld this[int rijIndex, int kolomIndex]
        {
            get
            {
                if (rijIndex >= 0 && rijIndex <= 2 && kolomIndex >= 0 && kolomIndex <= 2)
                {
                    int index = rijIndex * 3 + kolomIndex;
                    return this[index];
                }
                else { throw new ArgumentOutOfRangeException(); } // Index was out of range. Must be non - negative and less than the size of the collection.
            }
        }

        public List<Veld> LegeVelden()
        {
            List<Veld> legeVelden = new List<Veld>();
            foreach (Veld veld in this) if (veld.IsVrij()) legeVelden.Add(veld);
            return legeVelden;
        }

        private readonly ObservableCollection<Lijn> _lijnen = new ObservableCollection<Lijn>();
        public ReadOnlyObservableCollection<Lijn> Lijnen
        {
            get { return new ReadOnlyObservableCollection<Lijn>(_lijnen); }
        }
    }
}