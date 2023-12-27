# This year I'm doing C# because I looked at the job market and saw that C# is very prevalent, and I am a bit rusty with it.
Every problem was solved for less than 100 lines of code, while prioritizing readability over compactness.
Overall I had fun with a lot of the problems when they required a really interesting insight to solve.
The most important techniques for cleaner code which I used were both about navinating in 2D (or 3D) grids. They are:
1. If you wish to try moving up, down, left and right, then instead of writing similar code 4 times, write something like this:
`foreach (var (dy, dx) in new List<int>([(0, 1), (0, -1), (1, 0), (-1, 0)])`
Then use y + dy and x + dx as the new coordinates in each iteration.
2. When you are accessing the grid, you need to check whether you're outside of it, which amounts to 4 if statements (to check that y, x are from 0 until the length).
Instead of writing the 4 statements every time, write a function like my `Helpers.SafeGet` which returns the item at the position, or null if it doesn't exist.
The function can be implemented by catching an IndexOutOfRangeException and returning null in that case.

# Verdict on C#:
My opinion of C# got somewhat worse after this. 
The summary is that it often has both good and bad ways of doing things, because of this it's a mess, and sometimes it only has mildly bad ways.
It also has some bizarre rules which aren't present in other languages that I've used, which seem unnecessary / get in the way in my opinion. Or it does things in its own specific way which is generally not as good as the standard way.
Sometimes code ends up more verbose because the C# approach requires some boilerplate compared to what I'd do in Scala. Also sometimes you have to roll your own because the standard libraries aren't that good.
C# has plenty of syntactical sugar, but a lot of it only works in the most basic use case, and it fails if there is any complication whatsoever. For me the most important question is how it does in the majority of cases, and the answer is that it's good a lot less than half of the time.
I'm comparing C# with Scala, my favorite language. Scala doesn't have any of the listed problems / doesn't lack any of the features I'm complaining about being missing.
More specifically...

### Syntactical sugar and things that make the code more verbose:
- C# has destructuring, which is convenient, however it only works in variable assignment. It doesn't work in lambdas, which means that if I have List<(int, int)> and I call .Select on it, then I have to add an extra line to destructure, if I want to use separate variables for the members of the tuple. This often prevents me from having one-liners, they become a whole expression in { } braces, with the first line being the destructuring.
- In C#, you can use [] as a syntactical sugar for initializing a collection, but only if you are assigning it to a variable. You can't use it in a foreach loop, you have to write `foreach (int x in new List<int>([1, 3, 4]))`. (Scala doesn't have [], but it doesn't need it)
- C# has pattern matching, which is great, but it only works if each of the patterns returns a value. A lot of the time I need to do something with side-effects instead. As a result, my code uses pattern matching only sometimes when it needs it, and if statements if there are side-effects, which is an inconsistent style.
- C# has record types, such as `record Coords(X: int, Y: int);`, which is a neat way to store only those fields, but they are immutable. If I need it to be mutable, I either have to use `struct` (sometimes I need a class), or I need to do things the verbose way.
- Every time when I call any higher-order function such as .Select, .Where, .OrderBy, etc., I have to call .ToList() (or .ToArray() or whatever) at the end, which is presumably done for performance reasons like Scala's .view, but in the vast majority of the cases, either it isn't giving me extra performance because I am not chaining them, or it is but I don't need it because my sample size is tiny, and I'd rather take the reduced verbosity. I much prefer Scala's approach where I call .view in the tiny number of cases where it is needed, and in the other cases, the function returns the type it gets, e.g. `List<int> ls2 = ls1.Select(x => x * 2);` if C# were more like Scala.

