using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiunity.Unity {
    public class IdMap : MonoBehaviour
    {
        public GameObject[] prefabs;
        public GameObject[] signals;
        // Start is called before the first frame update
        void Start()
        {
            IdMappings.SetPrefabs(prefabs);
            IdMappings.SetSignals(signals);
        }
    }
}