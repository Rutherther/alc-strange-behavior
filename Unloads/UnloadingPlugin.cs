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
            string Data
        );
        
        public void DoJob()
        {
            // Works okay with array, List
            var elements = new[]
            {
                new Element("a"),
                new Element("b"),
                new Element("c"),
                new Element("d"),
            }.ToImmutableArray();

            foreach (Element element in elements)
            {
                Console.WriteLine(element);
            }
        }
    }
}