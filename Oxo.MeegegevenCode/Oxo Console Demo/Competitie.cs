using System;
using System.Collections.Generic;

namespace Oxo.ConsoleDemo
{
    class Competitie
    {
        static List<ComputerSpeler> _computerSpelers;

        static Dictionary<ComputerSpeler, ComputerSpelerScore> _scoreDictionary = new Dictionary<ComputerSpeler, ComputerSpelerScore>();

        static void Main()
        {
            _computerSpelers = new List<ComputerSpeler>();
            _computerSpelers.Add(new Willekeurigaard(Speler.O));
            _computerSpelers.Add(new Pessimist(Speler.O));
            _computerSpelers.Add(new Opportunist(Speler.O));
            _computerSpelers.Add(new Genie(Speler.O));
            _computerSpelers.AddRange(OxoSpelersArchief.Poeles.SpelersFebruari2016);
            _computerSpelers.AddRange(OxoSpelersArchief.Poeles.SpelersSeptember2016);
            _computerSpelers.AddRange(OxoSpelersArchief.Poeles.SpelersFebruari2017);
            _computerSpelers.AddRange(OxoSpelersArchief.Poeles.SpelersSeptember2017);
            //_computerSpelers.Add(nieuwe-instantie-van-jouwComputerSpeler-met-Speler.O-als-ctor-initialisatiewaarde); 

            Console.WriteLine($"Competitie:\n\nIn volgende competitie speelt elke computerspeler een duel tegen elke andere computerspeler.\nOm het eerlijk te houden wordt er een even aantal spellen gespeeld in elk duel, en mag elke speler afwisselend met O beginnen.");

            int aantalSpellenInDuel;
            Console.Write("Uit hoeveel (even aantal) spellen bestaat elk duel?: ");
            while (!int.TryParse(Console.ReadLine(), out aantalSpellenInDuel) || aantalSpellenInDuel < 2 || aantalSpellenInDuel % 2 != 0)
                Console.Write("Gelieve een geheel even getal (minstens 2) in te voeren!\nUit hoeveel spellen bestaat elk duel?: ");

            Console.WriteLine($"Onderlinge duels:\n");
            _computerSpelers.ForEach((cs) => _scoreDictionary.Add(cs, new ComputerSpelerScore(cs)));
            for (int spelerIndex = 0; spelerIndex < _computerSpelers.Count; spelerIndex++)
            {
                ComputerSpeler speler = _computerSpelers[spelerIndex];
                speler.Speler = Speler.O;

                for (int tegenspelerIndex = spelerIndex + 1; tegenspelerIndex < _computerSpelers.Count; tegenspelerIndex++)
                {
                    ComputerSpeler tegenspeler = _computerSpelers[tegenspelerIndex];
                    tegenspeler.Speler = Speler.X;

                    string duelInfo = $"- {speler.GetType().Name} vs {tegenspeler.GetType().Name}: ";
                    Console.WriteLine($"{duelInfo,-50}");

                    Duel onderlingDuel = SpeelSpellen(aantalSpellenInDuel, speler, tegenspeler);

                    _scoreDictionary[speler].Duels.Add(onderlingDuel);
                    _scoreDictionary[tegenspeler].Duels.Add(onderlingDuel);

                    ComputerSpeler winnaar = onderlingDuel.Winnaar();
                    if (winnaar == null) Console.WriteLine($"          -> Gelijkspel.");
                    else Console.WriteLine($"          -> {winnaar.GetType().Name} wint.");
                }
            }
            Console.WriteLine();

            Dictionary<ComputerSpeler, ComputerSpelerScore>.ValueCollection _scores = _scoreDictionary.Values;
            var scoresArray = new ComputerSpelerScore[_scores.Count];
            _scores.CopyTo(scoresArray, 0);
            Array.Sort(scoresArray);

            Console.WriteLine("Klassement:");
            Console.WriteLine("                                       │ Duels       │ Spellen");
            Console.WriteLine("   │ Computerspelers                   │  #  +  =  - │       +       =       -");
            Console.WriteLine("───┼───────────────────────────────────┼─────────────┼─────────────────────────");
            for (int scoreIndex = 0; scoreIndex < scoresArray.Length; scoreIndex++)
            {
                ComputerSpelerScore score = scoresArray[scoreIndex];
                Console.Write($"{scoreIndex + 1,2} │ {score.Speler.GetType().Name.Substring(0, (score.Speler.GetType().Name.Length >= 32 ? 32 : score.Speler.GetType().Name.Length)) ,-33} │");
                Console.Write($"{score.Duels.Count,3}{score.AantalDuelsGewonnen(),3}{score.AantalDuelsGelijkgespeeld(),3}{score.AantalDuelsVerloren(),3} │");
                Console.WriteLine($"{score.AantalSpellenGewonnen(),8}{score.AantalSpellenGelijkspel(),8}{score.AantalSpellenVerloren(),8}");
            }
            Console.WriteLine("\nLegende: # aantal | + gewonnen | = gelijk | - verloren\n");
            Console.WriteLine("De positie in het klassement wordt gebasseerd op (in volgorde):");
            Console.WriteLine("   1) aantal duels gewonnen");
            Console.WriteLine("   2) aantal duels gelijkgespeeld");
            Console.WriteLine("   3) aantal spellen gewonnen");
            Console.WriteLine("   4) aantal spellen gelijkgespeeld\n");

            Console.WriteLine("Wens je meer detail over een specifiek duel, \nvoer dan de nummers uit de eindstand van de spelers in.");
            do
            {
                int nummerSpeler1 = 1;
                do
                {
                    Console.Write("Nummer van eerste speler in de eindstand?: ");
                } while (!int.TryParse(Console.ReadLine(), out nummerSpeler1) || nummerSpeler1 < 1 || nummerSpeler1 > scoresArray.Length);
                ComputerSpeler speler1 = scoresArray[nummerSpeler1 - 1].Speler;

                int nummerSpeler2 = 2;
                do
                {
                    Console.Write($"Onderling duel van {speler1.GetType().Name} tegen?: ");
                } while (!int.TryParse(Console.ReadLine(), out nummerSpeler2) && nummerSpeler2 < 1 || nummerSpeler2 > scoresArray.Length);
                ComputerSpeler speler2 = scoresArray[nummerSpeler2 - 1].Speler;

                Duel onderlingduel = _scoreDictionary[speler1][speler2];
                Console.WriteLine($@"
Duel: {speler1.GetType().Name} tegen {speler2.GetType().Name}

{onderlingduel.AantalGespeeld} gespeeld = {onderlingduel.Overwinningen()} overwinningen ({onderlingduel.OverwinningenVsGespeeld():P2}) + {onderlingduel.AantalGelijkspelen} gelijkspelen ({onderlingduel.GelijkspelenVsGespeeld():P2})
   
{onderlingduel.AantalOverwinningenSpeler} overwinningen ({onderlingduel.OverwinningenSpelerVsGespeeld():P2}) vr {onderlingduel.Speler.GetType().Name} ({onderlingduel.OverwinningenSpelerVsOverwinningen():P2} v alle overwinningen)
{onderlingduel.AantalOverwinningenTegenspeler} overwinningen ({onderlingduel.OverwinningenTegenspelerVsGespeeld():P2}) vr {onderlingduel.Tegenspeler.GetType().Name} ({onderlingduel.OverwinningenTegenspelerVsOverwinningen():P2} v alle overwinningen)
                    ");

            } while (true);
        }

        static ComputerSpeler _computerSpeler1;
        static ComputerSpeler _computerSpeler2;

        static ComputerSpeler _computerSpelerO = _computerSpeler1;
        static ComputerSpeler _computerSpelerX = _computerSpeler2;

        static void WisselSpelerOEnSpelerX()
        {
            //Omwisselen ComputerSpeler O en X verwijzingen:
            ComputerSpeler vorigeComputerSpelerO = _computerSpelerO;
            _computerSpelerO = _computerSpelerX;
            _computerSpelerX = vorigeComputerSpelerO;

            //Stel ook effectief Speler eigenschap van ComputerSpeler objecten op juiste waarde in:
            _computerSpelerO.Speler = Speler.O;
            _computerSpelerX.Speler = Speler.X;
        }

        static Duel SpeelSpellen(int aantalSpellen, ComputerSpeler speler1, ComputerSpeler speler2)
        {
            var duel = new Duel(van: speler1, tegen: speler2);

            _computerSpelerO = _computerSpeler1 = speler1;
            _computerSpelerX = _computerSpeler2 = speler2;

            double aantGespeeld = 0;
            while (aantGespeeld < aantalSpellen)
            {
                Spel spel = new Spel();

                do
                {
                    if (spel.TeSpelenSpeler == Speler.O) _computerSpelerO.Speel(spel);
                    else _computerSpelerX.Speel(spel);
                } while (spel.Status() == Status.Bezig);

                aantGespeeld++;

                Status status = spel.Status();
                duel.Speelt(status);

                WisselSpelerOEnSpelerX();
            }

            return duel;
        }
    }
}