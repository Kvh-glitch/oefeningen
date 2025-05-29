using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackenqueue
{
    internal class HaakjesParser
    {

        private readonly Dictionary<string, string> _haakjesCombo = new Dictionary<string, string>();

        private readonly HashSet<string> _sluitingsHaakjes = new HashSet<string>();

        private readonly HashSet<string> _symmetrischeHaakjes = new HashSet<string>();

        public void AddHaakjesCombo(string openingsHaakje, string sluitHaakje)
        {
            _haakjesCombo[openingsHaakje] = sluitHaakje;
            _sluitingsHaakjes.Add(sluitHaakje);

            if (openingsHaakje == sluitHaakje)
                _symmetrischeHaakjes.Add(openingsHaakje);
        }

        public bool IsGeldig(string input)
        {
            Stack<string> stack = new Stack<string>();

            foreach (char c in input)
            {
                string character = c.ToString();

                if (_symmetrischeHaakjes.Contains(character))
                {
                    if (stack.Count > 0 && stack.Peek() == character)
                    {
                        stack.Pop();
                    }
                    else
                    {
                        stack.Push(character);
                    }
                }

                else if (_haakjesCombo.ContainsKey(character))
                {
                    stack.Push(character);
                }

                else if (_sluitingsHaakjes.Contains(character))
                {
                    if (stack.Count == 0)
                        return false;

                    string laatstBinnen = stack.Pop();

                    if (!_haakjesCombo.ContainsKey(laatstBinnen) || _haakjesCombo[laatstBinnen] != character)
                        return false;
                }

            }
            return stack.Count == 0;
        }
    }
}