### Weird complicated things:
- I'm not a fan of functions with `out` type, when you can just return your outputs as a tuple. Sadly, that is what you have to use if you want to try to get something from a Dictionary, with a default fallback in case the key is missing.
- `default` is not a great thing to use when you also have nullable types. Sometimes the libraries use default in place of nullables, but that is not adequate, e.g. if you want to get the first even number in a list of integers, then you use list.FirstOrDefault(isEven), then if nothing is found you get 0, but if the first result is 0 then you also get 0! There is no .FirstOrNull function.
- Jagged arrays lack most useful methods like .Select, .Where, .Sum, or even .ToList. You can use a LINQ statement to achieve similar results. Also you can't use a jagged array as an argument in place of a regular array of the same dimension, even though the compiler knows that they are the same! You have to write the same function again just for the jagged variant. Basically don't use them unless you really need that extra bit of performance.
- C# has ranges, like `2..9`, but they have almost no methods on them, e.g. `.Length` and `.ToList` are badly needed. However, `Range.Enumerable(2, 9)` has all that, which is a totally separate thing. I wish there was only one of them.
- LINQ seems unnecessary, given that we also have higher-order functions like .Select and .Where. Perhaps it is still useful for writing SQL, but if so, then why not restrict LINQ to only those cases? (It reminds me of Scala's for comprehensions and Haskell's do notation, but the latter both seem a lot more readable to me, or maybe I'm just wrong because I haven't used LINQ that much)
- C# has both properties and accessors, and I don't think it's worth having both, or why not allow some arguments in constructors to be an accessor, like in `record`, instead of making records be an all-or-nothing thing.
- In Helpers.SafeGet, I'm allowed to declare my output type as nullable, but I'm not allowed to return null, and anyone using the method is receiving a non-nullable output, and I have no idea why this is the case, but it for sure is very confusing and one way to have more bugs in your program.
- The only way to declare constants is to have them top-level. Local constants aren't allowed, they're quite useful so I dislike that design decision, I think it's a case of C# having bizarre overcomplicated harmful rules.
- In a lambda in a struct, you can't reference any of the struct's fields. However, you are perfectly allowed to put the field in a local variable just outside of the struct and then reference the local variable.
- Sometimes you can't declare a variable because another variable with the same name exists in a nearby scope - however, it is not in the same scope - in your current scope, you can't access the other variable. While it makes me think about whether I really need to use the same name, sometimes I really do, for example when there is one `x`, `y`, `z`, it's clear from context which one it is. This isn't really a thing in other languages as far as I know.

### Misc complaints:
- For some reason, some things were many orders of magnitude slower when I ran the debugger than without it. This could be also a problem in all other languages, not sure.
- In day 24 part 2, I used a library called Z3, and if I declared `var x1 = items[0].pos[0];` and then I referenced `x1` then my code worked, but if I directly referenced `items[0].pos[0]` then the code crashed, so I was forced to declare 18 variables just for a single use. I appreciate that this library is doing something extraordinarily meta and they're getting an expression, so they have access to the entire AST which I'm sending, however I wish I could tell it to eagerly evaluate a part of my expression, then it'd still all work out.
- By default, in C# the stack size is small, which is pretty standard. From other languages, if I wanted to do deep recursion I knew that I had to increase the stack size which ought to be very simple, however in C# the way to do that was by adding a config statement which had a hardcoded full path (it had "C:/" and "2017" in it), which can't be portable; guaranteed not to be for Linux users. I was told that I could run my code in a separate thread (with a custom stack size) in just 4 lines, but I ended up rewriting my code to be iterative.
- If you want a Dictionary with a custom key, the StackOverflow answer often tells you to write your own hash function! Even if the key is `List<string>`!
- Because of all of my complaints about nullable types, I wish that C# implemented optionals in a different way, such as Optional<A>. This way, I could have an optional of anything, and I generally won't need to worry about null semantics.

I estimate that at least 10% of my complaints are solvable by a C# feature that I haven't heard of.
 
## Good things about C#:
- It has a good variety of higher-order functions, such as .Select, .Where, .OrderBy, .Aggregate.
- It is statically typed, which I believe is very important. Also relatedly, it is very fast.
- It's great that it has nullable types, although it's not straightforward to declare a generic nullable, you'd have to know to declare that it's not a struct.
- The debugger in VS is nice, it takes you straight to the exception and it usually shows you the values of most of the variables.
- Installing libraries with NuGet is very easy
- Parallelism was really easy to implement whenever I did it: just call .AsParallel() and replace the shared Dictionary with ConcurrentDictionary and everything works like before
- A lot of its standard libraries were pretty decent, they just felt bad because I was comparing them to Scala.
- Looking back at the code I wrote, it's pretty readable and the language-limited issues are quite small.
