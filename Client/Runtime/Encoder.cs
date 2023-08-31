using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using Multiunity.Shared;

public static class Encoder
{
    public enum NetCodes {
        JOIN_ROOM,
        CREATE,
        UPDATE,
        DESTROY
    }
    private static Multiunity.Shared.IdGenerator<int> ObjectIdEncodings = new IdGenerator<int>();
    public static byte[] Create(GameObject obj, GameObject prefab) {
        List<byte> encoding = new List<byte>();
        encoding.Add((byte)NetCodes.CREATE);
        AppendInt16(PrefabIdMap.IdByPrefab(prefab), encoding);
        List<byte> updateEncoding = ObjectEncoding(obj);
        encoding.AddRange(updateEncoding);
        return encoding.ToArray();
    }
    private static void AppendInt16(int n, List<byte> buf) {
        buf.Add((byte)(n >> 8));
        buf.Add((byte)(n & 0xff));
    }
    private static void AppendFloat(float f, List<byte> buf) {
        byte[] floatBytes = BitConverter.GetBytes(f);
        for(int i = 0; i < 4; i++)
            buf.Add((floatBytes[i]));
    }
    //default velocity, accel, rotational velocity, rotational accel
    private static List<byte> NoBodyEncoding() {
        List<byte> encoding = new List<byte>();
        AppendFloat(0f, encoding);
        AppendFloat(0f, encoding);
        AppendFloat(0f, encoding);
        AppendFloat(0f, encoding);
        AppendFloat(0f, encoding);
        AppendFloat(0f, encoding);
        return encoding;
    }
    private static List<byte> BodyEncoding(Rigidbody2D body) {
        List<byte> encoding = new List<byte>();
        float vx = body.velocity.x;
        float vy = body.velocity.y;
        float vr = body.angularVelocity;
        float ax = body.totalForce.x / body.mass;
        float ay = body.totalForce.y / body.mass;
        float ar = body.totalTorque / body.mass;
        AppendFloat(vx, encoding);
        AppendFloat(vy, encoding);
        AppendFloat(ax, encoding);
        AppendFloat(ay, encoding);
        AppendFloat(vr, encoding);
        AppendFloat(ar, encoding);
        return encoding;
    }
    private static List<byte> ObjectEncoding(GameObject obj) {
        int id = ObjectIdEncodings.Assign(obj.GetInstanceID());
        List<byte> encoding = new List<byte>();
        AppendInt16(id, encoding);
        AppendFloat(obj.transform.position.x, encoding);
        AppendFloat(obj.transform.position.y, encoding);
        if(obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D body)) 
            encoding.AddRange(BodyEncoding(body));
        else
            encoding.AddRange(NoBodyEncoding());
        AppendFloat((float)Math.Acos(Quaternion.Dot(obj.transform.rotation, Quaternion.identity)), encoding);
        Transform parent = obj.transform.parent;
        if(parent == null)
            AppendInt16(0, encoding);
        else 
            AppendInt16(ObjectIdEncodings.Assign(parent.GetInstanceID()), encoding);
        return encoding;
    }
    public static byte[] Update(GameObject obj) {
        List<byte> encoding = new List<byte>();
        encoding.Add((byte)NetCodes.UPDATE);
        encoding.AddRange(ObjectEncoding(obj));
        return encoding.ToArray();
    }
    public static byte[] Join(int room) {
        List<byte> encoding = new();
        encoding.Add((byte)NetCodes.JOIN_ROOM);
        AppendInt16(room, encoding);
        return encoding.ToArray();
    }
    public static byte[] Destroy(GameObject obj) {
        List<byte> encoding = new();
        encoding.Add((byte)NetCodes.DESTROY);
        int id = ObjectIdEncodings.GetId(obj.GetInstanceID());
        AppendInt16(id, encoding);
        ObjectIdEncodings.Release(id);
        return encoding.ToArray();
    }
}
