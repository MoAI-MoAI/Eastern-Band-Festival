using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BandParty.Main
{
    public class Effect : MonoBehaviour
    {
        private Animator animator;

        // Use this for initialization
        void Start()
        {
            this.animator = this.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Died"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}