namespace Utils

open System
open System.Runtime.CompilerServices

[<AutoOpen>]
[<Extension>]
module TimeSpanExtensions =
    [<Extension>]
    let Square (time : TimeSpan) =
        TimeSpan.FromTicks(time.Ticks * time.Ticks)
