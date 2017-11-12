namespace FSharpUtils

[<AutoOpen>]
module OptionExtensions =
    type Option<'key> with 
        member this.OrElse (value : 'key) =
            match this with
            | Some x -> x
            | None   -> value