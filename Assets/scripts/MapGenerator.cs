using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    // public char[] entrances;
    public int numNorthEntrances = 0;
    public int numEastEntrances = 0;
    public int numSouthEntrances = 0;
    public int numWestEntrances = 0;

    public int tileWidthHeight = 2;
    public int mapWidth = 100;
    public int mapHeight = 100;

    private float cornerRotationToZXAxis = 0f;
    private float edgeRotationToZXAxis = 0f;
    private float entranceRotationToZXAxis = 180f;


    Dictionary<char, HashSet<int>> randomEntranceLocation;
    HashSet<(int, int, int)> randomEntranceCoordinates = new HashSet<(int, int, int)>();
    // 0 CornerTile
    // 1 Edge Tile
    // 2 Entrance Tile

    // 3 Straight Path Tile
    // 4 Elbow Path Tile
    // 5 T Path Tile
    // 6 Grass Tile

    // Start is called before the first frame update
    void Start()
    {
        // GenerateMap();
        GenerateBoarder();
        GeneratePath();

    }

    // Update is called once per frame
    void GenerateBoarder()
    {
        // Corners
        GameObject nwCorner = Instantiate(tilePrefabs[0], new Vector3((mapWidth - 1) * tileWidthHeight,0,0), Quaternion.Euler(0f, 90f + cornerRotationToZXAxis, 0f));
        GameObject neCorner = Instantiate(tilePrefabs[0], new Vector3(0,0,0), Quaternion.Euler(0f, 180f + cornerRotationToZXAxis, 0f));
        GameObject seCorner = Instantiate(tilePrefabs[0], new Vector3(0,0,(mapHeight - 1) * tileWidthHeight), Quaternion.Euler(0f, 270f + cornerRotationToZXAxis, 0f));
        GameObject swCorner = Instantiate(tilePrefabs[0], new Vector3((mapWidth - 1) * tileWidthHeight,0,(mapHeight - 1) * tileWidthHeight), Quaternion.Euler(0f, 0f + cornerRotationToZXAxis, 0f));
        nwCorner.transform.parent = this.transform;
        neCorner.transform.parent = this.transform;
        seCorner.transform.parent = this.transform;
        swCorner.transform.parent = this.transform;

        // Entrances
        randomEntranceLocation = new Dictionary<char, HashSet<int>>();
        char[] directions = {'n','e','s','w'};
        // Random random = new Random();

        // Specify the number of unique numbers you want for each direction
        int numbersPerDirection;

        // Generate random unique numbers for each direction
        foreach (char dir in directions)
        {
            // Create a HashSet for the current direction
            HashSet<int> uniqueNumbers = new HashSet<int>();

            // Generate random unique numbers for the current direction
            switch (dir){
                case 'n':
                    numbersPerDirection = numNorthEntrances;
                    break;
                case 'e':
                    numbersPerDirection = numEastEntrances;
                    break;
                case 's':
                    numbersPerDirection = numSouthEntrances;
                    break;
                default:
                    numbersPerDirection = numWestEntrances;
                    break;
            }
            while (uniqueNumbers.Count < numbersPerDirection)
            {
                int randomNumber;
                if (dir == 'n' || dir == 's')
                {
                    randomNumber = UnityEngine.Random.Range(1,mapWidth);
                }
                else
                {
                    randomNumber = UnityEngine.Random.Range(1, mapHeight);
                }
                uniqueNumbers.Add(randomNumber * tileWidthHeight);
                switch (dir){
                    case 'n':
                        randomEntranceCoordinates.Add((randomNumber, 0, 0));
                        break;
                    case 'e':
                        randomEntranceCoordinates.Add((0, 0, randomNumber));
                        break;
                    case 's':
                        randomEntranceCoordinates.Add((randomNumber, 0, (mapHeight - 1) * tileWidthHeight));
                        break;
                    default:
                        randomEntranceCoordinates.Add(((mapWidth - 1) * tileWidthHeight, 0, randomNumber));
                        break;
                }
            }

            // Add the set of unique numbers to the dictionary
            randomEntranceLocation[dir] = uniqueNumbers;
            
        }


        for (int x  = tileWidthHeight; x < (mapWidth - 1) * tileWidthHeight; x=x+tileWidthHeight)
        {
            // NORTH AND SOUTH
            GameObject nPrefab = tilePrefabs[1];
            GameObject sPrefab = tilePrefabs[1];
            float nEdgeRotation = edgeRotationToZXAxis;
            float sEdgeRotation = edgeRotationToZXAxis;
            if (randomEntranceLocation['n'].Contains(x))
            {
                nPrefab = tilePrefabs[2];
                nEdgeRotation = entranceRotationToZXAxis;
            }
            if (randomEntranceLocation['s'].Contains(x))
            {
                sPrefab = tilePrefabs[2];
                sEdgeRotation = entranceRotationToZXAxis;
            }
            GameObject nEdge = Instantiate(nPrefab, new Vector3(x, 0, 0), Quaternion.Euler(0f, 180f + nEdgeRotation, 0f));
            GameObject sEdge = Instantiate(sPrefab, new Vector3(x, 0, (mapHeight - 1) * tileWidthHeight), Quaternion.Euler(0f, 0f + sEdgeRotation, 0f));
            nEdge.transform.parent = this.transform;
            sEdge.transform.parent = this.transform;
        }
        for (int z = tileWidthHeight; z < (mapHeight - 1) * tileWidthHeight; z=z+tileWidthHeight)
        {
            // EAST AND WEST
            GameObject ePrefab = tilePrefabs[1];
            GameObject wPrefab = tilePrefabs[1];
            float eEdgeRotation = edgeRotationToZXAxis;
            float wEdgeRotation = edgeRotationToZXAxis;
            if (randomEntranceLocation['e'].Contains(z))
            {
                ePrefab = tilePrefabs[2];
                eEdgeRotation = entranceRotationToZXAxis;
            }
            if (randomEntranceLocation['w'].Contains(z))
            {
                wPrefab = tilePrefabs[2];
                wEdgeRotation = entranceRotationToZXAxis;
            }

            GameObject eEdge = Instantiate(ePrefab, new Vector3(0, 0, z), Quaternion.Euler(0f, 270f + eEdgeRotation, 0f));
            GameObject wEdge = Instantiate(wPrefab, new Vector3((mapWidth - 1) * tileWidthHeight, 0, z), Quaternion.Euler(0f, 90f + wEdgeRotation, 0f));
            eEdge.transform.parent = this.transform;
            wEdge.transform.parent = this.transform;
        }
    }

    void GenerateTerrain()
    {
        // 
    }

    void GeneratePath()
    {
        HashSet<(int,int,int)> coords = GetPathSet();
        foreach ((int,int,int) coord in coords)
        {
            GameObject path = Instantiate(tilePrefabs[3], new Vector3(coord.Item1, coord.Item2, coord.Item3), Quaternion.identity);
            path.transform.parent = this.transform;
            Debug.Log($"{coord.Item1},{coord.Item2},{coord.Item3}");
        }
    }

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
