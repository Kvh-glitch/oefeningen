using System;
using System.Collections.Generic;
using static Oxo.ConsoleDemo.Utils;

namespace Oxo.ConsoleDemo
{
    class DemoProgram2 // Met eventhandling.
    {
        static Dictionary<Speler, ComputerSpeler> _spelers;

        static void Main()
        {
            Console.BackgroundColor = ConsoleColor.White;
            do
            {
                Console.Clear();

                Spel spel = new Spel();

                _spelers = KiesSpelers();

                spel.VolgendeSpelerAanZet += _spel_VolgendeSpelerAanZet;
                spel.SpelBeëindigd += _spel_SpelBeëindigd;

                spel.Start();
            } while (true);
        }

        static void _spel_VolgendeSpelerAanZet(object sender, VolgendeSpelerAanZetEventArgs e)
        {
            Spel spel = sender as Spel;

            Console.ForegroundColor = ConsoleColor.Black; Console.Clear();
            PrintSpelInfo(spel);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Speler teSpelenSpeler = e.SpelerAanZet;
            if (_spelers[teSpelenSpeler] is ComputerSpeler)
            {
                Console.Write("Druk op enter om de computer te laten spelen..."); Console.ReadLine();

                ComputerSpeler computerSpeler = _spelers[teSpelenSpeler];
                computerSpeler.Speel(spel);
            }
            else
            {
                int rij, kolom;
                Veld veld;
                bool vrijVeld = false;
                do
                {
                    rij = Invoer("Rij");
                    kolom = Invoer("Kolom");

                    veld = spel.Bord[rij - 1, kolom - 1];
                    vrijVeld = veld.IsVrij();

                    if (!vrijVeld) Console.WriteLine($"Dit mag niet: Het bespeelde veld ({veld}) is niet meer vrij.");
                } while (!vrijVeld);
                spel.Speel(veld);
            }
        }
        static void _spel_SpelBeëindigd(object sender, SpelBeëindigdEventArgs e)
        {
            Spel spel = sender as Spel;

            Console.ForegroundColor = ConsoleColor.Black; Console.Clear();
            Console.WriteLine(SpelBord(spel) + "\n");
            Console.WriteLine(LijnenInfo(spel) + "\n");
            Console.WriteLine(LegeVeldenInfo(spel) + "\n");

            if (e.Status == Status.Gelijkspel) Console.WriteLine("Gelijkspel.\n");
            else if (e.Status == Status.SpelerOGewonnen) Console.WriteLine($"Speler O gewonnen!\n");
            else if (e.Status == Status.SpelerXGewonnen) Console.WriteLine($"Speler X gewonnen!\n");
            //Of: Console.WriteLine(StatusInfo(spel) + "\n");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Druk op enter om een nieuw spel te spelen..."); Console.ReadLine();
        }
    }
}