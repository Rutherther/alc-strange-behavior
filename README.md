# The problem (pooling)
Turns out this is not anything strange at all.
The problem is that `ImmutableList.Enumerator` uses pooling.
It is shared for all plugins by default
That means that if there is an object that is part of the plugin,
it will be stored in this pool for quite some time. For that time
the plugin cannot be unloaded as there is a reference to object
that is part of the plugin.

The minimum to reproduce is to create `ImmutableList` storing at least one
element that is loaded in the same AssemblyLoadContext as plugin is.
Then getting enumerator of this list and disposing it (that will add it to
pool).

To my knowledge it is not possible to clear the pool yourself as it is
`private` in `Enumerator` of `ImmutableList` (could be possible using
reflection).

One possible workaround would be to load `System.Collections.Immutable.dll`
for each plugin separately by overriding `AssemblyLoadContext.Load` and
loading it yourself. That way there wouldn't be reference from the default
`AssemblyLoadContext` to the plugin's ALC.

Another solution is to not use plugin types in `ImmutableList`, use only
those that are shared throughout the whole application and not just
a part of plugin.

# OUTDATED
~~This repository contains solution along with a few projects that demonstrate strange behavior (by that I mean I don't understand why this happens).~~

~~The problem is that if I use `ToImmutableList` in my plugin attached using collectible `AssemblyLoadContext` then after Unloading the ALC the plugin won't detach for a long time. I came across this behavior in my application that is a lot complicated than this project. The only thing preventing plugins from unloading fast was this thing.~~

~~With `ToImmutableList` call the process of unloading takes (for Debug build) at least 49 calls to GC.Collect with 1 second intervals. It sometimes took me even 420 iterations!~~
~~In Release it took 21 iterations (tested only once).

~~With the other project that uses `ToImmutableArray` unloading takes only 2 calls to GC.Collect with 1 second intervals.~~
