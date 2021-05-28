using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MapManager : MonoBehaviour
{

    [SerializeField] Dictionary<Vector3Int, GameObject> cubeDictionary = new Dictionary<Vector3Int, GameObject>();
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    private void AddCube(GameObject cube)
    {

        Vector3Int key = new Vector3Int();
        key.x = (int)cube.transform.position.x;
        key.y = (int)cube.transform.position.y;
        key.z = (int)cube.transform.position.z;

        if (cubeDictionary[key] == null)
        {
            cubeDictionary[key] = cube;
        }
        else 
        {
            Destroy(cube);
        }


    }
    private void AddObject(GameObject objectToAdd)
    {
        objects.Add(objectToAdd);
    }



}
