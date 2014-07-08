using UnityEngine;
using System.Collections;

/*
 * No Tangents or UV made
 * 
 * Implementation assumes cells are even number of units
 */
public class NavMeshCreator : MonoBehaviour {

    public GameObject navMesh; //assigned in inspector

    private int vertsPerUnit = 1;
    //private float navMeshY = 0.0f;
    //int vertOffset = 1; //number of vertices that we are offsetting from the walls (so that AI doesn't intersect w/ walls)

    private byte[][] cellGrid;
    private int numCellsX;
    private int numCellsY;
    private int cellSize;
    private int vertsPerRow;

    Vector3[] vertices; //vertex array that will be returned

    //this array will be used for making triangles. cells that have no north wall and/or no east wall will place the indices to vertices[] in this array
    //when triangle-building, check this when no north and/or east wall
        //if missing north wall, use first (vertsPerRow - vertOffset)# of indices
        //if missing east wall only, use vertsPerRow# of indices (if nrth missing too, this iteration start is offset: see north wall missing to know offset)

    public void CreateMesh(byte[][] cellGrid) {
        //cellSize is units Per Cell
        cellSize = MapDrawer.cellSize;
        this.cellGrid = cellGrid;
        numCellsX = cellGrid.Length;
        numCellsY = cellGrid[0].Length;
        vertsPerRow = cellSize * vertsPerUnit;

        int width = (numCellsX * vertsPerRow) + 1;
        int height = (numCellsY * vertsPerRow) + 1;
        
        //add filter and renderer
        navMesh.AddComponent<MeshFilter>();

        //Debug only; a navmesh shouldn't have a renderer
        navMesh.AddComponent<MeshRenderer>();
        navMesh.renderer.material.color = Color.blue;
        navMesh.renderer.castShadows = false;
        navMesh.renderer.receiveShadows = false;

        //then cache the mesh from the filter
        Mesh mesh = navMesh.GetComponent<MeshFilter>().mesh;

        //Debug.Log("Width: " + width + " | Height: " + height);

        //Place vertices
        mesh.vertices = MakeVertices(width, height);
        //build triangles
        mesh.triangles = BuildTriangles(width, height);
        //auto-calc vertex normals from mesh
        mesh.RecalculateNormals();
    }

    private Vector3[] MakeVertices(int width, int height) {
        vertices = new Vector3[(width) * (height)]; //vertex array that will be returned
        Vector3 sizeScale = new Vector3((1 / vertsPerUnit), 1, 1 / vertsPerUnit);

        for (int z = 0; z < height; ++z) {
            for (int x = 0; x < width; ++x) {
                Vector3 vertex = new Vector3(x, 0.025f, z);
                vertices[z * width + x] = Vector3.Scale(sizeScale, vertex);
            }
        }    
        return vertices;
    }

