using System;
using System.Collections.Generic;
using static Oxo.ConsoleDemo.Utils;

namespace Oxo.ConsoleDemo
{
    class DemoProgram1 // Zonder eventhandling.
    {
        static void Main()
        {
            Console.BackgroundColor = ConsoleColor.White;
            do
            {
                Spel spel = new Spel();
                ////Of bv onderstaande, als je wil vertrekken van een bepaald bord en startende speler:
                //Bord bord = new Bord();
                //bord[1, 1].Speler = Speler.O; bord[2, 2].Speler = Speler.X;
                //Spel spel = new Spel(bord, startendeSpeler: Speler.O);

                var spelers = KiesSpelers();

                Console.ForegroundColor = ConsoleColor.Black; Console.Clear();
                PrintSpelInfo(spel);

                do
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                    Speler teSpelenSpeler = spel.TeSpelenSpeler;
                    if (spelers[teSpelenSpeler] is ComputerSpeler)
                    {
                        Console.Write("Druk op enter om de computer te laten spelen..."); Console.ReadLine();

                        ComputerSpeler computerSpeler = spelers[teSpelenSpeler];
                        computerSpeler.Speel(spel);
                    }
                    else //Menselijke speler...
                    {
                        int rij, kolom;
                        Veld veld;
                        bool vrijVeld = false;

                        //Met runtime controle:
                        do
                        {
                            rij = Invoer("Rij");
                            kolom = Invoer("Kolom");

                            veld = spel.Bord[rij - 1, kolom - 1];
                            vrijVeld = veld.IsVrij();

                            if (!vrijVeld) Console.WriteLine($"Dit mag niet: Het bespeelde veld ({veld}) is niet meer vrij.");
                        } while (!vrijVeld);
                        spel.Speel(veld); //Of: spel.Speel(rij, kolom);

                        ////Of met exception handling:
                        //do
                        //{
                        //    rij = Invoer("Rij");
                        //    kolom = Invoer("Kolom");
                        //
                        //    veld = spel.Bord[rij - 1, kolom - 1];
                        //    try
                        //    {
                        //        spel.Speel(veld); //Of: spel.Speel(rij, kolom);
                        //        vrijVeld = true;
                        //    }
                        //    catch (InvalidOperationException ex)
                        //    {
                        //        Console.WriteLine("Dit mag niet: " + ex.Message); // Het bespeelde veld (...) is niet meer vrij.
                        //    }
                        //} while (!vrijVeld);
                    }

                    Console.ForegroundColor = ConsoleColor.Black; Console.Clear();
                    PrintSpelInfo(spel);

                } while (spel.Status() == Status.Bezig);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Druk op enter om een nieuw spel te spelen..."); Console.ReadLine();

            } while (true);
        }
    }
}