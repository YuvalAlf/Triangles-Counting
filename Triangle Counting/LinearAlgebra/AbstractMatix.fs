namespace LinearAlgebra

open System.Text


type row = int
type col = int
type value = int

[<AbstractClass>]
type AbstractMatrix() =
    abstract member GetValue : row * col -> value
    member this.AsString(size : int) =
        let mutable str = new StringBuilder()
        for i = 0 to size - 1 do
            for j = 0 to size - 1 do
                str <- str.Append(sprintf "%d " <| this.GetValue(i,j))
            str <- str.AppendLine()
        str.ToString()

