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
                        randomEntranceCoordinates.Add((randomNumber * tileWidthHeight, 0, 0));
                        break;
                    case 'e':
                        randomEntranceCoordinates.Add((0, 0, randomNumber * tileWidthHeight));
                        break;
                    case 's':
                        randomEntranceCoordinates.Add((randomNumber * tileWidthHeight, 0, (mapHeight - 1) * tileWidthHeight));
                        break;
                    default:
                        randomEntranceCoordinates.Add(((mapWidth - 1) * tileWidthHeight, 0, randomNumber * tileWidthHeight));
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

    private HashSet<(int,int,int)> GetTerrainSet()
    {
        HashSet<(int,int,int)> plainGroundCoordinates = new HashSet<(int,int,int)>();
        for (int x = tileWidthHeight; x < (mapWidth-1)*tileWidthHeight; x = x + tileWidthHeight)
        {
            for (int z = tileWidthHeight; z < (mapHeight-1)*tileWidthHeight; z = z + tileWidthHeight)
            {
                plainGroundCoordinates.Add((x,0,z));
            }
        }
        return plainGroundCoordinates;
    }

    void GeneratePath()
    {
        HashSet<(int,int,int)> coords = GetPathSet();
        HashSet<(int,int,int)> plainGroundCoordinates = GetTerrainSet();
        foreach ((int,int,int) coord in coords)
        {
            GameObject path = Instantiate(tilePrefabs[7], new Vector3(coord.Item1, coord.Item2, coord.Item3), Quaternion.identity);
            path.transform.parent = this.transform;
            plainGroundCoordinates.Remove(coord);
        }
        foreach(var coord in plainGroundCoordinates)
        {
            GameObject path = Instantiate(tilePrefabs[6], new Vector3(coord.Item1, coord.Item2, coord.Item3), Quaternion.identity);
            path.transform.parent = this.transform;
        }
    }

    private HashSet<(int,int,int)> GetPathSet()
    {
        (int,int,int) startCoordinate = (0,0,0);
        HashSet<(int,int,int)> endCoordinates = new HashSet<(int,int,int)>();
        HashSet<(int,int,int)> traveled = new HashSet<(int,int,int)>();

        bool is_first = true;
        foreach ((int,int,int) coordinate in randomEntranceCoordinates)
        {
            if (is_first) {
                startCoordinate = coordinate;
                is_first = false;
                traveled.Add(coordinate);
            }
            else {
                endCoordinates.Add(coordinate);
                traveled.Add(coordinate);
            }
        }

        // HashSet<(int,int,int)> temp = new HashSet<(int,int,int)>();
        // foreach(var endCoordinate in endCoordinates)
        // {
        //     Debug.Log($"End Coord {endCoordinate.Item1}, {endCoordinate.Item2}, {endCoordinate.Item3}");
        //     List<(int,int,int)> neighborOffsets = new List<(int,int,int)> {(-2,0,0), (2,0,0), (0,0,2), (0,0,-2)};
        //     foreach ((int,int,int) neighborOffset in neighborOffsets)
        //     {
        //         Debug.Log($"Trying {neighborOffset.Item1}, {neighborOffset.Item2}, {neighborOffset.Item3}");
        //         // alligning new
        //         (int,int,int) neighborCoordinate = (endCoordinate.Item1 + neighborOffset.Item1, 
        //                                             endCoordinate.Item2 + neighborOffset.Item2, 
        //                                             endCoordinate.Item3 + neighborOffset.Item3);
        //         // Check within bounds
        //         if (neighborCoordinate.Item1 < tileWidthHeight ||
        //             neighborCoordinate.Item1 > (mapWidth-1) * tileWidthHeight ||
        //             neighborCoordinate.Item3 < tileWidthHeight ||
        //             neighborCoordinate.Item3 > (mapHeight-1) * tileWidthHeight)
        //         {
        //             continue;
        //         }
        //         temp.Add(neighborCoordinate);
        //         Debug.Log($"Adding {neighborCoordinate.Item1}, {neighborCoordinate.Item2}, {neighborCoordinate.Item3}");
        //     }
        // }
        // return temp;

        Dictionary<(int,int,int),(int,int,int)> previousNode = new Dictionary<(int,int,int), (int,int,int)>();
        foreach ((int,int,int) endCoordinate in endCoordinates) 
        {
            // Fixing target
            (int,int,int) coordinate = (0,0,0);
            if (endCoordinate.Item1 == 0) { coordinate = (endCoordinate.Item1 + tileWidthHeight, endCoordinate.Item2, endCoordinate.Item3);}
            else if (endCoordinate.Item3 == 0) { coordinate = (endCoordinate.Item1, endCoordinate.Item2, endCoordinate.Item3 + tileWidthHeight);}
            else if (endCoordinate.Item1 == (mapWidth-1)*tileWidthHeight) { coordinate = (endCoordinate.Item1 - tileWidthHeight, endCoordinate.Item2, endCoordinate.Item3);}
            else if (endCoordinate.Item3 == (mapHeight-1)*tileWidthHeight) { coordinate = (endCoordinate.Item1, endCoordinate.Item2, endCoordinate.Item3 - tileWidthHeight);}
            previousNode.Add(endCoordinate, coordinate);


            Debug.Log($"End Coordinate {coordinate.Item1}, {coordinate.Item2}, {coordinate.Item3}");
            // Run djikstra
            BinaryHeap<WeightedCoordinate> heap = new BinaryHeap<WeightedCoordinate>();
            HashSet<(int,int,int)> visited = new HashSet<(int,int,int)>();
            Dictionary<(int,int,int), double> distances = new Dictionary<(int,int,int), double>();

            WeightedCoordinate startNode = new WeightedCoordinate(0.0, startCoordinate);
            heap.Add(startNode);
            int COUNT = 0;
            while (heap.IsEmpty() == false)
            {
                WeightedCoordinate currentNode = heap.Remove();
                // Already visited
                if (visited.Contains(currentNode.Coordinate)) {continue;}
                COUNT++;
                // Debug.Log($"At {currentNode.Coordinate.Item1}, {currentNode.Coordinate.Item2}, {currentNode.Coordinate.Item3}");
                // Target Found
                if (currentNode.Coordinate == coordinate) {
                    Debug.Log($"At {currentNode.Coordinate.Item1}, {currentNode.Coordinate.Item2}, {currentNode.Coordinate.Item3}");
                    break;
                    }
                visited.Add(currentNode.Coordinate);
                traveled.Add(currentNode.Coordinate);

                // Neighbors
                List<(int,int,int)> neighborOffsets = new List<(int,int,int)> {(-2,0,0), (2,0,0), (0,0,2), (0,0,-2)};
                foreach ((int,int,int) neighborOffset in neighborOffsets)
                {
                    // alligning new
                    (int,int,int) neighborCoordinate = (currentNode.Coordinate.Item1 + neighborOffset.Item1, 
                                                        currentNode.Coordinate.Item2 + neighborOffset.Item2, 
                                                        currentNode.Coordinate.Item3 + neighborOffset.Item3);
                    // Check within bounds
                    if (neighborCoordinate.Item1 < 2 ||
                        neighborCoordinate.Item1 > (mapWidth-2) * tileWidthHeight ||
                        neighborCoordinate.Item3 < 2 ||
                        neighborCoordinate.Item3 > (mapHeight-2) * tileWidthHeight)
                    {
                        continue;
                    }

                    // Check not visited
                    if (visited.Contains(neighborCoordinate)) {continue;}


                    // Update distance
                    double edgeCost = 0;
                    if (traveled.Contains(neighborCoordinate)){
                        edgeCost = 0;
                        currentNode.Weight = 0;
                    }
                    else {
                        edgeCost = (traveled.Contains(neighborCoordinate)) ? 0 : 2;
                    }
                    if (distances.ContainsKey(neighborCoordinate)) 
                    {
                        if (distances[neighborCoordinate] > currentNode.Weight + edgeCost)
                        {
                            distances[neighborCoordinate] = currentNode.Weight + edgeCost;
                            previousNode[neighborCoordinate] = currentNode.Coordinate;
                        }
                    }
                    else
                    {
                        distances.Add(neighborCoordinate, currentNode.Weight + edgeCost);
                        if (previousNode.ContainsKey(neighborCoordinate)){
                            previousNode[neighborCoordinate] = currentNode.Coordinate;
                        }
                        else{
                            previousNode.Add(neighborCoordinate, currentNode.Coordinate);
                        }
                    }

                    // Add neighbor to heap
                    WeightedCoordinate neighborNode = new WeightedCoordinate(distances[neighborCoordinate], neighborCoordinate);
                    heap.Add(neighborNode);
                }
            }
            // Debug.Log($"Count: {COUNT}");
            // Debug.Log($"{distances[coordinate]}");
            // Debug.Log($"{previousNode[coordinate]}");


        }
        // Debug.Log($"{COUNT}");
        HashSet<(int,int,int)> pathTileCoordinates = new HashSet<(int,int,int)>();
        (int,int,int) currentCoordinate;
        foreach ((int,int,int) endCoordinate in endCoordinates)
        {
            currentCoordinate = endCoordinate;
            pathTileCoordinates.Add(currentCoordinate);

            Debug.Log($"Adding {currentCoordinate.Item1}, {currentCoordinate.Item2}, {currentCoordinate.Item3}");
            while (previousNode.ContainsKey(currentCoordinate))
            {
                currentCoordinate = previousNode[currentCoordinate];
                pathTileCoordinates.Add(currentCoordinate);
                Debug.Log($"Adding {currentCoordinate.Item1}, {currentCoordinate.Item2}, {currentCoordinate.Item3}");
            }
        }
        return pathTileCoordinates;
    }


}



public class WeightedCoordinate : IComparable<WeightedCoordinate>
{
    public double Weight { get; set;}
    public (int,int,int) Coordinate { get; }

    public WeightedCoordinate(double weight, (int,int,int) coordinate)
    {
        Weight = weight;
        Coordinate = coordinate;
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
        return $"Weight: {Weight}, X: {Coordinate.Item1}, Y: {Coordinate.Item2}, Z: {Coordinate.Item3}";
    }
}
