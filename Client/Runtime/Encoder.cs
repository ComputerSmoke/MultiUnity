using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using Multiunity.Shared;

public static class Encoder
{
    private static Multiunity.Shared.IdGenerator<int> ObjectIdEncodings = new IdGenerator<int>();
    public static Entity Encode(GameObject obj) {
        int id = ObjectIdEncodings.Assign(obj.GetInstanceID());
        if(obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D body)) 
            return EncodeBody(id, obj, body);
        return EncodeNoBody(id, obj);
    }
    private static Entity EncodeBody(int id, GameObject obj, Rigidbody2D body) {
        (float, float) pos = (obj.transform.position.x, obj.transform.position.y);
        float rotation = (float)Math.Acos(Quaternion.Dot(obj.transform.rotation, Quaternion.identity));
        (float, float) vel = (body.velocity.x, body.velocity.y);
        (float, float) accel = (
            body.totalForce.x / body.mass + Physics2D.gravity.x*body.gravityScale, 
            body.totalForce.y / body.mass + Physics2D.gravity.y*body.gravityScale
        );

        float rotv = body.angularVelocity;
        float rota = body.totalTorque / body.mass;
        Transform parent = obj.transform.parent;
        int parentId = 0;
        if(parent != null)
            parentId = ObjectIdEncodings.Assign(parent.GetInstanceID());
        Entity entity = new Entity(id, pos, vel, accel, rota, rotv, rota, parentId);
        entity.drag = body.drag;
        entity.angularDrag = body.angularDrag;
        return entity;
    }
    private static Entity EncodeNoBody(int id, GameObject obj) {
        (float, float) pos = (obj.transform.position.x, obj.transform.position.y);
        float rotation = (float)Math.Acos(Quaternion.Dot(obj.transform.rotation, Quaternion.identity));
        (float, float) vel = (0, 0);
        (float, float) accel = (0, 0);
        float rotv = 0;
        float rota = 0;
        Transform parent = obj.transform.parent;
        int parentId = 0;
        if(parent != null)
            parentId = ObjectIdEncodings.Assign(parent.GetInstanceID());
        return new Entity(id, pos, vel, accel, rota, rotv, rota, parentId);
    }
    public static int GetPrefabId(GameObject prefab) {
        return PrefabIdMap.IdByPrefab(prefab);
    }
}
