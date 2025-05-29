using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;

namespace Oxo
{
    public abstract class ComputerSpeler
    {
        public Speler Speler { get; set; }

        public ComputerSpeler(Speler speler) { Speler = speler; }

        public void Speel(Spel spel)
        {
            Veld veld = VerkiestVeld(spel);
            spel.Speel(veld);
        }
        public abstract Veld VerkiestVeld(Spel spel);
    }

    public class Willekeurigaard : ComputerSpeler
    {
        public Willekeurigaard(Speler speler) : base(speler) { }

        protected static Random _random = new Random();
        public override Veld VerkiestVeld(Spel spel)
        {
            return WillekeurigVeld(spel);
        }

        private static Veld WillekeurigVeld(Spel spel)
        {
            List<Veld> legeVelden = spel.Bord.LegeVelden();
            int willekeurigeIndex = _random.Next(0, legeVelden.Count);
            return legeVelden[willekeurigeIndex];
        }
    }
    public class Pessimist : Willekeurigaard
    {
        public Pessimist(Speler speler) : base(speler) { }

        public Speler Tegenstander { get { return (Speler == Oxo.Speler.O ? Oxo.Speler.X : Oxo.Speler.O); } }

        protected Veld BlokkerendVeld(Spel spel)
        {
            foreach (Lijn lijn in spel.Bord.Lijnen)
            {
                //Indien er op een lijn nog 1 vrij veld is, en 2 velden die door de tegenstander zijn bezet:
                if (lijn.AantalVrijeVelden() == 1 && lijn.AantalVelden(Tegenstander) == 2)
                {
                    //Lever het nog vrij veld op.
                    foreach (Veld v in lijn.Velden) if (v.IsVrij()) return v;
                    //of: return lijn.Velden.First(veld => veld.IsVrij());
                }
            }
            return null;
        }
        public override Veld VerkiestVeld(Spel spel)
        {
            //1) Probeer een reeks van de tegenstander te blokkeren:
            Veld blokkerendVeld = BlokkerendVeld(spel);
            if (blokkerendVeld != null) return blokkerendVeld;

            //2) Indien het middelste veld nog vrij is lever deze op:
            Veld middelsteVeld = spel.Bord[1, 1];
            if (middelsteVeld.IsVrij()) return middelsteVeld;

            //3) Willekeurig vrij veld opleveren:
            return base.VerkiestVeld(spel);
        }
    }
    public class Opportunist : Pessimist
    {
        public Opportunist(Speler speler) : base(speler) { }

        protected Veld WinnendVeld(Spel spel)
        {
            foreach (Lijn lijn in spel.Bord.Lijnen)
            {
                //Indien er op een lijn nog 1 vrij veld is, en 2 velden die door deze speler zijn bezet:
                if (lijn.AantalVrijeVelden() == 1 && lijn.AantalVelden(Speler) == 2)
                {
                    //Lever het nog vrij veld op.
                    foreach (Veld v in lijn.Velden) if (v.IsVrij()) return v;
                    //of: return lijn.Velden.First(veld => veld.IsVrij());
                }
            }
            return null;
        }

        public override Veld VerkiestVeld(Spel spel)
        {
            //1) Probeer zelf een reeks te maken:
            Veld winnendVeld = WinnendVeld(spel);
            if (winnendVeld != null) return winnendVeld;

            //2) Kan er geen reeks gemaakt worden => tegenstander blokkeren:
            return base.VerkiestVeld(spel);
        }
    }

    public class Genie : Opportunist
    {
        // dictionary aanmaken

        private Dictionary<int, Func<Spel, Veld>> strategiePerZet;

        // dictionary met strategieën per zet:
        public Genie(Speler speler) : base(speler)
        {
            strategiePerZet = new Dictionary<int, Func<Spel, Veld>>
            {
                { 0, Zet1Aanval },
                { 1, Zet1Verdediging },
                { 2, Zet2Aanval },
                { 3, Zet2Verdediging },
                { 4, Zet3Aanval },
                { 5, Zet3Verdediging }
            };
        }
        // aangepast, om de strategie dictionary te gebruiken op basis van zet, anders default gedrag
        public override Veld VerkiestVeld(Spel spel)
        {
            int huidigeZet = AantalZetten(spel);

            if (strategiePerZet.ContainsKey(huidigeZet))
            {
                Func<Spel, Veld> strategie = strategiePerZet[huidigeZet];
                return strategie(spel);
            }

            return base.VerkiestVeld(spel);
        }
        // hoeveelste zet berekenen op basis van aantal legevelden
        protected int AantalZetten(Spel spel)
        {
            int aantalLegeVelden = spel.Bord.LegeVelden().Count;
            return 9 - aantalLegeVelden;
        }
        //oplijsting hoeken
        protected List<Veld> Hoeken(Spel spel)
        {
            return new List<Veld> { spel.Bord[0, 0], spel.Bord[0, 2], spel.Bord[2, 0], spel.Bord[2, 2] };
        }
        //oplijsten randvelden, de velden die niet in het midden of de hoeken staan
        protected List<Veld> RandVelden(Spel spel)
        {
            return new List<Veld> { spel.Bord[0, 1], spel.Bord[1, 0], spel.Bord[1, 2], spel.Bord[2, 1] };
        }
        //methode om een lijst vrijevelden te maken met een List velden als meegegeven parameter 
        protected List<Veld> VrijeVelden(List<Veld> velden)
        {
            List<Veld> vrijeVelden = new List<Veld>();

            foreach (Veld v in velden)
            {
                if (v.IsVrij())
                {
                    vrijeVelden.Add(v);
                }
            }
            return vrijeVelden;
        }

