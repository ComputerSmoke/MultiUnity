using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Multiunity.Unity {
    public abstract class Signal : MonoBehaviour
    {
        public GameObject prefab;
        public bool external;
        public abstract void Init(byte[] msg, GameObject prefab);
        public abstract byte[] Message();
        public abstract void Execute();
        protected void Forward() {
            if(external || !MultiSession.connected)
                return;
            MultiSession.Signal(this);
        }
        //To use half the byte for negative numbers
        protected static byte Shrink(int x) {
            if(x >= 0) 
                return (byte)x;
            return (byte)(255+x);
        }
        protected static int Grow(byte x) {
            if(x < 128) 
                return (int)x;
            return (int)x-255;
        }
    }
}