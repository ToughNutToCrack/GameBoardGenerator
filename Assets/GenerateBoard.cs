using System.Collections.Generic;
using UnityEngine;

public enum BoardType
{
    Random,
    AlternateLines,
    Chess
}

public class GenerateBoard : MonoBehaviour
{
    public BoardType type = BoardType.Chess;
    public bool wantParentForTheBoard = true, wantParentPivotOnCenter = true;
    public int xLength = 8, yLength = 1, zLength = 8;
    public List<GameObject> toSpawn; 
    public Transform startPosition;
    public Vector3 offset;
    List<Transform> spawned;
    void Start()
    {
        spawned = new List<Transform>();
        SpawnElements();
    }

    public void SpawnElements()
    {
        int o = 0;
        float zOffset = 0, xOffset = 0, yOffset = 0;

        GameObject parent = new GameObject();
        parent.name = "BoardParent";
        
        for (int y = 0; y < yLength; y++)
        {
            yOffset = offset.y * y;
            for (int x = 0; x < xLength; x++)
            {
                if (type != BoardType.AlternateLines)
                    o = HandleCounter(o);

                xOffset = offset.x * x;
                for (int z = 0; z < zLength; z++)
                {
                    zOffset = offset.z * z;
                    var position = new Vector3(xOffset, yOffset, zOffset);

                    var piece = Instantiate(toSpawn[o], position, toSpawn[o].transform.rotation);

                    spawned.Add(piece.transform);

                    o = HandleCounter(o);
                }
            }
        }

        if (wantParentForTheBoard)
        {
            /*
             * This is tought for objects of equal size (my chess purpose). Offset between them is the size of the objects.
             * ex. cube 1 1 1 offset 1 1 1
             * ex. cube 2 2 2 offset 2 2 2 
             */
            if (wantParentPivotOnCenter)
            {
                var yT = yLength != 0 ? yLength : 1;
                var xT = xLength != 0 ? xLength : 1;
                var zT = zLength != 0 ? zLength : 1;

                float height = yLength * toSpawn[0].transform.localScale.y;
                float length = xLength * toSpawn[0].transform.localScale.x;
                float depth = zLength * toSpawn[0].transform.localScale.z;

                Vector3 offsetToRemove = new Vector3(toSpawn[0].transform.localScale.x, toSpawn[0].transform.localScale.y, toSpawn[0].transform.localScale.z) / 2;

                parent.transform.position = new Vector3(length / 2, height / 2, depth / 2) - offsetToRemove;
            }

            foreach (var spawnedObject in spawned)
                spawnedObject.transform.parent = parent.transform;

            parent.transform.position = startPosition.position;
            parent.transform.rotation = startPosition.rotation;
        }
    }

    int HandleCounter(int o) 
    {
        if (type == BoardType.Random)
            return Random.Range(0, toSpawn.Count);
        else
        {
            o++;
            if (o == toSpawn.Count)
                return 0;
            else
                return o;
        }

    }
}
