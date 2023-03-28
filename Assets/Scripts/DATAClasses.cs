using System.Collections.Generic;
using UnityEngine;

public class DATAClasses
{
    public static int Chunksize = 16;
    public static int ChunkHeight = 16;
    public static int CHUNKSHEIGHT = 20;

    public enum BlockIDS 
    { 
        AIR,
        GRASS_BLOCK,
        DIRT,
        STONE,
        COBBLESTONE,
        ANDESITE,
        GRANITE,
        DIORITE,
        BEDROCK,
        OBSIDIAN,DIAMOND_ORE_BLOCK,
        GLASS
    }

    public static Vector3[] QuadVerts(face sf)
    {
        Vector3[] verts = new Vector3[4];
        if (sf == face.FRONT)
        {
            verts = new Vector3[6]
            {
                new Vector3(0,0,1),
                new Vector3(1,1,1),
                new Vector3(0,1,1),

                new Vector3(0,0,1),
                new Vector3(1,0,1),
                new Vector3(1,1,1)
                        };
        }

        else if (sf == face.BACK)
        {
            verts = new Vector3[6]
            {
                new Vector3(1,0,0),
                new Vector3(0,1,0),
                new Vector3(1,1,0),

                new Vector3(1,0,0),
                new Vector3(0,0,0),
                new Vector3(0,1,0)
                        };
        }

        else if (sf == face.RIGHT)
        {
            verts = new Vector3[6]
            {
                new Vector3(1,0,1),
                new Vector3(1,1,0),
                new Vector3(1,1,1),

                new Vector3(1,0,1),
                new Vector3(1,0,0),
                new Vector3(1,1,0),
                        };
        }

        else if (sf == face.LEFT)
        {
            verts = new Vector3[6]
            {
                new Vector3(0,0,0),
                new Vector3(0,1,1),
                new Vector3(0,1,0),

                new Vector3(0,0,0),
                new Vector3(0,0,1),
                new Vector3(0,1,1),

                        };
        }

        else if (sf == face.UP)
        {
            verts = new Vector3[6]
            {
                new Vector3(0,1,1),
                new Vector3(1,1,0),
                new Vector3(0,1,0),

                new Vector3(0,1,1),
                new Vector3(1,1,1),
                new Vector3(1,1,0),
                        };
        }

        else if (sf == face.DOWN)
        {
            verts = new Vector3[6]
            {
                new Vector3(0,0,0),
                new Vector3(1,0,1),
                new Vector3(0,0,1),

                new Vector3(0,0,0),
                new Vector3(1,0,0),
                new Vector3(1,0,1),
                        };
        }

        return verts;
    }

    public static Vector2[] QuadTexures(DATAClasses.face sf,DATAClasses.BlockIDS block)
    {
        Vector2[] UVS = new Vector2[6];

        if (block == DATAClasses.BlockIDS.GRASS_BLOCK && sf == DATAClasses.face.UP )
        {
            UVS = new Vector2[6]
            {
                new Vector2(26 * 16 / 1024f, 62 * 16 / 1024f),
                new Vector2(27 * 16 / 1024f, 63 * 16 / 1024f),
                new Vector2(26 * 16 / 1024f, 63 * 16 / 1024f),

                new Vector2(26 * 16 / 1024f, 62 * 16 / 1024f),
                new Vector2(27 * 16 / 1024f, 62 * 16 / 1024f),
                new Vector2(27 * 16 / 1024f, 63 * 16 / 1024f)
            };
        }
        if (block == DATAClasses.BlockIDS.GRASS_BLOCK && sf == DATAClasses.face.DOWN)
        {
            UVS = new Vector2[6]
            {
                new Vector2(21 * 16 / 1024f, 53 * 16 / 1024f),
                new Vector2(22 * 16 / 1024f, 54 * 16 / 1024f),
                new Vector2(21 * 16 / 1024f, 54 * 16 / 1024f),

                new Vector2(21 * 16 / 1024f, 53 * 16 / 1024f),
                new Vector2(22 * 16 / 1024f, 53 * 16 / 1024f),
                new Vector2(22 * 16 / 1024f, 54 * 16 / 1024f)
            };
        }
        if (block == DATAClasses.BlockIDS.GRASS_BLOCK && sf != DATAClasses.face.UP && sf != DATAClasses.face.DOWN)
        {
            UVS = new Vector2[6]
            {
                new Vector2(25 * 16 / 1024f, 63 * 16 / 1024f),
                new Vector2(26 * 16 / 1024f, 64 * 16 / 1024f),
                new Vector2(25 * 16 / 1024f, 64 * 16 / 1024f),

                new Vector2(25 * 16 / 1024f, 63 * 16 / 1024f),
                new Vector2(26 * 16 / 1024f, 63 * 16 / 1024f),
                new Vector2(26 * 16 / 1024f, 64 * 16 / 1024f),
            };
        }


        return UVS;
    }

    public enum face { FRONT, BACK, RIGHT, LEFT, UP, DOWN }
    public enum State { READY,RELOAD}

    public static int friendIsSolid(int _x, int _y, int _z,int[,,] IDS,Vector3 CHUNKPOS,Dictionary<Vector3,ChunkScript> CHUNKS)
    {
        int x = new int();
        int y = new int();
        int z = new int();

        int[,,] IDs;
        if (_x < 0)
        {
            x = _x + DATAClasses.Chunksize;
            y = _y;
            z = _z;

            try 
            { 
                IDs = CHUNKS[CHUNKPOS + new Vector3(-1, 0, 0)].IDS; 
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return 0;
            }
        }
        else if(_y < 0)
        {
            y = _y + DATAClasses.ChunkHeight;
            x = _x;
            z = _z;

            try 
            { 
                IDs = CHUNKS[CHUNKPOS + new Vector3(0,-1, 0)].IDS; 
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return 0;
            }
        }
        else if(_z < 0)
        {
            z = _z + DATAClasses.Chunksize;
            y = _y;
            x = _x;

            try 
            {
                IDs = CHUNKS[CHUNKPOS + new Vector3(0, 0,-1)].IDS; 
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return 0;
            }
        }

        else if(_x >= DATAClasses.Chunksize)
        {
            x = _x - DATAClasses.Chunksize;
            y = _y;
            z = _z;

            try 
            { 
                IDs = CHUNKS[CHUNKPOS + new Vector3(1, 0, 0)].IDS; 
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return 0;
            }
        }
        else if(_y >= DATAClasses.Chunksize)
        {
            y = _y - DATAClasses.Chunksize;
            x = _x;
            z = _z;

            try 
            { 
                IDs = CHUNKS[CHUNKPOS + new Vector3(0, 1, 0)].IDS; 
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return 0;
            }
        }
        else if(_z >= DATAClasses.Chunksize)
        {
            z = _z - DATAClasses.Chunksize;
            x = _x;
            y = _y;

            try 
            { 
                IDs = CHUNKS[CHUNKPOS + new Vector3(0, 0, 1)].IDS; 
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return 0;
            }
        }
        else
        {
            IDs = IDS;
            x = _x;
            y = _y;
            z = _z;
        }
        return IDs[x, y, z];
    }

}
    

