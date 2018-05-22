using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BandParty
{
    public class LookAtCamera : MonoBehaviour
    {

        public Camera targetCamera;
        public bool fixX = false;
        public bool fixY = false;
        public bool fixZ = false;

        // Use this for initialization
        void Start()
        {
            if (this.targetCamera == null)
                targetCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 p = targetCamera.transform.position;

            if (fixX) p.x = transform.position.x;
            if (fixY) p.y = transform.position.y;
            if (fixZ) p.z = transform.position.z;

            this.transform.LookAt(p);

        }
    }
}