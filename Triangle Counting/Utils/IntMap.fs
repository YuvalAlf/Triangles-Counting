namespace Utils

open System.Collections.Generic

type IntMap<'value>(tree : RBTree<int, 'value>) =
    static member Comparer = LanguagePrimitives.FastGenericComparer<int>
    static member Empty = IntMap(RBTree<int, 'value>.Empty(IntMap<'value>.Comparer))
    member x.AddOrUpdate (key : int, value : 'value) = IntMap<'value>(tree.AddOrUpdate(key, value))
    member x.Remove (key : int) = IntMap<'value>(tree.Remove(key))
    member x.EnumerateFields() = tree.EnumerateFields()
    member x.TryFind key = tree.TryFind(IntMap<'value>.Comparer, key)
    member x.Count() = tree.Count()
    static member OfSortedArray array = RBTree<int, 'value>.CreateOfSortedArray (IntMap<'value>.Comparer, array)
