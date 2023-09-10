using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Multiunity.Unity {
    public static class IdMappings
    {
        static GameObject[] prefabs;
        static GameObject[] signals;
        public static void SetPrefabs(GameObject[] newPrefabs) {
            prefabs = newPrefabs;
        }
        public static GameObject PrefabById(int id) {
            return prefabs[id];
        }
        public static int IdByPrefab(GameObject prefab) {
            return Array.IndexOf(prefabs, prefab);
        }
        public static void SetSignals(GameObject[] newSignals) {
            signals = newSignals;
        }
        public static GameObject SignalById(int id) {
            return signals[id];
        }
        public static int IdBySignal(GameObject signal) {
            return Array.IndexOf(signals, signal.GetComponent<Signal>().prefab);
        }
    }
}