using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeGenerator : MonoBehaviour
{
    public static CubeGenerator instance = null;

    public uint textureGridStepSizeX = 4;
    public uint textureGridStepSizeY = 4;
    public uint imgWidth = 512;
    public uint imgHeight = 512;

    public GameObject vCube;
    [SerializeField] float vHeightScale = 5.0f;
    public RawImage rawImage;
    public bool vGrid;

    [SerializeField] Texture2D texture;

    public object GeneratedObjectControl { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (vGrid)
        {
            VisualizeGrid();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float SampleStepped(int x, int y)
    {
        uint gridStepSizeX = imgWidth / textureGridStepSizeX;
        uint gridStepSizeY = imgHeight / textureGridStepSizeY;

        float sampledFloat = texture.GetPixel
                   ((Mathf.FloorToInt(x * gridStepSizeX)), (Mathf.FloorToInt(y * gridStepSizeX))).grayscale;

        return sampledFloat;
    }

    void VisualizeGrid()
    {
        GameObject vParent = new GameObject("VParent");
        vParent.transform.SetParent(this.transform);

        for (int x = 0; x < textureGridStepSizeX; x++)
        {
            for (int y = 0; y < textureGridStepSizeY; y++)
            {
                GameObject clone = Instantiate(vCube,
                    new Vector3(x, SampleStepped(x, y) * vHeightScale, y)
                    + transform.position, transform.rotation);

                clone.transform.SetParent(vParent.transform);
                instance.AddObject(clone);
            }
        }
        //visualizationParent.transform.position =
        //new Vector3(-perlinGridStepSizeX * .5f, -visualizationHeightScale * .5f, -perlinGridStepSizeY * .5f);
    }

    private void AddObject(GameObject clone)
    {
        throw new NotImplementedException();
    }
}
