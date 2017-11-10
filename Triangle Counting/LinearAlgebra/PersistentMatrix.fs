namespace LinearAlgebra

open Utils

[<Sealed>]
type PersistentMatrix(cols : Map<col, Map<row, value>>) =
    inherit AbstractMatrix()

    member private this.GetCol col = cols.TryFind(col).OrElse(Map.empty)
    member private this.Cols = cols

    // O(log n)
    override this.GetValue(row, col) =
        this.GetCol(col).TryFind(row).OrElse(0)

    // O(log n)
    member private this.ZeroValue(row, col) =
        let newCol = this.GetCol(col).Remove(row)
        if newCol.Count = 0 then
            PersistentMatrix(cols.Remove(col))
        else
            PersistentMatrix(cols.Add(col, newCol))

    // O(log n)
    member this.SetValue(row, col, value) =
        if value = 0 then
            this.ZeroValue(row, col)
        else
            let newCols = cols.Add(col, this.GetCol(col).Add(row, value))
            PersistentMatrix(newCols)
            
    // O(log n)
    member this.AddValue(row, col, valueToAdd) =
        this.SetValue(row, col, this.GetValue(row, col) + valueToAdd)

    // O(n) could be made O(1)
    member this.Trace() =
        let mutable trace = 0
        for col in cols do
            trace <- trace + this.GetValue(col.Key, col.Key)
        trace

    // O(log n)
    member this.Add(sMatrix : SparseMatrix) =
        sMatrix.Values
        |> Array.fold(fun (mat : PersistentMatrix) (row, col, value) -> mat.AddValue(row, col, value)) this

    // O(log n + max{values in a column})
    member this.Add(pMatrix : PersistentMatrix) =
        let mutable resMatrix = this
        ()
        // THINK!!!!!!!!
    //    for col in pMatrix. do
    //        for row in col.Value do
    //            for 

        resMatrix

    // O(1)
    static member Empty =
        PersistentMatrix(Map.empty)

    

