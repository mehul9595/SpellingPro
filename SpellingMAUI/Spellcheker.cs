using System.Linq;
using NHunspell;

namespace SpellingMAUI
{
    public class Spellcheker : IDisposable
    {
        // I want to use NHunspell to check the spelling of words using en-US, and en-GB dictionaries.
        private List<Hunspell> hunspells;

        public Spellcheker()
        {
            string affPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionaries", "en_US.aff");
            string dicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionaries", "en_US.dic");

            string affPathGB = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionaries", "en_GB.aff");
            string dicPathGB = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionaries", "en_GB.dic");

            var hunspellGB = new Hunspell(affPathGB, dicPathGB);

            var hunspellUS = new Hunspell(affPath, dicPath);

            hunspells = new List<Hunspell>
            {
                hunspellUS,
                hunspellGB
            };
        }

        public void Dispose()
        {
            if (hunspells != null & hunspells.Count > 0)
            {
                hunspells.ForEach(x => x.Dispose());
            }
        }

        public bool Spell(string word)
        {
            return hunspells.Any(x =>x.Spell(word));
        }

        public bool Math(string correctWord, string word)
        {
            // this function checks the 'word' against the 'correctWord' and returns true if both are correct
            var checkSuggestion = hunspells.Any(x => x.Suggest(correctWord).Contains(word));

            return checkSuggestion == false ? correctWord == word && Spell(correctWord) && Spell(word) : checkSuggestion;
        }
    }
}