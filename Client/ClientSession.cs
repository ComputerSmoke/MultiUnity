using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClientSession
{
    Dictionary<GameObject, int> prefabCodes;
    GameObject[] prefabs;
    public void SetPrefabIds(GameObject[] objects) {
        for(int i = 0; i < objects.Length; i++) {
            prefabs[i] = objects[i];
            prefabCodes[objects[i]] = i;
        }
    }
    public GameObject Instantiate(GameObject prefab) {
        GameObject obj = Object.Instantiate(prefab);
        Create(obj);
        return obj;
    }
    public GameObject Instantiate(GameObject prefab, Transform parent) {
        GameObject obj = Object.Instantiate(prefab, parent);
        Create(obj);
        return obj;
    }
    public GameObject Instantiate(GameObject prefab, Transform parent, bool instantiateInWorldSpace) {
        GameObject obj = Object.Instantiate(prefab, parent, instantiateInWorldSpace);
        Create(obj);
        return obj;
    }
    public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation) {
        GameObject obj = Object.Instantiate(prefab, position, rotation);
        Create(obj);
        return obj;
    }
    public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) {
        GameObject obj = Object.Instantiate(prefab, position, rotation, parent);
        Create(obj);
        return obj;
    }

}
