namespace LinearAlgebra

open Utils

type 'value IntMap = Map<int, 'value>

[<Sealed>]
type PersistentMatrix(cols : IntMap<IntMap<value>>) =
    inherit AbstractMatrix()

    member this.Cols = cols
    member this.GetCol(col) = cols.TryFind(col).OrElse(Map.empty)
    

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

    // O(1)
    static member Empty =
        PersistentMatrix(Map.empty)
(*        
    // O(n) could be made O(1)
    member this.Trace() =
        cols
        |> Map.toSeq
        |> Seq.sumBy (fun (col, _) -> this.GetValue(col, col))

    // O(log n)
    member this.Add(sMatrix : SparseMatrix) =
        sMatrix.Values
        |> Array.fold(fun (mat : PersistentMatrix) (row, col, value) -> mat.AddValue(row, col, value)) this

    // O(log n + max{#_of_values in a column})
    member this.Add(pMatrix : PersistentMatrix) =
        let mutable resCols = cols
        for col, rows in pMatrix.Cols.EnumerateFields() do
            let rowsInColThis = this.GetCol(col).EnumerateFields() |> Seq.toArray
            let rowsInColpMatrix = rows.EnumerateFields() |> Seq.toArray
            let mergedArray = IntMap<int>.MergeArray rowsInColThis rowsInColpMatrix
            resCols <- resCols.AddOrUpdate(col, IntMap.OfSortedArray mergedArray)

        PersistentMatrix(resCols)

    // O(log n)
    member this.MulTrace(s : SparseMatrix) =
        let mutable trace = 0
        for (row, col, value) in s.Values do
            trace <- trace + (this.GetValue(col, row) * value)
        trace

    // O(log n + max{#_of_values in a column})
    member this.MulRight(s : SparseMatrix) =
        let mulOparation (m : PersistentMatrix) (row, col, value) : PersistentMatrix =
            let column = this.GetCol(row).MapValues(fun v -> v * value)
            let columnAsMatrix = PersistentMatrix(IntMap.Empty.AddOrUpdate(col, column))
            m.Add(columnAsMatrix)
            
        s.Values
        |> Array.fold mulOparation PersistentMatrix.Empty

    *)

