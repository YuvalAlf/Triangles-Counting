
namespace PersistentTriangleCounting

open LinearAlgebra


type PersistentCounter(graph : PersistentGraph, numOfTriangles : int) =
    member this.NumOfTriangles = numOfTriangles
    member this.Graph = graph
    static member Create() = PersistentCounter(PersistentGraph.EmptyGraph, 0)
    member this.AddEdge(node1, node2) =
        if graph.ContainsEdge(node1, node2) then
            this
        else
            let minAdjacentNode = [node1; node2] |> List.minBy (fun node -> graph.GetNode(node).Count)
            let otherNode = if minAdjacentNode = node1 then node1 else node2
            let addedTriangles =
                graph.GetNode(minAdjacentNode)
                |> Set.toSeq
                |> Seq.where (fun node -> graph.ContainsEdge(node, otherNode))
                |> Seq.length

            let newGraph = graph.AddEdge(node1, node2).AddEdge(node2, node1)
            PersistentCounter(newGraph, numOfTriangles + addedTriangles)
    member this.RemoveEdge(node1, node2) =
        if not <| graph.ContainsEdge(node1, node2) then
            this
        else
            let minAdjacentNode = [node1; node2] |> List.minBy (fun node -> graph.GetNode(node).Count)
            let otherNode = if minAdjacentNode = node1 then node1 else node2
            let removedTriangles =
                graph.GetNode(minAdjacentNode)
                |> Set.toSeq
                |> Seq.filter (fun node -> graph.ContainsEdge(node, otherNode))
                |> Seq.length

            let newGraph = graph.RemoveEdge(node1, node2).RemoveEdge(node2, node1)
            PersistentCounter(newGraph, numOfTriangles + removedTriangles)
