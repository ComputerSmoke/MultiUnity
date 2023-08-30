using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClientSession
{
    static Dictionary<GameObject, int> prefabCodes;
    static GameObject[] prefabs;
    public static void SetPrefabIds(GameObject[] objects) {
        for(int i = 0; i < objects.Length; i++) {
            prefabs[i] = objects[i];
            prefabCodes[objects[i]] = i;
        }
    }
    public static GameObject Instantiate(GameObject prefab) {
        GameObject obj = Object.Instantiate(prefab);
        Create(obj, prefab);
        return obj;
    }
    public static GameObject Instantiate(GameObject prefab, Transform parent) {
        GameObject obj = Object.Instantiate(prefab, parent);
        Create(obj, prefab);
        return obj;
    }
    public static GameObject Instantiate(GameObject prefab, Transform parent, bool instantiateInWorldSpace) {
        GameObject obj = Object.Instantiate(prefab, parent, instantiateInWorldSpace);
        Create(obj, prefab);
        return obj;
    }
    public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation) {
        GameObject obj = Object.Instantiate(prefab, position, rotation);
        Create(obj, prefab);
        return obj;
    }
    public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) {
        GameObject obj = Object.Instantiate(prefab, position, rotation, parent);
        Create(obj, prefab);
        return obj;
    }
    private static void Create(GameObject obj, GameObject prefab) {
        byte[] buf = Encoder.Create(obj, prefab);
        SocketHandler.Send(buf);
    }

}
