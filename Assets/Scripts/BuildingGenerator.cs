using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

namespace Assets.Scripts
{
    [System.Serializable]
    public class Building
    {
        public string name;
        public int buildingSpacing;
        public GameObject[] meshes;
    }

    public class MeshArray : MonoBehaviour
    {
        public Building[] meshArray;
    }

    public enum FloodFillAlgorithm
    {
        BRUTE_FORCE = 0,
        FOUR_WAY_RECURSION = 1,
        FOUR_WAY_LINEAR = 2,
        SPAN_FILL = 3,
        WALK_BASED_FILL = 4,
        PERIMETER_FILL = 5,
    }

    public enum DataStructure
    {
        STACK = 0,
        QUEUE = 1,
    }

    public class BuildingGenerator : MonoBehaviour
    {
        public float multipledImgSize = 1.0f;

        public FloodFillAlgorithm algorithm = FloodFillAlgorithm.FOUR_WAY_LINEAR;
        public DataStructure dataStruct = DataStructure.STACK;
        private int buildingNo;

        [SerializeField] Texture2D texture;
        [SerializeField] Building[] meshArray;
        [SerializeField] Color32[] colours;

        Stopwatch stopWatch = new Stopwatch();

        // Start is called before the first frame update
        void Awake()
        {
            transform.localScale += new Vector3(texture.width, -0.5f, texture.height);
            Main();
        }

        void Main()
        {
            FloodFill ff = gameObject.AddComponent<FloodFill>();

            for (int i = 0; i < colours.Length; i++)
            {
                if (dataStruct == DataStructure.STACK)
                {
                    Stack<Vector2> shape = new Stack<Vector2>();
                    shape = ff.FFStack(shape, texture, texture.height, texture.width, colours[i], algorithm, stopWatch);
                    //sortShape(shape);

                    //for (int j = 0; j < shape.Count;)
                    //{
                    //    SpawnBuilding(meshArray[i].meshes, shape.ToArray()[j]);

                    //    j = j + meshArray[i].buildingSpacing;
                    //}
                }
                else if (dataStruct == DataStructure.QUEUE)
                {
                    Queue<Vector2> shape = new Queue<Vector2>();
                    shape = ff.FFQueue(shape, texture, texture.height, texture.width, colours[i], algorithm, stopWatch);
                    sortShape(shape);

                    for (int j = 0; j < shape.Count;)
                    {
                        SpawnBuilding(meshArray[i].meshes, shape.ToArray()[j]);

                        j = j + meshArray[i].buildingSpacing;
                    }
                }
            }
            print(stopWatch.ElapsedMilliseconds);
        }

        void SpawnBuilding(GameObject[] buildingArray, Vector2 position)
        {
            buildingNo = Random.Range(0, buildingArray.Length - 1);
            Transform randomTransform = buildingArray[buildingNo].transform;
            GameObject clone = Instantiate(randomTransform.gameObject, new Vector3((position.x + 0.5f) * multipledImgSize, 0, (position.y + 0.5f) * multipledImgSize), Quaternion.identity) as GameObject;
            Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().mesh;
            Bounds bouasdfasnds = cloneMesh.bounds;
        }

        // Turns Stack / Queue into an Array
        void sortShape(Stack<Vector2> shape)
        {
            sortShape(shape.ToArray());
        }
    
        void sortShape(Queue<Vector2> shape)
        {
            sortShape(shape.ToArray());
        }

        // Sorts Shapes to be ordered counter clockwise.
        void sortShape(Vector2[] shape)
        {
            for (int i = 0; i < shape.Length - 1; i++)
            {
                int yAxis = 0;
                int diagonal = 0;
                // Continue if next coord is [x+1 || x-1,y]
                if (shape[i].y == shape[i + 1].y && (shape[i].x + 1 == shape[i + 1].x || shape[i].x - 1 == shape[i + 1].x))
                    continue;

                for (int j = i + 2; j < shape.Length; j++)
                {
                    if (shape[i].y == shape[j].y && (shape[i].x + 1 == shape[j].x || shape[i].x - 1 == shape[j].x))
                    {
                        Vector2 temp = shape[i + 1];
                        shape[i + 1] = shape[j];
                        shape[j] = temp;
                        break;
                    }

                    if (shape[i].x == shape[j].x && (shape[i].y + 1 == shape[j].y || shape[i].y - 1 == shape[j].y))
                    {
                        yAxis = j;
                        continue;
                    }

                    // Continue if next coord is [x+1 || x-1, y+1 || y-1]
                    if ((shape[i].x + 1 == shape[i + 1].x || shape[i].x - 1 == shape[i + 1].x) && (shape[i].y + 1 == shape[i + 1].y || shape[i].y - 1 == shape[i + 1].y))
                        diagonal = j;
                }

                // Continue if next coord is [x, y+1 || y-1]
                if (shape[i].x == shape[i + 1].x && (shape[i].y + 1 == shape[i + 1].y || shape[i].y - 1 == shape[i + 1].y))
                    continue;

                if (shape[i].x == shape[yAxis].x && (shape[i].y + 1 == shape[yAxis].y || shape[i].y - 1 == shape[yAxis].y))
                {
                    Vector2 temp = shape[i + 1];
                    shape[i + 1] = shape[yAxis];
                    shape[yAxis] = temp;
                    continue;
                }

                // Continue if next coord is [x+1 || x-1, y+1 || y-1]
                if ((shape[i].x + 1 == shape[i + 1].x || shape[i].x - 1 == shape[i + 1].x) && (shape[i].y + 1 == shape[i + 1].y || shape[i].y - 1 == shape[i + 1].y))
                    continue;

                if ((shape[i].x + 1 == shape[diagonal].x || shape[i].x - 1 == shape[diagonal].x) && (shape[i].y + 1 == shape[diagonal].y || shape[i].y - 1 == shape[diagonal].y))
                {
                    Vector2 temp = shape[i + 1];
                    shape[i + 1] = shape[diagonal];
                    shape[diagonal] = temp;
                    continue;
                }
            }
        }
    }
}

 