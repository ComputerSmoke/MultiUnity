using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PeerHandler
{
    static Dictionary<int, GameObject> peerObjects = new();

    public static void Create(int prefabId, Decoder.Entity entity) {
        GameObject prefab = PrefabIdMap.PrefabById(prefabId);
        GameObject newObject = UnityEngine.Object.Instantiate(prefab);
        peerObjects[entity.id] = newObject;
        Update(entity);
    }
    public static void Update(Decoder.Entity entity) {
        GameObject obj = peerObjects[entity.id];

        obj.transform.position = entity.pos;
        obj.transform.rotation = Quaternion.Euler(0, 0, entity.rot);

        if(peerObjects.ContainsKey(entity.parent))
            obj.transform.parent = peerObjects[entity.parent].transform;
        else
            obj.transform.parent = null;

        if(obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D body)) {
            body.velocity = entity.vel;
            body.angularVelocity = entity.rotV;
            //TODO: make subcomponent for adding force, calculated based off body mass and target accel. Update its parameters here, make it in Create method
        }
    }
    public static void Destroy(int id) {
        GameObject obj = peerObjects[id];
        UnityEngine.Object.Destroy(obj);
        peerObjects.Remove(id);
        //TODO: remove children from mapping, perhaps use EntityDictionary. (make the shared classes actually shared and beat the duplicate code problem)
    }
}
