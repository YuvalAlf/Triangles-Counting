namespace Utils

open System.Collections.Generic

type color = Red | Black | BB | NB

type RemoveOperation = Removed | DidntRemove
type AddOperation = Added | Updated

[<NoEquality; NoComparison>]
type Tree<'key, 'value> = 
    | Empty // black leaf
    | EEmpty // double black leaf
    | Node of color * Tree<'key, 'value> * 'key * 'value * Tree<'key, 'value>

module RedBlackTree =

    let toString (x:'a) = 
        match Microsoft.FSharp.Reflection.FSharpValue.GetUnionFields(x, typeof<'a>) with | case, _ -> case.Name

    let hd = function 
        | Node(c, l, k, v, r) -> k
        | _ -> failwith "empty"

    let redden = function
        | Empty | EEmpty -> failwith "cannot redden empty"
        | Node(Red, _, _, _, _) as r -> r
        | Node(_, l, k, v, r ) -> Node(Red, l, k, v, r)

    // blacken' for insert
    let blacken' = function
        | Empty | EEmpty -> failwith "cannot blacken' empty"
        | Node(Black, _, _, _, _) as node -> node
        | Node(_, l, k, v, r) -> Node(Black, l, k, v, r)
    // blacken for delete
    let blacken = function
        | Node(Black, _, _, _, _) as node -> node
        | Node(BB, l, k, v, r) as node -> Node(Black, l, k, v, r)
        | Empty -> Empty
        | EEmpty -> Empty
        | _ -> failwith "cannot blacken Red"

    let isBB = function
        | EEmpty -> true
        | Node(BB, _, _, _, _) -> true
        | _ -> false

    let blacker = function
        | NB -> Red
        | Red -> Black
        | Black -> BB
        | BB -> failwith "cannot blacker, is double black already"

    let redder = function
        | NB -> failwith "cannot redder, not black enough"
        | Red -> NB
        | Black -> Red
        | BB -> Black

    let blacker' = function
        | Empty -> EEmpty
        | Node(c, l, k, v, r) -> Node(blacker c, l, k, v, r)
        | _ as s -> failwith ("cant blacker' " + (toString s))

    let redder' = function
        | EEmpty -> Empty
        | Node(c, l, k, v, r) -> Node(redder c, l, k, v, r)
        | _ as s -> failwith ("cant redder' " + (toString s))

    let rec balance = function
            //Okasaki's original cases:
            | (Black, Node(Red, Node(Red, a, x, xv, b), y, yv, c), z, zv, d)
            | (Black, Node(Red, a, x, xv, Node(Red, b, y, yv, c)), z, zv, d)
            | (Black, a, x, xv, Node(Red, Node(Red, b, y, yv, c), z, zv, d))
            | (Black, a, x, xv, Node(Red, b, y, yv, Node(Red, c, z, zv, d)))
                -> Node(Red, Node(Black, a, x, xv, b), y, yv, Node(Black, c, z, zv, d))
            //Six cases for deletion:
            | (BB, Node(Red, Node(Red, a, x, xv, b), y, yv, c), z, zv, d ) -> Node(Black, Node(Black, a, x, xv, b), y, yv, Node(Black, c, z, zv, d))
            | (BB, Node(Red, a, x, xv, Node(Red, b, y, yv, c)), z, zv, d) -> Node(Black, Node(Black, a, x, xv, b), y, yv, Node(Black, c, z, zv, d))
            | (BB, a, x, xv, Node(Red, Node(Red, b, y, yv, c), z, zv, d)) -> Node(Black, Node(Black, a, x, xv, b), y, yv, Node(Black, c, z, zv, d))
            | (BB, a, x, xv, Node(Red, b, y, yv, Node(Red, c, z, zv, d))) -> Node(Black, Node(Black, a, x, xv, b), y, yv, Node(Black, c, z, zv, d))
            //
            | (BB, a, x, xv, Node(NB, Node(Black, b, y, yv, c), z, zv, (Node(Black, _, _, _, _) as d))) ->
                Node(Black, Node(Black, a, x, xv, b), y, yv, (balance (Black, c,  z, zv, (redden d))))
            | (BB, Node(NB, (Node(Black, _, _, _, _) as a), x, xv, Node(Black, b, y, yv, c)), z, zv, d) ->
                Node(Black, balance (Black, redden a, x, xv, b), y, yv, Node(Black, c, z, zv, d))
            | (c, l, x, xv, r) -> Node(c, l, x, xv, r)

    let bubble color l k v r = 
        match isBB(l) || isBB(r) with
        | true -> balance ((blacker color), (redder' l), k, v, (redder' r))
        | _ -> balance (color, l, k, v, r)

    let empty = Empty

    let rec tryFind (comparer: IComparer<'key>) (key : 'key) (tree : Tree<'key, 'value>) : 'value option =
        match tree with
        | Empty -> None
        | Node(_, l, k, v, r) ->
            let comp = comparer.Compare(key, k)
            if   comp = 0 then Some(v)
            elif comp < 0 then tryFind comparer key l
            else               tryFind comparer key r 
        | _ as s -> failwith ("cant lookup " + (toString s))

    let addOrUpdate (comparer: IComparer<'key>) (key : 'key) (value : 'value) (tree : Tree<'key, 'value>) =
        let rec ins = function
            | Empty -> (Added, Node(Red, Empty, key, value, Empty))
            | Node(color, a, y, yv, b) as s ->
                let comp = comparer.Compare(key, y)
                if comp < 0 then let op, lt = ins a in (op, balance (color, lt, y, yv, b))
                elif comp > 0 then let op, rt = ins b in (op, balance (color, a, y, yv, rt))
                else (Updated, Node(color, a, key, value, b))
            | _ as s -> failwith ("cant insert " + (toString key))
        in let (op, resTree) = ins tree in (op, blacken' resTree)

    let rec max = function
        | Empty -> failwith "no largest element"
        | Node(_, _, k, v, Empty) -> (k, v)
        | Node(_, _, _, _, r) -> max r
        | _ as s -> failwith ("cant max " + (toString s))

    let rec removeMax = function
        | Empty -> failwith "no maximum to remove"
        | Node(_, _, _, _, Empty) as s -> remove s
        | Node(color, l, k, v, r)-> bubble color l k v (removeMax r)
        | _ as s -> failwith ("cant removeMax " + (toString s))

    and remove = function
        | Empty -> Empty
        | Node(Red, Empty, _, _, Empty) -> Empty
        | Node(Black, Empty, _, _, Empty) -> EEmpty
        | Node(Black, Empty, _, _, Node(Red, a, k, v, b)) -> Node(Black, a, k, v, b)
        | Node(Black, Node(Red, a, k, v, b), _, _, Empty) -> Node(Black, a, k, v, b)
        | Node(color, l, k, v, r) -> let maxKey, maxVal = max l in bubble color (removeMax l) maxKey maxVal r
        | _ as s -> failwith ("cant remove case" + (toString s))

    let delete (comparer: IComparer<'key>) (k : 'key) (tree : Tree<'key, 'value>) = 
        let rec del = function
            | Empty -> (DidntRemove, Empty)
            | Node(c, a, y, yz, b) as s ->
                let comp = comparer.Compare(k, y)
                if comp < 0 then let op, lt = del a in (op, bubble c lt y yz b)
                elif comp > 0 then let op, rt = del b in (op, bubble c a y yz rt)
                else (Removed, remove s)
            | _ as s -> failwith ("cant delete " + (toString k))
        in let op, resTree = del tree in (op, blacken(resTree))

    let rec count = function
        | Node(_, l, _, _, r) -> 1 + count l + count r
        | _ -> 0

    let rec exist (comparer: IComparer<'key>) item tree =
        match tryFind comparer item tree with
        | None -> false
        | _ -> true
  
    let rec iterKeys f = function
        | Node(_, l, k, _, r) -> iterKeys f l ; f k ; iterKeys f r
        | _ -> ()

    let rec iterValues f = function
        | Node(_, l, _, v, r) -> iterKeys f l ; f v ; iterKeys f r
        | _ -> ()

    let inverseColor = function
        | Red   -> Black
        | Black -> Red
        | _     -> failwith "Error!"

    let createTreeOfSortedArray (sortedArray : ('key * 'value) array) =
        let rec create (startIndex : int) (endIndex : int) (color : color) =
            if startIndex > endIndex then
                Tree.Empty
            else
                let middle = (endIndex + startIndex) / 2
                let leftTree = create (startIndex) (middle - 1) (inverseColor color)
                let rightTree = create (middle + 1) (endIndex) (inverseColor color)
                Tree.Node(color, leftTree, fst sortedArray.[middle], snd sortedArray.[middle], rightTree)
        create (0) (sortedArray.Length - 1) (Black)

    let rec enumerateFields tree = seq {
            match tree with
            | Node(_, l, k, v, r) ->
                yield (k, v)
                yield! enumerateFields l
                yield! enumerateFields r
            | _ -> ignore()
        }

[<Sealed>]
type RBTree<'key, 'value>(comparer : IComparer<'key>, tree : Tree<'key, 'value>, count) =
    static member Empty(comparer) : RBTree<'key, 'value> = new RBTree<'key, 'value>(comparer , Empty, 0)
    static member CreateOfSortedArray (comparer, array : ('key * 'value) array) = 
         new RBTree<'key, 'value>(comparer , RedBlackTree.createTreeOfSortedArray array, array.Length)

    member s.AddOrUpdate(key, value) : RBTree<'key, 'value> = 
        match RedBlackTree.addOrUpdate comparer key value tree with
        | Updated, newTree -> new RBTree<'key, 'value>(comparer, newTree, count)
        | Added, newTree -> new RBTree<'key, 'value>(comparer, newTree, count+1)
    member s.Remove key : RBTree<'key, 'value> = 
        match RedBlackTree.delete comparer key tree with
        | Removed, newTree -> new RBTree<'key, 'value>(comparer, newTree, count-1)
        | DidntRemove, newTree -> new RBTree<'key, 'value>(comparer, newTree, count)

    member s.Count = count
    member s.ContainsKey(key) = RedBlackTree.exist comparer key tree
    member s.TryFind(comparer, key) = RedBlackTree.tryFind comparer key tree
    member s.EnumerateFields() = RedBlackTree.enumerateFields tree