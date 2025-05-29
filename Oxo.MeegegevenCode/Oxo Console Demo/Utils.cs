using System;
using System.Collections.Generic;

namespace Oxo.ConsoleDemo
{
    class Utils
    {
        public static Dictionary<Speler, ComputerSpeler> KiesSpelers()
        {
            var spelers = new Dictionary<Speler, ComputerSpeler>();

            Console.ForegroundColor = ConsoleColor.Blue; Console.Clear();
            foreach (Speler speler in System.Enum.GetValues(typeof(Speler)))
            {
                Console.Write($@"Speler {speler} is een:
- <m>ens
- <w>illekeurigaard (computer level 1)
- <p>essimist (computer level 2)
- <o>pportunist (computer level 3) ?: ");
                ComputerSpeler computerSpeler = null;
                string wie = Console.ReadLine().Trim().ToLower();
                if (wie == "w") computerSpeler = new Willekeurigaard(speler);
                else if (wie == "p") computerSpeler = new Pessimist(speler);
                else if (wie == "o") computerSpeler = new Opportunist(speler);
                spelers.Add(speler, computerSpeler);
            }

            return spelers;
        }

        public static int Invoer(string label)
        {
            int getal;
            bool invoerOk;
            do
            {
                Console.Write(label + "?: ");
                invoerOk = int.TryParse(Console.ReadLine(), out getal);
                if (!invoerOk) Console.WriteLine("Invoer niet ok.");
            } while (!invoerOk || getal < 1 || getal > 3);
            return getal;
        }

        public static void PrintSpelInfo(Spel spel)
        {
            Console.WriteLine(SpelBord(spel) + "\n");
            Console.WriteLine(LijnenInfo(spel) + "\n");
            Console.WriteLine(LegeVeldenInfo(spel) + "\n");
            Console.WriteLine(StatusInfo(spel) + "\n");
        }

        public static string SpelBord(Spel spel)
        {
            return $@"  Kolom   1   2   3
        ┌───┬───┬───┐
  Rij 1 │ {Symb(spel.Bord[0, 0])} │ {Symb(spel.Bord[0, 1])} │ {Symb(spel.Bord[0, 2])} │
        ├───┼───┼───┤
  Rij 2 │ {Symb(spel.Bord[1, 0])} │ {Symb(spel.Bord[1, 1])} │ {Symb(spel.Bord[1, 2])} │
        ├───┼───┼───┤
  Rij 3 │ {Symb(spel.Bord[2, 0])} │ {Symb(spel.Bord[2, 1])} │ {Symb(spel.Bord[2, 2])} │
        └───┴───┴───┘";
        }
        public static string LijnenInfo(Spel spel)
        {
            string lijnenInfo = $"{"Lijn:",23} | {"Velden:",-15} | {"Vrij:",-5} | {"Vol:",-5} | {"Reeks:",-6} | {"Reeks kan:",-10}";
            foreach (Lijn lijn in spel.Bord.Lijnen)
            {
                string velden = null; foreach (Veld veld in lijn.Velden) velden += veld;
                lijnenInfo += $"\n{lijn.Naam,23} | {velden,-15} | {lijn.AantalVrijeVelden(),-5} | {lijn.IsVol(),-5} | {lijn.IsReeks(),-6} | {lijn.IsReeksMogelijk(),-10}";
            }
            return lijnenInfo;
        }
        public static string LegeVeldenInfo(Spel spel)
        {
            List<Veld> legeVelden = spel.Bord.LegeVelden();
            string legeVeldenInfo = null;
            foreach (Veld veld in spel.Bord.LegeVelden()) legeVeldenInfo += veld;
            return $"           {legeVelden.Count} lege velden: {legeVeldenInfo}";
        }
        public static string StatusInfo(Spel spel)
        {
            string statusInfo = null;
            Status status = spel.Status();
            if (status == Status.Bezig) statusInfo += $"Spel bezig, speler {Symb(spel.TeSpelenSpeler)} aan beurt...";
            else if (status == Status.Gelijkspel) statusInfo += "Gelijkspel.";
            else if (status == Status.SpelerOGewonnen) statusInfo += "Speler O gewonnen!";
            else if (status == Status.SpelerXGewonnen) statusInfo += "Speler X gewonnen!";
            return statusInfo;
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