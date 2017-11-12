namespace PersistentTriangleCounting

open FSharpUtils

type node = int

[<Sealed>]
type PersistentGraph(nodes : Map<node, Set<node>>) =
    static member EmptyNode : int Set = Set.empty
    static member EmptyGraph = PersistentGraph(Map.empty)
    member this.Nodes = nodes
    member this.GetNode(node) = nodes.TryFind(node).OrElse(PersistentGraph.EmptyNode)
    
    // O(log n)
    member this.ContainsEdge(node1, node2) =
        this.GetNode(node1).Contains(node2)

    // O(log n)
    member this.RemoveEdge(node1, node2) =
        let newNode1 = this.GetNode(node1).Remove(node2)
        if newNode1.Count = 0 then
            PersistentGraph(nodes.Remove(node1))
        else
            PersistentGraph(nodes.Add(node1, newNode1))

    // O(log n)
    member this.AddEdge(node1, node2) =
        let newNode1 = this.GetNode(node1).Add(node2)
        PersistentGraph(nodes.Add(node1, newNode1))
    
