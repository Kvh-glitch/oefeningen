namespace D17gemiddeldetellingen
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Dictionary<string, int> tellingenJan = new Dictionary<string, int>()
            {
                { "hond", 5},
                { "papegaai", 1 },
                { "worm", 3 },
                { "konijn", 2 },
                { "gruffalo", 0 }
            };


            Dictionary<string, int> tellingenMieke = new Dictionary<string, int>()
            {
                { "hond", 2},
                { "worm", 1 },
                { "konijn", 3 },
                { "gruffalo", 1 },
                { "dromedaris", 2 }
            };

            Dictionary<string, double> gemiddelde = GetGemiddelde(tellingenJan, tellingenMieke);
            foreach (var item in gemiddelde)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        static Dictionary<string, double> GetGemiddelde(Dictionary<string, int> telling1, Dictionary<string, int> telling2)
        {
            Dictionary<string, double> gemiddelde = new Dictionary<string, double>();

            foreach (var kvp in telling1)
            {
                string dierensoort = kvp.Key;
                int aantal1 = kvp.Value;
                
                if (telling2.TryGetValue(dierensoort, out int aantal2))
                {
                    double gemiddeldeWaarde = (aantal1 + aantal2) / 2.0;
                    gemiddelde[dierensoort] = gemiddeldeWaarde;
                }

            }
            return gemiddelde;
        }
    }
}
