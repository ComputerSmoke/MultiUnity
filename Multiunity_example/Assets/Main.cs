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
        ClientSession.Instantiate(baller);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
