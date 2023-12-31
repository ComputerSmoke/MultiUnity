using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Multiunity.Client;
using Multiunity.Shared;


namespace Multiunity.Unity {
    public static class MultiSession
    {
        static Dictionary<GameObject, int> prefabCodes;
        static GameObject[] prefabs;
        static Client.Client client;
        static ClientSession session;
        static public bool connected;
        public static void Connect(int tcpPort) {
            session = new ClientSession();
            client = new Client.Client(tcpPort, session);
            connected = true;
        }
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
            obj.AddComponent(typeof(Updater));
            int prefabId = Encoder.GetPrefabId(prefab);
            Entity entity = Encoder.Encode(obj);
            client.Create(prefabId, entity);
        }
        public static void Join(int roomId) {
            client.Join(roomId);
        }
        public static void Update(GameObject obj) {
            Entity entity = Encoder.Encode(obj);
            client.Update(entity);
        }
        public static void Destroy(GameObject obj) {
            Entity entity = Encoder.Encode(obj);
            client.Destroy(entity.clientId);
            Object.Destroy(obj);
        }
        public static void Signal(Signal signal) {
            int id = IdMappings.IdBySignal(signal.gameObject);
            byte[] msg = signal.Message();
            string msgString = "";
            for (int i = 0; i < msg.Length; i++)
                msgString += msg[i] + ",";
            Debug.Log("Signaling. Id: " + id + "msg: " + msgString);
            client.Signal(id, msg);
        }
    }
}