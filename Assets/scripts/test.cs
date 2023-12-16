using System.Collections;
using System.Collections.Generic;
public class Djikstra
{
    private HashSet<(int,int,int)> GetPathSet()
    {
        // Pick a arb starting point
        HashSet<(int, int, int)> visited = new HashSet<(int,int,int)>();
        HashSet<(int,int,int)> targetCoords = new HashSet<(int,int,int)>();
        Dictionary<(int,int,int), (int,int,int)> previousNode = new Dictionary<(int,int,int), (int,int,int)>();
        Dictionary<(int,int,int), double> costs = new Dictionary<(int,int,int), double>();
        BinaryHeap<WeightedCoordinate> heapq;

        WeightedCoordinate startingCoord = new WeightedCoordinate(0.0, 0,0,0);
        WeightedCoordinate currentCoord;

        WeightedCoordinate targetCoord = new WeightedCoordinate(double.PositiveInfinity, 0,0,0);

        bool is_starting_coord = true;

        foreach ((int x, int y, int z) in randomEntranceCoordinates)
        {
            if (is_starting_coord) {
                startingCoord = new WeightedCoordinate(0.0, x, y, z);
                costs.Add((startingCoord.X, startingCoord.Y, startingCoord.Z), 0.0);
                is_starting_coord = false;
                continue;
            }
            targetCoords.Add((x,y,z));
            heapq = new BinaryHeap<WeightedCoordinate>();
            
            heapq.Add(startingCoord);

            while (heapq.IsEmpty() == false)
            {
                currentCoord = heapq.Remove();
                if (visited.Contains((currentCoord.X, currentCoord.Y, currentCoord.Z)))
                {
                    continue;
                }
                visited.Add((currentCoord.X, currentCoord.Y, currentCoord.Z));

                // Currently visiting target Node
                if ((currentCoord.X, currentCoord.Y, currentCoord.Z) == (x,y,z))
                {
                    break;
                }



                List<(int,int,int)> neighbors = new List<(int,int,int)> {(2,0,0), (-2,0,0), (0,0,2), (0,0,-2)};
                foreach ((int,int,int) neigbor in neighbors)
                {
                    (int,int,int) neighborCoord = (currentCoord.X + neigbor.Item1, currentCoord.Y + neigbor.Item2, currentCoord.Z + neigbor.Item3);
                    
                    // Out of Bounds neighbors
                    if (neighborCoord.Item1 < 2 || 
                        neighborCoord.Item1 > (mapWidth-1) * (tileWidthHeight) ||
                        neighborCoord.Item3 < 2 || 
                        neighborCoord.Item3 > (mapHeight-1) * tileWidthHeight)
                    {
                        continue;
                    }

                    // Update weights
                    double currentEdgeCost = visited.Contains(neighborCoord) ? 0 : 2;
                    if (costs.ContainsKey(neighborCoord))
                    {
                        if (costs[neighborCoord] > currentCoord.Weight + currentEdgeCost)
                        {
                            costs[neighborCoord] = currentCoord.Weight + currentEdgeCost;
                            previousNode[neighborCoord] = (currentCoord.X, currentCoord.Y, currentCoord.Z);
                        }
                    }
                    else
                    {
                        costs.Add(neighborCoord, currentCoord.Weight + currentEdgeCost);
                        previousNode.Add(neighborCoord, (currentCoord.X, currentCoord.Y, currentCoord.Z));
                    }

                    // Adding neighbors to heap
                    if (visited.Contains(neighborCoord) == false)
                    {
                        WeightedCoordinate neighborNode = new WeightedCoordinate(costs[neighborCoord], neighborCoord.Item1, neighborCoord.Item2, neighborCoord.Item3);
                        heapq.Add(neighborNode);
                    }

                }


            }
            // Debug.Log(previousNode.ToString());
            foreach (KeyValuePair<(int,int,int),(int,int,int)> entry in previousNode)
            {
                Debug.Log($"{entry.Key}, {entry.Value}");
            }

        }

        // return paths using set of coords
        HashSet<(int,int,int)> pathCoords = new HashSet<(int,int,int)>();
        foreach ((int,int,int) coord in targetCoords)
        {
            pathCoords.Add(coord);
            (int,int,int) currCoord = coord;
            while (previousNode.ContainsKey(currCoord))
            {
                pathCoords.Add(previousNode[currCoord]);
                currCoord = previousNode[currCoord];
            }
        }

        return pathCoords;
    }
}




public class WeightedCoordinate : IComparable<WeightedCoordinate>
{
    public double Weight { get; }
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public WeightedCoordinate(double weight, int x, int y, int z)
    {
        Weight = weight;
        X = x;
        Y = y;
        Z = z;
    }

    // Implement the IComparable<WeightedCoordinate> interface
    public int CompareTo(WeightedCoordinate other)
    {
        // Compare based on weight only
        return Weight.CompareTo(other.Weight);
    }

    // Optionally override ToString for better readability
    public override string ToString()
    {
        return $"Weight: {Weight}, X: {X}, Y: {Y}, Z: {Z}";
    }
}

