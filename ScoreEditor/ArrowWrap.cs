using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class ArrowWrap : MonoBehaviour
    {

        public Transform parent = null;

        // Use this for initialization
        void Start()
        {
            this.transform.eulerAngles = new Vector3(-this.parent.eulerAngles.x, -this.parent.eulerAngles.y, -this.parent.eulerAngles.z);
        }

        // Update is called once per frame
        void Update()
        {
            this.transform.eulerAngles = new Vector3(-this.parent.eulerAngles.x, -this.parent.eulerAngles.y, -this.parent.eulerAngles.z);
        }
    }
}
