using System;
using System.ComponentModel;

namespace Oxo
{
    public class Veld : INotifyPropertyChanged, IEquatable<Veld>
    {
        #region "Velden"
        private readonly int _rij;
        private readonly int _kolom;
        private Speler? _speler;
        #endregion

        #region "Constructor"
        public Veld(int rij, int kolom)
        {
            if (rij >= 1 && rij <= 3 && kolom >= 1 && kolom <= 3)
            {
                _rij = rij;
                _kolom = kolom;
            }
            else throw new ArgumentOutOfRangeException($"Parameters {nameof(rij)} en {nameof(kolom)} mogen enkel een waarde van 1 tot en met 3 aannemen.");
        }
        #endregion

        #region "Properties"
        public int Rij { get { return _rij; } }
        public int Kolom { get { return _kolom; } }
        public Speler? Speler
        {
            get { return _speler; }
            set
            {
                if (value != _speler)
                {
                    _speler = value;
                    NotifyPropertyChanged(nameof(Speler));
                }
            }
        }
        #endregion

        #region "Overige queries"
        public bool IsVrij() { return !Speler.HasValue; }

        public override string ToString() { return $"[{Rij}/{Kolom}]"; }
        #endregion

        #region "INotifyPropertyChanged implementatie"
        public event PropertyChangedEventHandler PropertyChanged;
        // Toegevoegde lokale utility functie voor raisen van PropertyChanged event.
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region "Logische gelijkheid (IEquatable<Veld> implementatie)"
        // Overgeërfde Object.Equals:
        public override bool Equals(object obj)
        {
            if (obj is Veld) { return Equals(obj as Veld); }
            else { return false; }
        }
        // Impliciete IEquatable<Veld>.Equals implementatie
        public bool Equals(Veld other)
        {
            if (other != null) return this.Rij == other.Rij && this.Kolom == other.Kolom && this.Speler == other.Speler;
            else return false;
        }

        // Operator == en != overloading:
        public static bool operator ==(Veld linker, Veld rechter)
        {
            if (object.ReferenceEquals(linker, rechter)) return true;
            if (((object)linker == null) || ((object)rechter == null)) return false;
            return linker.Equals(rechter);
        }
        public static bool operator !=(Veld linker, Veld rechter)
        {
            return !(linker == rechter);
        }

        public override int GetHashCode()
        {
            return Rij ^ Kolom;
        }
        #endregion
    }
}