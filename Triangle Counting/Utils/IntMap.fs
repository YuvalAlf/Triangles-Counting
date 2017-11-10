namespace Utils

open System.Collections.Generic

type IntMap<'value>(tree : RBTree<int, 'value>) =
    static member Comparer = LanguagePrimitives.FastGenericComparer<int>
    static member Empty = IntMap(RBTree<int, 'value>.Empty(IntMap<'value>.Comparer))
    member x.AddOrUpdate (key : int, value : 'value) = IntMap<'value>(tree.AddOrUpdate(key, value))
    member x.Remove (key : int) = IntMap<'value>(tree.Remove(key))
    member x.EnumerateFields() = tree.EnumerateFields()
    member x.TryFind key = tree.TryFind(IntMap<'value>.Comparer, key)
    member x.ContainsKey key = tree.ContainsKey key
    member x.Count = tree.Count
    static member OfSortedArray array = IntMap<'value>(RBTree<int, 'value>.CreateOfSortedArray (IntMap<'value>.Comparer, array))
    static member MergeArray (array1 : (int * 'value) array) (array2 : (int * 'value) array) =
        let len = array1.Length + array2.Length
        let mergedArray = Array.zeroCreate (len)
        let rec fillArray index1 index2 indexMerged =
            if index1 >= array1.Length && index2 >= array2.Length then
                mergedArray
            elif index1 >= array1.Length then
                mergedArray.[indexMerged] <- array2.[index2]
                fillArray index1 (index2 + 1) (indexMerged + 1)
            elif index2 >= array2.Length then
                mergedArray.[indexMerged] <- array1.[index1]
                fillArray (index1 + 1) index2 (indexMerged + 1)
            elif fst array1.[index1] < fst array2.[index2] then
                mergedArray.[indexMerged] <- array1.[index1]
                fillArray (index1 + 1) index2 (indexMerged + 1)
            else
                mergedArray.[indexMerged] <- array2.[index2]
                fillArray index1 (index2 + 1) (indexMerged + 1)
        fillArray 0 0 0
