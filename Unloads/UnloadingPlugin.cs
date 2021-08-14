using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Library;

namespace Unloads
{
    public class UnloadingPlugin : IPlugin
    {
        public record Element(
            int Data
        );

        public void DoJob()
        {
            // Works okay with array, List
            var elements = new List<Element>();
            for (int i = 0; i < 5000; i++)
            {
                elements.Add(new Element(i));
            }

            var immutableElements = elements.ToImmutableArray();
            foreach (Element element in immutableElements)
            {
                Console.WriteLine(element);
            }
        }
    }
}