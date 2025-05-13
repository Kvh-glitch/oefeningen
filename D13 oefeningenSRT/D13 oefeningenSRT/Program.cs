namespace D13_oefeningenSRT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            (string fileName, string path) = VragenNaarBestand("Geef de naam van het bestand (inclusief .srt): ");

            int offset = VragenNaarOffset("Offset in milliseconden (kan negatief zijn): ");


            string[] lines = File.ReadAllLines(path);
            string backupPath = path + ".backup";
            try
            {
                File.Copy(path, backupPath, true);
                Console.WriteLine($"Backup gemaakt van {fileName} naar {backupPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het maken van de backup:\n{ex.Message}");
                return;
            }

            ApplyOffsetMetLogging(lines, offset);
            

            try
            {
                File.WriteAllLines(path, lines);
                Console.WriteLine("Offset toegepast op het bestand.");
                Console.WriteLine($"Origineel bestand is opgeslagen als backup op {Path.GetFileName(backupPath)}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het opslaan van het bestand:\n{ex.Message}");
                return;
            }

            
        }
        static (string fileName, string path) VragenNaarBestand(string vraag)
        {
            while (true)
            {
                Console.WriteLine(vraag);
                string fileName = Console.ReadLine();
                string path = Path.Combine(AppContext.BaseDirectory, "subtitles", fileName);
                try
                {
                    using (FileStream stream = File.OpenRead(path)) { }
                    return (fileName, path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fout bij het controleren van het bestand:\n{ex.Message}");
                    Console.WriteLine("Probeer opnieuw.\n");

                }

            }
        }
        static int VragenNaarOffset(string vraag)
        {
            while (true)
            {
                Console.WriteLine(vraag);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int offset))
                {
                    return offset;
                }
                Console.WriteLine("Ongeldige invoer. Probeer opnieuw.");
            }
        }
        static void ApplyOffsetMetLogging(string[] lines, int offset)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("-->"))
                {
                    (string startRaw, string endRaw) = SplitTimeCodes(lines[i]);
                    string startOutput = PasTijdToeMetLog(startRaw, offset, "Start");
                    string endOutput = PasTijdToeMetLog(endRaw, offset, "Eind");
                    lines[i] = $"{startOutput} --> {endOutput}";
                }
               
            }
        }
        static (string start, string end) SplitTimeCodes(string line)
        {
            string[] parts = line.Split(new[] { " --> " }, StringSplitOptions.None);
            return (parts[0].Trim(), parts[1].Trim());
        }
        static string PasTijdToeMetLog(string origineleTijd, int offset, string label)
        {
            bool parsed = TimeSpan.TryParseExact(origineleTijd, @"hh\:mm\:ss\,fff", null, out var tijd);
            if (parsed)
            {
                var nieuweTijd = tijd.Add(TimeSpan.FromMilliseconds(offset));
                
                string output = nieuweTijd.ToString(@"hh\:mm\:ss\,fff");
                Console.WriteLine($"{label} timecode {origineleTijd} aangepast met offset {offset} ms naar {output}");
                return output;
            }
            else
            {
                Console.WriteLine($"{label} timecode \"{origineleTijd}\" wordt niet herkend en wordt niet aangepast");
                return origineleTijd; 
            }
        
        
        }
    }
}