    private int[] BuildTriangles(int width, int height) {
        int[] triangles = new int[(height - 1) * (width - 1) * 6]; //not sure what a more appropriate size would be
        int index = 0; //index for above array

        //loop cells: X# most significant
        for (int cellY = 0; cellY < numCellsY; ++cellY) {
            for (int cellX = 0; cellX < numCellsX; ++cellX) {
                //now in individual cell
                //cache results of cell's north and east wall checks. only need these two walls because neighbors will take care of "overflow" vertices
                bool hasNorthWall = false;
                bool hasEastWall = false;
                bool hasSouthWall = false;
                bool hasWestWall = false;

                if ((cellGrid[cellX][cellY] & (byte) WallMask.NorthRemovedCheck) == (byte) WallMask.NorthRemovedCheck) { //mask byte array to see if there is a wall
                    hasNorthWall = true;
                }
                if ((cellGrid[cellX][cellY] & (byte) WallMask.EastRemovedCheck) == (byte) WallMask.EastRemovedCheck) {
                    hasEastWall = true;
                }
                if ((cellGrid[cellX][cellY] & (byte) WallMask.SouthRemovedCheck) == (byte) WallMask.SouthRemovedCheck) { //mask byte array to see if there is a wall
                    hasSouthWall = true;
                }
                if ((cellGrid[cellX][cellY] & (byte) WallMask.WestRemovedCheck) == (byte) WallMask.WestRemovedCheck) {
                    hasWestWall = true;
                }

                //if has south wall, start on 1.
				//Debug.Log("Cell: " + cellX + ", " + cellY);
                for (int vertY = (hasSouthWall ? 1 : 0); vertY < (hasNorthWall ? (vertsPerRow - 1) : vertsPerRow); ++vertY) {
                    for (int vertX = (hasWestWall ? 1 : 0); vertX < (hasEastWall ? (vertsPerRow - 1) : vertsPerRow); ++vertX) {
                        //abort making triangles at any point if any vert is on wall. must check non-shared neighbor on corners
                        if (vertY == 0 && vertX == 0) { //bottom left corner
                            if (cellY > 0) { //if has South neighbor
                                //if neighbor has west wall
                                if ((cellGrid[cellX][cellY - 1] & (byte) WallMask.WestRemovedCheck) == (byte) WallMask.WestRemovedCheck) {
                                    Debug.Log("Loc " + cellX + ", " + cellY + ", " + vertX + ", " + vertY + "'s neighbor, " + cellX + ", " + (cellY - 1) + " has West wall");
                                    continue;
                                }
                            } if (cellX > 0) { //if has west neighbor
                                //if neighbor has south wall
                                if ((cellGrid[cellX - 1][cellY] & (byte) WallMask.SouthRemovedCheck) == (byte) WallMask.SouthRemovedCheck) {
                                    Debug.Log("Loc " + cellX + ", " + cellY + ", " + vertX + ", " + vertY + "'s neighbor, " + (cellX - 1) + ", " + (cellY) + " has South wall");
                                    continue;
                                }
                            }
                        } else if (vertY == 0 && vertX == vertsPerRow - 1) { //bottom right corner (before wall since building triangles to those verts)
                            if (cellY > 0) { //if has South neighbor
                                //if neighbor has east wall
                                if ((cellGrid[cellX][cellY - 1] & (byte) WallMask.EastRemovedCheck) == (byte) WallMask.EastRemovedCheck) {
                                    Debug.Log("Loc " + cellX + ", " + cellY + ", " + vertX + ", " + vertY + "'s neighbor, " + (cellX) + ", " + (cellY - 1) + " has East wall");
                                    continue;
                                }
                            } if (cellX < numCellsX) { //if has east neighbor
                                //if neighbor has south wall
                                if ((cellGrid[cellX + 1][cellY] & (byte) WallMask.SouthRemovedCheck) == (byte) WallMask.SouthRemovedCheck) {
                                    Debug.Log("Loc " + cellX + ", " + cellY + ", " + vertX + ", " + vertY + "'s neighbor, " + (cellX + 1) + ", " + (cellY) + " has South wall");
                                    continue;
                                }
                            }
                        } else if (vertY == vertsPerRow - 1 && vertX == 0) { //top left corner (before wall since building triangles to those verts)
                            if (cellY < numCellsY) { //if has North neighbor
                                //if neighbor has west wall
                                if ((cellGrid[cellX][cellY + 1] & (byte) WallMask.WestRemovedCheck) == (byte) WallMask.WestRemovedCheck) {
                                    Debug.Log("Loc " + cellX + ", " + cellY + ", " + vertX + ", " + vertY + "'s neighbor, " + (cellX) + ", " + (cellY + 1) + " has West wall");
                                    continue;
                                }
                            } if (cellX > 0) { //if has west neighbor
                                //if neighbor has north wall
                                if ((cellGrid[cellX - 1][cellY] & (byte) WallMask.NorthRemovedCheck) == (byte) WallMask.NorthRemovedCheck) {
                                    Debug.Log("Loc " + cellX + ", " + cellY + ", " + vertX + ", " + vertY + "'s neighbor, " + (cellX - 1) + ", " + (cellY) + " has North wall");
                                    continue;
                                }
                            }
                        } else if (vertY == vertsPerRow - 1 && vertX == vertsPerRow - 1) { //top right corner (before walls)
                            if (cellY < numCellsY) { //if has North neighbor
                                //if neighbor has east wall
                                if ((cellGrid[cellX][cellY + 1] & (byte) WallMask.EastRemovedCheck) == (byte) WallMask.EastRemovedCheck) {
                                    Debug.Log("Loc " + cellX + ", " + cellY + ", " + vertX + ", " + vertY + "'s neighbor, " + (cellX) + ", " + (cellY + 1) + " has East wall");
                                    continue;
                                }
                            } if (cellX < numCellsX) { //if has east neighbor
                                //if neighbor has north wall
                                if ((cellGrid[cellX + 1][cellY] & (byte) WallMask.NorthRemovedCheck) == (byte) WallMask.NorthRemovedCheck) {
                                    Debug.Log("Loc " + cellX + ", " + cellY + ", " + vertX + ", " + vertY + "'s neighbor, " + (cellX + 1) + ", " + (cellY) + " has North wall");
                                    continue;
                                }
                            }
                        }
                        //put into triangles array the corresponding vert array indices
                        //Triangle 1
                        //(cellY * width * vertsPerRow) = since vert array adds verts across whole map a row at a time (width), this offsets correctly by current cell Y pos
                        //(vertY * width) = offsets by current vert Y pos (Y row within cell)
                        //(... + width) = when building triangle, adding width will go up exactly 1 row to get northern vert. I.E. counts width more indices to get northern vert index
                        //(cellX * vertsPerRow) + vertX = offset by current cell X plus current vert X (x pos within cell)
                        //(.. + 1) = when building triangle, adding 1 to index gets eastern index
                        triangles[index++] = (cellY * width * vertsPerRow) + (vertY * width) + ((cellX * vertsPerRow) + vertX);
                        triangles[index++] = (cellY * width * vertsPerRow) + ((vertY * width) + width) + ((cellX * vertsPerRow) + vertX);
                        triangles[index++] = (cellY * width * vertsPerRow) + (vertY * width) + ((cellX * vertsPerRow) + vertX) + 1;
                        //Triangle 2
                        triangles[index++] = (cellY * width * vertsPerRow) + ((vertY * width) + width) + ((cellX * vertsPerRow) + vertX);
                        triangles[index++] = (cellY * width * vertsPerRow) + ((vertY * width) + width) + ((cellX * vertsPerRow) + vertX) + 1;
                        triangles[index++] = (cellY * width * vertsPerRow) + (vertY * width) + ((cellX * vertsPerRow) + vertX) + 1;
                    }
                }
            }
        }

        return triangles;
    }
}