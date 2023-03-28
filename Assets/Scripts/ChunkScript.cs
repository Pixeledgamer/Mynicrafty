using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkScript 
{
    public Vector3 ChunkPos;
    public Material mat;
    public Transform parent;

    GameObject ChunkObj;

    public DATAClasses.State STATUS = DATAClasses.State.RELOAD;
    public int[,,] IDS;
    Mesh msh;

    public void CreateData()
    {
        IDS = new int[DATAClasses.Chunksize, DATAClasses.ChunkHeight, DATAClasses.Chunksize];

        

        for (int x = 0; x < DATAClasses.Chunksize; x++)
        {
            for (int y = 0; y < DATAClasses.ChunkHeight; y++)
            {
                for (int z = 0; z < DATAClasses.Chunksize; z++)
                {
                    IDS[x, y, z] = 1;
                }
            }
        }
        IDS[15, 15, 15] = 0;
        IDS[14, 15, 15] = 0;
        IDS[15, 15, 14] = 0;
        IDS[15, 14, 15] = 0;
        STATUS = DATAClasses.State.READY;
    }
    public void RenderData(int[,,] DATA)
    {
        int ind = 0;
        List<Vector3> VERTICES = new List<Vector3>();
        List<int> TRIANGLES = new List<int>();
        List<Vector2> UVS = new List<Vector2>();

        for (int x = 0; x < DATAClasses.Chunksize; x++)
        {
            for (int y = 0; y < DATAClasses.ChunkHeight; y++)
            {
                for (int z = 0; z < DATAClasses.Chunksize; z++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    if (DATA[x, y, z] != (int)DATAClasses.BlockIDS.AIR)
                    {
                        if (DATAClasses.friendIsSolid(x, y + 1, z, DATA, ChunkPos, Worlder.Chunks) == (int)DATAClasses.BlockIDS.AIR)
                        {
                            for  (int i = 0; i < DATAClasses.QuadVerts(DATAClasses.face.UP).Length;i++)
                            {
                                VERTICES.Add(DATAClasses.QuadVerts(DATAClasses.face.UP)[i]+ pos);
                                UVS.Add(DATAClasses.QuadTexures(DATAClasses.face.UP, (DATAClasses.BlockIDS)DATA[x, y, z])[i]);
                                TRIANGLES.Add(ind);
                                ind++;
                            }
                        }
                        if (DATAClasses.friendIsSolid(x, y - 1, z, DATA, ChunkPos, Worlder.Chunks) == (int)DATAClasses.BlockIDS.AIR)
                        {
                            for (int i = 0; i < DATAClasses.QuadVerts(DATAClasses.face.DOWN).Length; i++)
                            {
                                VERTICES.Add(DATAClasses.QuadVerts(DATAClasses.face.DOWN)[i] + pos);
                                UVS.Add(DATAClasses.QuadTexures(DATAClasses.face.DOWN, (DATAClasses.BlockIDS)DATA[x,y,z])[i]);
                                TRIANGLES.Add(ind);
                                ind++;
                            }
                        }
                        if (DATAClasses.friendIsSolid(x, y, z + 1, DATA, ChunkPos, Worlder.Chunks) == (int)DATAClasses.BlockIDS.AIR)
                        {
                            for (int i = 0; i < DATAClasses.QuadVerts(DATAClasses.face.FRONT).Length; i++)
                            {
                                VERTICES.Add(DATAClasses.QuadVerts(DATAClasses.face.FRONT)[i] + pos);
                                UVS.Add(DATAClasses.QuadTexures(DATAClasses.face.FRONT, (DATAClasses.BlockIDS)DATA[x, y, z])[i]);
                                TRIANGLES.Add(ind);
                                ind++;
                            }
                        }
                        if (DATAClasses.friendIsSolid(x, y, z - 1, DATA, ChunkPos, Worlder.Chunks) == (int)DATAClasses.BlockIDS.AIR)
                        {
                            for (int i = 0; i < DATAClasses.QuadVerts(DATAClasses.face.BACK).Length; i++)
                            {
                                VERTICES.Add(DATAClasses.QuadVerts(DATAClasses.face.BACK)[i] + pos);
                                UVS.Add(DATAClasses.QuadTexures(DATAClasses.face.BACK, (DATAClasses.BlockIDS)DATA[x, y, z])[i]);
                                TRIANGLES.Add(ind);
                                ind++;
                            }
                        }
                        if (DATAClasses.friendIsSolid(x + 1, y, z, DATA, ChunkPos, Worlder.Chunks) == (int)DATAClasses.BlockIDS.AIR)
                        {
                            for (int i = 0; i < DATAClasses.QuadVerts(DATAClasses.face.RIGHT).Length; i++)
                            {
                                VERTICES.Add(DATAClasses.QuadVerts(DATAClasses.face.RIGHT)[i] + pos);
                                UVS.Add(DATAClasses.QuadTexures(DATAClasses.face.RIGHT, (DATAClasses.BlockIDS)DATA[x, y, z])[i]);
                                TRIANGLES.Add(ind);
                                ind++;
                            }
                        }
                        if (DATAClasses.friendIsSolid(x - 1, y, z, DATA, ChunkPos, Worlder.Chunks) == (int)DATAClasses.BlockIDS.AIR)
                        {
                            for (int i = 0; i < DATAClasses.QuadVerts(DATAClasses.face.LEFT).Length; i++)
                            {
                                VERTICES.Add(DATAClasses.QuadVerts(DATAClasses.face.LEFT)[i] + pos);
                                UVS.Add(DATAClasses.QuadTexures(DATAClasses.face.LEFT, (DATAClasses.BlockIDS)DATA[x, y, z])[i]);
                                TRIANGLES.Add(ind);
                                ind++;
                            }
                        }
                    }
                }
            }
        }
        msh = new Mesh();
        msh.Clear();
        msh.vertices = VERTICES.ToArray();
        msh.triangles = TRIANGLES.ToArray();
        msh.uv = UVS.ToArray();
        msh.RecalculateNormals();

    }

    public void Break(Vector3Int POS)
    {
        IDS[POS.x, POS.y, POS.z] = (int)DATAClasses.BlockIDS.AIR;
        //IDS[POS.x, POS.y+1, POS.z] = (int)DATAClasses.BlockIDS.GRASS_BLOCK;
        UpdateChunk();
    }
    public void UpdateChunk()
    {

        GameObject.DestroyImmediate(ChunkObj.GetComponent<MeshFilter>());
        GameObject.DestroyImmediate(ChunkObj.GetComponent<MeshRenderer>());
        GameObject.DestroyImmediate(ChunkObj.GetComponent<MeshCollider>());

        RenderData(IDS);

        ChunkObj.AddComponent<MeshFilter>().mesh = msh;
        ChunkObj.AddComponent<MeshRenderer>().material = mat;
        ChunkObj.AddComponent<MeshCollider>();

    }

    public void StartUpdate()
    {
        RenderData(IDS);
        ChunkObj = new GameObject();
        ChunkObj.AddComponent<MeshFilter>().mesh = msh;
        ChunkObj.AddComponent<MeshRenderer>().material = mat;
        ChunkObj.transform.position = ChunkPos * DATAClasses.Chunksize;
        ChunkObj.transform.parent = parent;
        ChunkObj.layer = ChunkObj.transform.parent.gameObject.layer;
        ChunkObj.AddComponent<MeshCollider>();
    }


}




