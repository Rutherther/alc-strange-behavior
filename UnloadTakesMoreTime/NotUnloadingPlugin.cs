using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Library;

namespace DoesntUnload
{
    public class NotUnloadingPlugin : IPlugin
    {
        // Record stored in immutable list
        public record Element(
            string Data
        );

        public void DoJob()
        {
            var elements = new[]
            {
                new Element("a"),
                new Element("b"),
                new Element("c"),
                new Element("d"),
                new Element("e"),
            }.ToImmutableList();
            
            // works okay with char, string, int, object (seems like that it works okay with all built-in types, but I did not test each one of them)
            // unload takes more time for custom classes/records, structs being iterated over
            // okay means that it unloads correctly on second iteration of GC.Collect (waiting for 5000 + 2000 ms)
            
            // For debug
            // unload takes more time means it unloads correctly on 49th iteration or later. (5000 + 49000 ms at least for me, may be different for you)
            // Some runs took 49 iterations, some even 420 iterations
            
            // In release it took 21 iterations
            foreach (var element in elements)
            {
                Console.WriteLine(element);
            }
        }
    }
}