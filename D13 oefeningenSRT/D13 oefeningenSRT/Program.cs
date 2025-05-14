using System.Diagnostics;

namespace D13_oefeningenSRT
{
    internal class Program
    {

        static void Main(string[] args)
        {
            (string fileName, string path) = VragenNaarBestand("Geef de naam van het bestand (inclusief .srt): ");

            int offset = VragenNaarOffset("Offset in milliseconden (kan negatief zijn): ");


            string[] lines = File.ReadAllLines(path);
            (string[] nummers, string[] subtitles, string[] timestamps) = SplitsenInArrays(lines);

            string backupPath = path + ".backup";

            try
            {
                File.Copy(path, backupPath, true);
                Console.WriteLine($"Backup gemaakt van {fileName} naar {Path.GetFileName(backupPath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het maken van de backup:\n{ex.Message}");
                return;
            }
            string[] aangepasteLijnen = ApplyOffsetMetLogging(lines, offset, out List<string> logs);
            ToonWijzigingen(nummers, subtitles, logs);
            WriteToFile(aangepasteLijnen, path);

            Console.WriteLine($"Offset succesvol toegepast op het bestand {fileName}.");



        }
        static (string[] nummers, string[] subtitles, string[] timestamps) SplitsenInArrays(string[] lines)
        {
            var nummers = new List<string>();
            var timestamps = new List<string>();
            var subtitles = new List<string>();


            for (int i = 0; i < lines.Length; i++)
            {
                string nummer = lines[i++];
                nummers.Add(nummer);

                if (string.IsNullOrEmpty(lines[i]))
                    continue;
                if (i < lines.Length && lines[i].Contains("-->"))
                    timestamps.Add(lines[i++]);

                var ondertitelLijnen = new List<string>();
                while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
                    ondertitelLijnen.Add(lines[i++]);

                subtitles.Add(string.Join("\n", ondertitelLijnen));

                while (i < lines.Length && string.IsNullOrWhiteSpace(lines[i]))
                {
                    i++;
                }
            }
            return (nummers.ToArray(), subtitles.ToArray(), timestamps.ToArray());
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

        static string[] ApplyOffsetMetLogging(string[] lines, int offset, out List<string> logs)
        {
            logs = new List<string>();
            string[] output = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Contains("-->"))
                {
                    (string startRaw, string endRaw) = SplitTimeCodes(lines[i]);
                    (string startOutput, string startLog) = PasTijdToeMetLog(startRaw, offset, "Start");
                    (string endOutput, string endLog) = PasTijdToeMetLog(endRaw, offset, "Eind");
                    output[i] = $"{startOutput} --> {endOutput}";
                    logs.Add($"{startLog}\n{endLog}");

                }
                else output[i] = line;
            }
            return output;

        }

        static (string start, string end) SplitTimeCodes(string line)
        {
            string[] parts = line.Split(new[] { " --> " }, StringSplitOptions.None);
            return (parts[0].Trim(), parts[1].Trim());
        }
        static (string output, string log) PasTijdToeMetLog(string origineleTijd, int offset, string label)
        {
            bool parsed = TimeSpan.TryParseExact(origineleTijd, @"hh\:mm\:ss\,fff", null, out var tijd);
            if (parsed)
            {
                var nieuweTijd = tijd.Add(TimeSpan.FromMilliseconds(offset));
                string output = nieuweTijd.ToString(@"hh\:mm\:ss\,fff");
                string log = $"{label} timecode {origineleTijd} aangepast met offset {offset} ms naar {output}";
                return (output, log);
            }
            else
            {
                string log = $"{label} timecode \"{origineleTijd}\" wordt niet herkend en wordt niet aangepast";
                return (origineleTijd, log);
            }
        }
        static void WriteToFile(string[] lines, string path)
        {
            try
            {
                File.WriteAllLines(path, lines);
                Console.WriteLine($"Bestand succesvol geschreven naar {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het schrijven naar het bestand:\n{ex.Message}");
            }
        }
        static void ToonWijzigingen(string[] nummers, string[] subtitles, List<string> logs)
        {
            for (int i = 0; i < nummers.Length; i++)
            {
                Console.WriteLine(nummers[i]);
                Console.WriteLine(subtitles[i]);
                Console.WriteLine(logs[i]);
                Console.WriteLine();
            }
        }
    }
}

