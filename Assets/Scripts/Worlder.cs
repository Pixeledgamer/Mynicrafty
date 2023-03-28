using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Worlder : MonoBehaviour
{
    public static Dictionary<Vector3, ChunkScript> Chunks = new Dictionary<Vector3, ChunkScript>();

    void Start()
    {
        GENDATA();
        GENMESH();
    }



    void GENDATA()
    {
        for (int ChunkX = 0; ChunkX < 3; ChunkX++)
        {
            for (int ChunkY = 0; ChunkY < 3; ChunkY++)
            {
                for (int ChunkZ = 0; ChunkZ < 3; ChunkZ++)
                {
                    ChunkScript CH = new ChunkScript();
                    CH.ChunkPos = new Vector3(ChunkX, ChunkY, ChunkZ);
                    CH.mat = transform.GetComponent<MeshRenderer>().material;
                    CH.parent = transform;
                    CH.CreateData();
                    Chunks[new Vector3(ChunkX, ChunkY, ChunkZ)] = CH;

                }
            }
        }
    }
    void GENMESH()
    {

        for (int ChunkX = 0; ChunkX < 3; ChunkX++)
        {
            for (int ChunkY = 0; ChunkY < 3; ChunkY++)
            {
                for (int ChunkZ = 0; ChunkZ < 3; ChunkZ++)
                {
                    if (Chunks[new Vector3(ChunkX, ChunkY, ChunkZ)].STATUS ==
                        DATAClasses.State.READY)
                    {
                        Chunks[new Vector3(ChunkX, ChunkY, ChunkZ)].StartUpdate();
                    }
                }
            }
        }
    }

}

