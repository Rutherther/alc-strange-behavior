using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Library;

namespace Application
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // NOTE: build solution twice (build + rebuild)
            // for auto copy plugins to correct folder
            // If that doesn't work for you,
            // copy Unloads/bin/Debug/net5.0/* into Application/bin/Debug/net5.0/Plugins/Unloads/
            // copy UnloadTakesMoreTime/bin/Debug/net5.0/* into Application/bin/Debug/net5.0/Plugins/UnloadTakesMoreTime/
            // or from and to /bin/Release/net5.0 if release version is used

            // Difference between Unloads and UnloadTakesMoreTime
            // is just Unloads uses ToImmutableArray,
            // UnloadTakesMoreTime uses ToImmutableList

            // More information in UnloadTakesMoreTime NotUnloadingPlugin

            await RunAndMonitor("Unloads");
            await RunAndMonitor("UnloadTakesMoreTime");
        }

        private static async Task RunAndMonitor(string name)
        {
            WeakReference alc = LoadAndUnload(name);
            await Task.Delay(5000);

            uint iters = 0;
            // Iterates until WeakReference is not alive
            // Each iteration calls GC to speed up the process
            while (alc.IsAlive)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Console.WriteLine($"{name} is still alive. #iters: {iters++}");
                await Task.Delay(1000);
            }

            Console.WriteLine($"{name} was detached successfully.");
        }

        // Just loads the plugin into different AssemblyLoadContext
        // and then calls DoJob on IPlugin instance found in the plugin assembly.
		// Finishes with unloading ALC and storing it in WeakReference that is also returned
        private static WeakReference LoadAndUnload(string name)
        {
            string path = Path.GetFullPath($"Plugins/{name}/{name}.dll");

            AssemblyLoadContext alc = new AssemblyLoadContext(name, true);

            // Not handling any exceptions or nulls as this is just an example
            Assembly assembly = alc.LoadFromAssemblyPath(path);
            
            // Find type implementing IPlugin
            Type pluginType = assembly.ExportedTypes.First(x => x.IsAssignableTo(typeof(IPlugin)));
            IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginType);
            
            plugin.DoJob();
            plugin = null; // Instantly get rid of the reference just to be sure

            WeakReference alcWeak = new WeakReference(alc); // Store weak reference so we can monitor unloading process
            alc.Unload();

            return alcWeak;
        }
    }
}