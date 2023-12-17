// using System.Collections;
// using System.Collections.Generic;
// using System;


// public class Djikstra
// {
//     private HashSet<(int,int,int)> GetPathSet()
//     {
//         (int,int,int) startCoordinate = (0,0,0);
//         HashSet<(int,int,int)> endCoordinates = new HashSet<(int,int,int)>();
//         HashSet<(int,int,int)> traveled = new HashSet<(int,int,int)>();

//         bool is_first = true;
//         foreach ((int,int,int) coordinate in randomEntranceCoordinates)
//         {
//             if (is_first) {
//                 startCoordinate = coordinate;
//                 is_first = false;
//                 traveled.Add(coordinate);
//             }
//             else {
//                 endCoordinates.Add(coordinate);
//                 traveled.Add(coordinate);
//             }
//         }

//         Dictionary<(int,int,int),(int,int,int)> previousNode = new Dictionary<(int,int,int), (int,int,int)>();
//         foreach ((int,int,int) coordinate in endCoordinates) 
//         {
//             // Run djikstra
//             BinaryHeap<WeightedCoordinate> heap = new BinaryHeap<WeightedCoordinate>();
//             HashSet<(int,int,int)> visited = new HashSet<(int,int,int)>();
//             Dictionary<(int,int,int), double> distances = new Dictionary<(int,int,int), double>();

//             WeightedCoordinate startNode = new WeightedCoordinate(0.0, startCoordinate);
//             heap.Add(startNode);

//             while (heap.IsEmpty() == false)
//             {
//                 WeightedCoordinate currentNode = heap.Remove();
//                 // Already visited
//                 if (visited.Contains(currentNode.Coordinate)) {continue;}
//                 visited.Add(currentNode.Coordinate);

//                 // Neighbors
//                 List<(int,int,int)> neighborOffsets = new List<(int,int,int)> {(-2,0,0), (2,0,0), (0,0,2), (0,0,-2)};
//                 foreach ((int,int,int) neighborOffset in neighborOffsets)
//                 {
//                     // alligning new
//                     (int,int,int) neighborCoordinate = (currentNode.Coordinate.Item1 + neighborOffset.Item1, 
//                                                         currentNode.Coordinate.Item2 + neighborOffset.Item2, 
//                                                         currentNode.Coordinate.Item3 + neighborOffset.Item3);

//                     // Check not visited
//                     if (visited.Contains(neighborCoordinate)) {continue;}

//                     // Update distance
//                     double edgeCost = (traveled.Contains(neighborCoordinate)) ? 0 : 2;
//                     if (distances.ContainsKey(neighborCoordinate)) 
//                     {
//                         if (distances[neighborCoordinate] > currentNode.Weight + edgeCost)
//                         {
//                             distances[neighborCoordinate] = currentNode.Weight + edgeCost;
//                             previousNode[neighborCoordinate] = currentNode.Coordinate;
//                         }
//                     }
//                     else
//                     {
//                         distances.Add(neighborCoordinate, currentNode.Weight + edgeCost);
//                         previousNode.Add(neighborCoordinate, currentNode.Coordinate);
//                     }

//                     // Add neighbor to heap
//                     WeightedCoordinate neighborNode = new WeightedCoordinate(distances[neighborCoordinate], neighborCoordinate);
//                     heap.Add(neighborNode);
//                 }
//             }            

//         }

//         HashSet<(int,int,int)> pathTileCoordinates = new HashSet<(int,int,int)>();
//         (int,int,int) currentCoordinate;
//         foreach ((int,int,int) endCoordinate in endCoordinates)
//         {
//             currentCoordinate = endCoordinate;
//             pathTileCoordinates.Add(currentCoordinate);
//             while (previousNode.ContainsKey(currentCoordinate))
//             {
//                 currentCoordinate = previousNode[currentCoordinate];
//                 pathTileCoordinates.Add(currentCoordinate);
//             }
//         }
//         return pathTileCoordinates;
//     }
// }



// public class WeightedCoordinate : IComparable<WeightedCoordinate>
// {
//     public double Weight { get; }
//     public (int,int,int) Coordinate { get; }

//     public WeightedCoordinate(double weight, (int,int,int) coordinate)
//     {
//         Weight = weight;
//         Coordinate = coordinate;
//     }

//     // Implement the IComparable<WeightedCoordinate> interface
//     public int CompareTo(WeightedCoordinate other)
//     {
//         // Compare based on weight only
//         return Weight.CompareTo(other.Weight);
//     }

//     // Optionally override ToString for better readability
//     public override string ToString()
//     {
//         return $"Weight: {Weight}, X: {Coordinate.Item1}, Y: {Coordinate.Item2}, Z: {Coordinate.Item3}";
//     }
// }
