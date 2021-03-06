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
            int Data
        );

        public void DoJob()
        {
            var elements = new List<Element>();
            for (int i = 0; i < 5000; i++)
            {
                elements.Add(new Element(i));
            }

            var immutableElements = elements.ToImmutableList();

            // works okay with any object that is not loaded inside AssemblyLoadContext of the plugin
            // okay means that it unloads correctly on second iteration of GC.Collect (waiting for 5000 + 2000 ms)
            
            // For debug
            // unload takes more time means it unloads correctly on 49th iteration or later. (5000 + 49000 ms at least for me, may be different for you)
            // Some runs took 49 iterations, some even 420 iterations
            
            // In release it took 21 iterations
            
            // Minimum would be to call immutableElements.GetEnumerator().Dispose()
            // instead of iterating over foreach
            foreach (var element in immutableElements)
            {
                Console.WriteLine(element);
            }
        }
    }
}