using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Multiunity.Shared;

public static class PeerHandler
{
    static Dictionary<int, GameObject> peerObjects = new();

    public static int Create(int prefabId, Entity entity) {
        GameObject prefab = PrefabIdMap.PrefabById(prefabId);
        GameObject newObject = UnityEngine.Object.Instantiate(prefab);
        peerObjects[entity.clientId] = newObject;
        Update(entity);
        return 0;
    }
    public static int Update(Entity entity) {
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
        return 0;
    }
    public static int Destroy(int id) {
        GameObject obj = peerObjects[id];
        UnityEngine.Object.Destroy(obj);
        peerObjects.Remove(id);
        //TODO: remove children from mapping, perhaps use EntityDictionary. (make the shared classes actually shared and beat the duplicate code problem)
        return 0;
    }
}
