using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class CellManagement : MonoBehaviour {

    /*
     * 00 = no cell (empty space)
     * 01 = wall, unprotected (set as default)
     * 10 = no wall, protected
     * 11 = wall, protected
     */

    /* 0000 0000
     * |||| ||||_west
     * |||| ||_south
     * ||||_east
     * ||_north
     */

    private enum Direction {North, South, East, West};

    private byte cellDefault = 85;  //01010101 / all walls present and unprotected

    private int gridX = 12;
    private int gridZ = 12;

    private byte[][] cellGrid;
    private List<int[]> activeCellPositions;
    private Direction neighborDirection;

    void Start() {
        cellGrid = new byte[gridX][];
        for (int i = 0; i < gridX; ++i) { //every row
            cellGrid[i] = new byte[gridZ];
            for (int j = 0; j < gridZ; ++j) { //every column per row (cell)
                cellGrid[i][j] = cellDefault; //make the grid. each cell has default value of all cells having 4 walls.
            }
        }

        activeCellPositions = new List<int[]>();

        //int x = Random.Range(0, gridX);
        //int z = Random.Range(0, gridZ);
        int[] position = new int[] { Random.Range(0, gridX), Random.Range(0, gridZ) }; //start random pos
        //int[] position = new int[] { 0, 0 }; //start bot left

        activeCellPositions.Add(position);

        while (activeCellPositions.Count != 0) {
            position = chooseFromActiveList();

            int[] neighborPos = chooseNeighbor(position);
            if (neighborPos == null) {
                activeCellPositions.Remove(position);
                //Debug.Log("No neighbors for " + position[0] + "," + position[1]);
            } else {
                RemoveWall(neighborDirection, position);
                RemoveWall(OppositeDirection(neighborDirection), neighborPos);
                activeCellPositions.Add(neighborPos);
            }
        }

        //draw map enough?
        MapDrawer drawMap = GetComponent<MapDrawer>(); 
        drawMap.DrawMap(cellGrid);

        //make navmesh
        NavMeshCreator nmCreator = GetComponent<NavMeshCreator>();
        nmCreator.CreateMesh(cellGrid);
    }

    //Choose a neighbor that hasn't been visited before. Returns its grid position.
    private int[] chooseNeighbor(int[] pos) {
        int x = pos[0], z = pos[1];
        int neighborX = x, neighborZ = z;

        //get a random direction by choosing a random number between 0 and the # of vars in the Enum
        //# of enums comes from getting Length of the array of the names
        Direction[] directions = ShuffleDirections();
        foreach (Direction dir in directions) {
            //Debug.Log("Checking " + dir);
            switch (dir) {
                case Direction.North:
                    neighborDirection = Direction.North;
                    neighborZ = z + 1;
                    break;
                case Direction.East:
                    neighborDirection = Direction.East;
                    neighborX = x + 1;
                    break;
                case Direction.South:
                    neighborDirection = Direction.South;
                    neighborZ = z - 1;
                    break;
                case Direction.West:
                    neighborDirection = Direction.West;
                    neighborX = x - 1;
                    break;
            }

            int[] neighborArray = new int[] { neighborX, neighborZ };
            //Debug.Log("neighbor check: " + neighborArray[0] + "," + neighborArray[1]);
            //if within bounds and cell has default value (never been traversed)
            if (neighborX >= 0 && neighborZ >= 0 && neighborX < gridX && neighborZ < gridZ && (cellGrid[neighborArray[0]][neighborArray[1]] == cellDefault)) {
                //Debug.Log("neighbor accepted: " + neighborArray[0] + "," + neighborArray[1]);
                return neighborArray;
            } else { //Don't forget to reset to current position after failed check!
                neighborX = x;
                neighborZ = z;
            }
        }
        return null;
    }

    //First, last, random, etc.
    //currently random / Prim's
    private int[] chooseFromActiveList() {
        return activeCellPositions[Random.Range(0, activeCellPositions.Count)]; //Random = Prim's
        //return activeCellPositions.Last<int[]>(); //Newest = Recursive Backtracker
    }

    //randomize an array of the 4 directions for traversing grid. Limits direction bias
    private Direction[] ShuffleDirections() {
        //first create array of directions names
        string[] nameArray = System.Enum.GetNames(typeof(Direction));
        Direction[] dirArray = new Direction[nameArray.Length];
        //then create an array of Directions (enums) using those names
        for (int i = 0; i < dirArray.Length; ++i) {
            dirArray[i] = (Direction) i;
        }

        //Every day I'm shufflin'
        for (int i = 0; i < dirArray.Length; ++i) {
            Direction temp = dirArray[i];
            int rndIndex = Random.Range(0, dirArray.Length);
            dirArray[i] = dirArray[rndIndex];
            dirArray[rndIndex] = temp;
        }
        return dirArray;
    }

    //returns opposite direction of passed (N->S, S->N, E->W, W->E)
    private Direction OppositeDirection(Direction dir) {
        switch (dir) {
            case Direction.North:
                return Direction.South;
            case Direction.East:
                return Direction.West;
            case Direction.South:
                return Direction.North;
            case Direction.West:
            default:
                return Direction.East;
        }
    }

    //mask cell appropriately to remove wall from cell
    private void RemoveWall(Direction dir, int[] pos) {
        switch (dir) {
            case Direction.North:
                cellGrid[pos[0]][pos[1]] &= (byte) WallMask.NorthRemoval;
                break;
            case Direction.East:
                cellGrid[pos[0]][pos[1]] &= (byte) WallMask.EastRemoval;
                break;
            case Direction.South:
                cellGrid[pos[0]][pos[1]] &= (byte) WallMask.SouthRemoval;
                break;
            case Direction.West:
                cellGrid[pos[0]][pos[1]] &= (byte) WallMask.WestRemoval;
                break;
        }
    }

    public byte[][] GetGridArray() {
        return cellGrid;
    }
}