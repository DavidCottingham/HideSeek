using UnityEngine;
using System.Collections;

public class MapDrawer : MonoBehaviour {

    public static int cellSize = 5;

    public GameObject wall; //inspector assigned
    public Transform mapHolder; //inspector assigned

    //Draws each wall that is marked in the byte grid only if its neighbor has not yet been drawn
    public void DrawMap(byte[][] cellGrid) {
        //multiply by cellsize to make cell walls spaced for appropriate map size
        Vector3 northWall = new Vector3(0.5f * cellSize, 0.5f, 1.0f * cellSize);
        Vector3 eastWall = new Vector3(1.0f * cellSize, 0.5f, 0.5f * cellSize);
        Vector3 southWall = new Vector3(0.5f * cellSize, 0.5f, 0.0f);
        Vector3 westWall = new Vector3(0.0f, 0.5f, 0.5f * cellSize);

        //Walls are mirrored, so don't worry about a specific face pointing a specific direction
        Quaternion northWallrot = Quaternion.Euler(0, 90.0f, 0);
        Quaternion eastWallrot = Quaternion.identity;
        Quaternion southWallrot = northWallrot;
        Quaternion westWallrot = eastWallrot;

        //2 loops cycle through byte grid
        for (int i = 0; i < cellGrid.Length; ++i) {
            for (int j = 0; j < cellGrid[i].Length; ++j) {
                //multiplied by cellsize to make each cell spaced from each other for appropriate map size
                Vector3 currentWorldPos = new Vector3(i * cellSize, 0, j * cellSize);
                if ((cellGrid[i][j] & (byte) WallMask.NorthRemovedCheck) == (byte) WallMask.NorthRemovedCheck) {  //if byte grid says has wall
                    GameObject tempWall = null; //temporary object so wall's parent can be set
                    if (j + 1 < cellGrid[i].Length && (cellGrid[i][j + 1] & (byte) WallMask.SouthDrawn) != (byte) WallMask.SouthDrawn) { //if northern neighbor hasn't drawn south wall (and within map bounds)
                        tempWall = (GameObject) Instantiate(wall, northWall + currentWorldPos, northWallrot); //temp object gets copy of now instantiated wall
                        cellGrid[i][j] |= (byte) WallMask.NorthDrawn; //mark this wall drawn for neighbor's check
                    } else if (j == cellGrid[i].Length - 1) { //if is border wall, don't check neighbor (there isn't a neighbor!)
                        tempWall = (GameObject) Instantiate(wall, northWall + currentWorldPos, northWallrot);
                    }
                    if (tempWall) { tempWall.transform.parent = mapHolder; } //set wall's parent to mapHolder
                    tempWall = null; //help garbage collector ... maybe?
                }
                if ((cellGrid[i][j] & (byte) WallMask.EastRemovedCheck) == (byte) WallMask.EastRemovedCheck) {
                    GameObject tempWall = null;
                    if (i + 1 < cellGrid.Length && (cellGrid[i + 1][j] & (byte) WallMask.WestDrawn) != (byte) WallMask.WestDrawn) {
                        tempWall = (GameObject) Instantiate(wall, eastWall + currentWorldPos, eastWallrot);
                        cellGrid[i][j] |= (byte) WallMask.EastDrawn;
                    } else if (i == cellGrid.Length - 1) {
                        tempWall = (GameObject) Instantiate(wall, eastWall + currentWorldPos, eastWallrot);
                    }
                    if (tempWall) { tempWall.transform.parent = mapHolder; }
                    tempWall = null;
                }
                if ((cellGrid[i][j] & (byte) WallMask.SouthRemovedCheck) == (byte) WallMask.SouthRemovedCheck) {
                    GameObject tempWall = null;
                    if (j-1 >= 0 && (cellGrid[i][j - 1] & (byte) WallMask.NorthDrawn) != (byte) WallMask.NorthDrawn) {
                        tempWall = (GameObject) Instantiate(wall, southWall + currentWorldPos, southWallrot);
                        cellGrid[i][j] |= (byte) WallMask.SouthDrawn;
                    } else if (j == 0) {
                        tempWall = (GameObject) Instantiate(wall, southWall + currentWorldPos, southWallrot);
                    }
                    if (tempWall) { tempWall.transform.parent = mapHolder; }
                    tempWall = null;
                }
                if ((cellGrid[i][j] & (byte) WallMask.WestRemovedCheck) == (byte) WallMask.WestRemovedCheck) {
                    GameObject tempWall = null;
                    if (i-1 >= 0 && (cellGrid[i - 1][j] & (byte) WallMask.EastDrawn) != (byte) WallMask.EastDrawn) {
                        tempWall = (GameObject) Instantiate(wall, westWall + currentWorldPos, westWallrot);
                        cellGrid[i][j] |= (byte) WallMask.WestDrawn;
                    } else if (i == 0) {
                        tempWall = (GameObject) Instantiate(wall, westWall + currentWorldPos, westWallrot);
                    }
                    if (tempWall) { tempWall.transform.parent = mapHolder; }
                    tempWall = null;
                }
            }
        }

        //draw floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = new Vector3(cellGrid.Length / 2 * 5, 0.0f, cellGrid[0].Length / 2 * 5); //move center of plane to center of map. 5 is half default size of plane. ASSUMES either first elements are longest array length or all arrays same length
        floor.transform.localScale = new Vector3(cellGrid.Length / 2, 1.0f, cellGrid[0].Length / 2); //scale x and z (from center, so half of map size). ASSUMES either first elements are longest array length or all arrays same length
        floor.transform.parent = mapHolder; //set floor's parent to the map holder object
    }
}
