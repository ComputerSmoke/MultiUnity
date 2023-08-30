using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PrefabIdMap
{
    static GameObject[] prefabs;
    public static void SetPrefabs(GameObject[] newPrefabs) {
        prefabs = new GameObject[newPrefabs.Length];
        for(int i = 0; i < prefabs.Length; i++) prefabs[i] = newPrefabs[i];
    }
    public static GameObject PrefabById(int id) {
        return prefabs[id];
    }
    public static int IdByPrefab(GameObject prefab) {
        return Array.IndexOf(prefabs, prefab);
    }
}
