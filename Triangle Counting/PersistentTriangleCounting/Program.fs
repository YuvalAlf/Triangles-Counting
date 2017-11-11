namespace PersistentTriangleCounting

open System.IO

module Program =
    open System

    let parseGraph filePath = seq {
            use stream = File.OpenText filePath
            while not stream.EndOfStream do
                let line = stream.ReadLine()
                if not <| line.StartsWith("#") then
                    let nodes = line.Split('\t')
                    yield (Int32.Parse nodes.[1], Int32.Parse nodes.[0])
        }

    let createGraph n m dMax (rnd : Random) =
        ()

    let benchmark alg =
        let starting = DateTime.Now
        let res = alg()
        let ending = DateTime.Now
        (res, ending - starting)
        
    //let graph1 = @"C:\Users\Yuval\Downloads\com-lj.ungraph.txt\com-lj.ungraph.txt"
    let graph2 = @"C:\Users\Yuval\Downloads\wiki-Vote.txt\Wiki-Vote.txt"
    let graph3 = @"C:\Users\Yuval\Downloads\email-Enron.txt\Email-Enron.txt"
    let graph4 = @"C:\Users\Yuval\Downloads\ca-AstroPh.txt\CA-AstroPh.txt"

    [<EntryPoint>]
    let main argv = 
        for graphPath in [graph2; graph3; graph4] do
            let graph = parseGraph graphPath
            let mutable triangleCounter = PersistentCounter.Create()
            let mutable numOfEdges = 0
            let starting = DateTime.Now
        
            for (node1, node2) in graph do
                triangleCounter <- triangleCounter.AddEdge(node1, node2)
                numOfEdges <- numOfEdges + 1
            let ending = DateTime.Now
            let totalTime = ending-starting

            printfn "# of edges: %d" (numOfEdges)
            printfn "# of triangles: %d" (triangleCounter.NumOfTriangles)
            printfn "Overall time: %f sec" (totalTime.TotalSeconds)
            printfn "Average time per operation: %f ms" (totalTime.TotalMilliseconds / double(numOfEdges))
            printfn "Max degree: %d degree" (triangleCounter.Graph.Nodes |> Map.toSeq |> Seq.maxBy (fun (_, s) -> s.Count) |> fun (_, s) -> s.Count)
            printfn "-----------------------------------"
        0
