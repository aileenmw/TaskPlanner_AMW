using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.Helpers
{
    public class Translator { 

          private static readonly Dictionary<string, string> Danish
                = new Dictionary<string, string>
            {
                {"FirstName", "Fornavn"},
                {"LastName", "Efternavn"},
                {"Age", "Alder"}
            };

    public static string toDanish(string toBeTranslated)
        {
            string translated = toBeTranslated;
            if (Danish.ContainsKey(toBeTranslated)) {
                translated = Danish[toBeTranslated];
            }
            return translated;
        }
    }
}
