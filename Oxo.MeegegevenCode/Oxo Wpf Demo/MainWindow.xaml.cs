using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Oxo.WpfDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NieuwSpel();
        }

        private Dictionary<Button, Veld> _bv;
        private Spel _spel;
        private ComputerSpeler _computerSpeler = new Opportunist(Speler.O);

        private void NieuwSpel()
        {
            _computerSpeler.Speler = _computerSpeler.Speler == Speler.O ? Speler.X : Speler.O;

            _spel = new Spel();
            _spel.VolgendeSpelerAanZet += _spel_VolgendeSpelerAanZet;
            _spel.SpelBeëindigd += _spel_SpelBeëindigd;

            _bv = new Dictionary<Button, Veld>();
            _bv[Button1] = _spel.Bord[0, 0]; _bv[Button2] = _spel.Bord[0, 1]; _bv[Button3] = _spel.Bord[0, 2];
            _bv[Button4] = _spel.Bord[1, 0]; _bv[Button5] = _spel.Bord[1, 1]; _bv[Button6] = _spel.Bord[1, 2];
            _bv[Button7] = _spel.Bord[2, 0]; _bv[Button8] = _spel.Bord[2, 1]; _bv[Button9] = _spel.Bord[2, 2];

            foreach (Button b in Bord.Children) { b.Content = "."; b.IsEnabled = true; }
            Bord.IsEnabled = true;

            _spel.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Veld v = _bv[b];
            if (v.IsVrij())
            {
                _spel.Speel(v);
                ToonBord();
            }
        }
        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            NieuwSpel();
            StatusButton.Content = "Bezig";
            StatusButton.IsEnabled = false;
        }

        private void ToonBord()
        {
            foreach (KeyValuePair<Button, Veld> bv in _bv)
            {
                Button b = bv.Key;
                Veld v = bv.Value;
                b.Content = Symb(v.Speler);
                if (v.IsVrij()) b.IsEnabled = true;
            }
            this.Title = _spel.Status().ToString();
        }

        private void _spel_VolgendeSpelerAanZet(object sender, VolgendeSpelerAanZetEventArgs e)
        {
            if (e.SpelerAanZet == _computerSpeler.Speler)
            {
                _computerSpeler.Speel(_spel);
                ToonBord();
            }
        }
        private void _spel_SpelBeëindigd(object sender, SpelBeëindigdEventArgs e)
        {
            Bord.IsEnabled = false;
            if (e.Status == Status.Gelijkspel) StatusButton.Content = "Gelijkspel.";
            else if (e.Status == Status.SpelerOGewonnen) StatusButton.Content = $"Speler O gewonnen!";
            else if (e.Status == Status.SpelerXGewonnen) StatusButton.Content = $"Speler X gewonnen!";
            StatusButton.Content += "  Klik om opnieuw te spelen.";
            StatusButton.IsEnabled = true;
        }

        public static string Symb(Veld veld)
        {
            return Symb(veld.Speler);
        }
        public static string Symb(Speler? speler)
        {
            if (speler == Speler.O) return "O";
            else if (speler == Speler.X) return "X";
            return ".";
        }
    }
}
