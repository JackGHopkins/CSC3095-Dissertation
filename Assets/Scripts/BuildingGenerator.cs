using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildingGenerator : MonoBehaviour
{
    public int minPieces = 1;
    public int maxPieces = 5;
    public GameObject[] baseParts;
    public GameObject[] middleParts;
    public GameObject[] topParts;

    public uint textureGridStepSizeX = 4;
    public uint textureGridStepSizeY = 4;
    public uint imgWidth = 512;
    public uint imgHeight = 512;
    [SerializeField] Texture2D texture;

    private bool positionValid;


    // Start is called before the first frame update
    void Awake()
    {
        Build(); 
    }

    void Build()
    {
        int targetPieces = Random.Range(minPieces, maxPieces);

        for (int i = minPieces - 1; i < maxPieces; i++)
        {
            SpawnBuilding(baseParts);
        }
    }

    void SpawnBuilding(GameObject[] buildingArray)
    {
        Transform randomTransform = buildingArray[Random.Range(0, buildingArray.Length - 1)].transform;
        GameObject clone = Instantiate(randomTransform.gameObject, GetSpawnPostion(), Quaternion.identity) as GameObject;
        Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().mesh;
        Bounds bounds = cloneMesh.bounds;
    }

    Vector3 GetSpawnPostion()
    {
        float scaleRX = this.transform.localScale.x / imgWidth;
        float scaleRZ = this.transform.localScale.z / imgHeight;

        positionValid = false;
        int x = 0;
        int z = 0;
        while (!positionValid) {
            x = Mathf.FloorToInt(Random.Range(1, imgWidth));
            z = Mathf.FloorToInt(Random.Range(1, imgHeight));

            if (texture.GetPixel(x,z) != Color.black)
            {
                positionValid = true;
            }

        }
        Vector3 position = new Vector3((x * scaleRX) - (this.transform.localScale.x / 2), 0, (z * scaleRZ) - (this.transform.localScale.x / 2));
        return position;
    }

    bool BoundsCheck(int x, int y)
    {
        int sizeX;

        return true;
    }
}
