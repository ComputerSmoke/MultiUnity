using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Multiunity.Unity {
    public abstract class EmptySignal : Signal
    {
        public override void Init(byte[] msg, GameObject prefab) {
            this.prefab = prefab;
        }
        public override byte[] Message() {
            List<byte> res = new List<byte>();
            return res.ToArray();
        }
        public void Init(GameObject prefab) {
            this.prefab = prefab;
        }
    }
}