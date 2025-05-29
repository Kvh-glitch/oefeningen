namespace D15.D15stellingverhuur
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime startVerhuur = new DateTime(2022, 1, 1, 8, 0, 0);
            DateTime eindVerhuur = new DateTime(2022, 2, 1, 16, 30, 0);

            StellingVerhuring sv1 = new StellingVerhuring(startVerhuur, eindVerhuur);

            int aantalUurOpbouw = sv1.AantalUurOpbouw;
            int aantalUurAfbraak = sv1.AantalUurAfbraak;

            Console.WriteLine(aantalUurAfbraak);

            Periode verhuurPeriode = sv1.NettoVerhuurPeriode();
            DateTime startTijdstip = verhuurPeriode.Start;
            Console.WriteLine(startTijdstip);
            DateTime eindTijdstip = verhuurPeriode.Einde;
            Console.WriteLine(eindTijdstip);

            int aantalUur = verhuurPeriode.AantalUur();
            Console.WriteLine(aantalUur);

            decimal prijs = sv1.Prijs();
            Console.WriteLine(prijs);
            sv1.AantalUurOpbouw = 5;
            sv1.AantalUurAfbraak = 3;
            decimal prijs2 = sv1.Prijs();
            Console.WriteLine(prijs2);
            Console.WriteLine(verhuurPeriode.Start);
            Console.WriteLine(verhuurPeriode.Einde);

            verhuurPeriode = sv1.NettoVerhuurPeriode();

            Levering leveringX = new Levering("Antwerpen", 62);
            Levering leveringY = new Levering("Gent", 43);
            string adres = leveringX.Adres;
            Console.WriteLine(adres);
            int afstandInKm = leveringY.AfstandInKm;
            Console.WriteLine(afstandInKm);

            sv1.Levering = leveringY;
            Levering leveringSv1 = sv1.Levering;
            Console.WriteLine(leveringSv1.Adres);
            Console.WriteLine(leveringSv1.AfstandInKm);
            Console.WriteLine(sv1.Prijs());
        }
    }
}
