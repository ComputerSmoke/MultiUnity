using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Multiunity.Shared;

namespace Multiunity.Unity {
    public class ClientSession : ISession
    {
        Dictionary<int, GameObject> peerObjects;
        public ClientSession() {
            peerObjects = new();
        }

        public void Create(Entity entity) {
            Debug.Log("Creating");
            GameObject prefab = IdMappings.PrefabById(entity.prefabId);
            GameObject newObject = UnityEngine.Object.Instantiate(prefab);
            peerObjects[entity.clientId] = newObject;
            Update(entity);
        }
        public void Update(Entity entity) {
            GameObject obj = peerObjects[entity.clientId];

            obj.transform.position = new Vector3(entity.pos.Item1, entity.pos.Item2, 0);
            obj.transform.rotation = Quaternion.Euler(0, 0, entity.rot);

            if(peerObjects.ContainsKey(entity.parent))
                obj.transform.parent = peerObjects[entity.parent].transform;
            else
                obj.transform.parent = null;

            if(obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D body)) {
                body.velocity = new Vector3(entity.vel.Item1, entity.vel.Item2, 0);
                body.angularVelocity = entity.rotv;
                //TODO: make subcomponent for adding force, calculated based off body mass and target accel. Update its parameters here, make it in Create method
            }
        }
        public void Destroy(int id) {
            GameObject obj = peerObjects[id];
            UnityEngine.Object.Destroy(obj);
            peerObjects.Remove(id);
            //TODO: remove children from mapping, perhaps use EntityDictionary. (make the shared classes actually shared and beat the duplicate code problem)
        }
        public void Signal(int id, byte[] msg) {
            Debug.Log("making signal: " + id);
            GameObject prefab = IdMappings.SignalById(id);
            Debug.Log("prefab is null: " + (prefab == null));
            GameObject signalObject = UnityEngine.Object.Instantiate(prefab);
            Signal signal = signalObject.GetComponent<Signal>();
            Debug.Log("intializing");
            signal.Init(msg, prefab);
            signal.external = true;
            signal.Execute();
        }
        public void Join(int roomId) {

        }
    }
}
