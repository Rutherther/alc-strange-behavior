This repository contains solution along with a few projects that demonstrate strange behavior (by that I mean I don't understand why this happens).

The problem is that if I use `ToImmutableList` in my plugin attached using collectible `AssemblyLoadContext` then after Unloading the ALC the plugin won't detach for a long time. I came across this behavior in my application that is a lot complicated than this project. The only thing preventing plugins from unloading fast was this thing.

With `ToImmutableList` call the process of unloading takes (for Debug build) at least 49 calls to GC.Collect with 1 second intervals. It sometimes took me even 420 iterations!
In Release it took 21 iterations (tested only once).

With the other project that uses `ToImmutableArray` unloading takes only 2 calls to GC.Collect with 1 second intervals.
