namespace LinearAlgebra

open Utils

[<Sealed>]
type SparseMatrix(values : Map<row * col, value>) =
    inherit AbstractMatrix()

    let items = 
        values
        |> Map.toSeq
        |> Seq.map (fun ((row, col), value) -> (row, col, value))
        |> Seq.toArray

    member this.Values : (row * col * value) array = items


    override this.GetValue (row : row, col : col) =
        values.TryFind((row, col)).OrElse(0)

    member this.Trace() =
        values
        |> Map.toSeq
        |> Seq.where (fun ((row, col), value) -> row = col)
        |> Seq.sumBy (fun ((row, col), value) -> value)

    member this.Mul (other : SparseMatrix) =
        let mutable newMap = Map.empty
        for (row1, col1, value1) in this.Values do
            for (row2, col2, value2) in other.Values do
                if col1 = row2 then
                    let newValue = newMap.TryFind((row1, col2)).OrElse(0) + value1 * value2
                    newMap <- newMap.Add((row1, col2), newValue)

        SparseMatrix newMap


