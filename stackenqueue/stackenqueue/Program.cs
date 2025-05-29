namespace stackenqueue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HaakjesParser target = new HaakjesParser();
            
            target.AddHaakjesCombo(openingsHaakje: "(", sluitHaakje: ")");
            target.AddHaakjesCombo(openingsHaakje: "[", sluitHaakje: "]");
            target.AddHaakjesCombo(openingsHaakje: "{", sluitHaakje: "}");

            target.AddHaakjesCombo("|", "|"); // testen of hij dit ook kan
            target.AddHaakjesCombo("L", "P"); // testen of hij dit ook kan
           

            Console.WriteLine(target.IsGeldig("abc(def[ghi])jkl") == true);  // assertion should be true
            Console.WriteLine(target.IsGeldig("abcd|f)") == false);          // assertion should be false
            Console.WriteLine(target.IsGeldig("abc(de][fgh)i") == false);    // assertion should be true
            Console.WriteLine(target.IsGeldig("abc{def})ghi") == false);     // assertion should be true
            Console.WriteLine(target.IsGeldig("[abc](def)[ghi)") == false);  // assertion should be true
            Console.WriteLine(target.IsGeldig("abc{def[ghijkl}") == false); // assertion should be true
            Console.WriteLine(target.IsGeldig("abcdefghijkl]}") == true); // assertion should be false
            Console.WriteLine(target.IsGeldig("abc|def|ghi") == true); // assertion should be true
            Console.WriteLine(target.IsGeldig("abLc(def)hPi") == true); // assertion should be true
            Console.WriteLine(target.IsGeldig("abL|c(def)h|Pi") == true); // assertion should be true want L | ( ) | P

        }
    }
}
