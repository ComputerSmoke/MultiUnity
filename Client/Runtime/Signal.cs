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
            if(external)
                return;
            MultiSession.Signal(this);
        }
    }
}