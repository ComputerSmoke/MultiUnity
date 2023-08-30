using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject baller;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] prefabs = new GameObject[] {baller};
        PrefabIdMap.SetPrefabs(prefabs);
        ClientSession.Join(1);
        System.Random rand = new();
        ClientSession.Instantiate(baller, new Vector3(rand.Next(10), 0, 0), Quaternion.Euler(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