        // tegenzet op basis van een hoek die door de tegenstander bezet is , hoek 0 (linksboven), reactie hoek 3 (rechtsonder)
        protected Veld KiesTegenovergesteldeHoek(Spel spel)
        {
            List<Veld> hoeken = Hoeken(spel);

            if (hoeken[0].Speler == Tegenstander && hoeken[3].IsVrij())
                return hoeken[3];
            if (hoeken[3].Speler == Tegenstander && hoeken[0].IsVrij())
                return hoeken[0];
            if (hoeken[1].Speler == Tegenstander && hoeken[2].IsVrij())
                return hoeken[2];
            if (hoeken[2].Speler == Tegenstander && hoeken[1].IsVrij())
                return hoeken[1];

            return null;

        }
        // tegenzet op basis van een rand die bezet is
        protected Veld KiesVerdedigingOpBasisVanRand(Spel spel, Veld bezetteRand)
        {
            List<Veld> hoeken = Hoeken(spel);
            List<Veld> randen = RandVelden(spel);
            if (bezetteRand == randen[0])
                return hoeken[0];
            if (bezetteRand == randen[2])
                return hoeken[1];
            if (bezetteRand == randen[3])
                return hoeken[4];
            if (bezetteRand == randen[1])
                return hoeken[3];

            return spel.Bord[1, 1];

        }

        // als eerste beginnen 

        // vaste hoek nemen
        protected Veld Zet1Aanval(Spel spel)
        {
            return spel.Bord[0, 0];
        }
        //nog een hoek nemen 
        protected Veld Zet2Aanval(Spel spel)
        {
            if (spel.Bord[2, 2].IsVrij())
                return spel.Bord[2, 2];
            else
                return spel.Bord[0, 2];
        }
        // winnendezet proberen, anders tegenzet, anders hoek, anders basisgedrag
        protected Veld Zet3Aanval(Spel spel)
        {
            if (WinnendVeld(spel) is Veld winnend)
                return winnend;

            List<Veld> opties = new List<Veld> { spel.Bord[0, 0], spel.Bord[2, 0] };

            foreach (var veld in opties)
            {
                if (veld.IsVrij())
                {
                    if (BlokkerendVeld(spel) is Veld blokkerend)
                        return blokkerend;

                    return veld;
                }
            }

            List<Veld> vrijeHoeken = VrijeVelden(Hoeken(spel));
            if (vrijeHoeken.Count > 0)
                return vrijeHoeken[0];

            return base.VerkiestVeld(spel);
        }
        // als tweede beginnen 

        // als er een randveld bezet is, kies de passende tegenzet, dan een tegenovergestelde hoek, dan gewoon de eerste hoek [0,0]
        protected Veld Zet1Verdediging(Spel spel)
        {
            if (spel.Bord[1, 1].IsVrij())
            {
                Veld bezetteRand = null;
                foreach (var veld in RandVelden(spel))
                {
                    if (veld.Speler != null)
                    {
                        bezetteRand = veld;
                        break;
                    }
                }
                return KiesVerdedigingOpBasisVanRand(spel, bezetteRand);
            }

            Veld tegenovergesteldeHoek = KiesTegenovergesteldeHoek(spel);
            if (tegenovergesteldeHoek != null)
                return tegenovergesteldeHoek;
            return spel.Bord[0, 0];

        }
        // blokkerende zet eerst, dan eventueel vrijehoek, anders basisgedrag
        protected Veld Zet2Verdediging(Spel spel)
        {
            if (BlokkerendVeld(spel) is Veld blokkerend)
                return blokkerend;

            if (spel.Bord[1, 1].Speler == Tegenstander)
            {
                List<Veld> vrijeHoeken = VrijeVelden(Hoeken(spel));
                if (vrijeHoeken.Count > 0)
                    return vrijeHoeken[0];
            }

            return base.VerkiestVeld(spel);
        }
        // blokkerende zet eerst, dan vrijehoek, dan vrije rand, anders basisgedrag
        protected Veld Zet3Verdediging(Spel spel)
        {
            Veld blokkerendVeld = BlokkerendVeld(spel);
            if (blokkerendVeld != null)
                return blokkerendVeld;


            List<Veld> vrijeHoeken = VrijeVelden(Hoeken(spel));
            if (vrijeHoeken.Count > 0)
                return vrijeHoeken[0];


            List<Veld> vrijeRandVelden = VrijeVelden(RandVelden(spel));
            if (vrijeRandVelden.Count > 0)
                return vrijeRandVelden[0];


            return base.VerkiestVeld(spel);
        }


    }
}






