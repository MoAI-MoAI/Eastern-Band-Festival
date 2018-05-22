using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BandParty.Main;

namespace BandParty.Main
{
    public class JudgeDisplay : MonoBehaviour
    {
        public float length = 2f;

        private Text text;
        private float time = 0f;
        private bool isShowing = false;

        // Use this for initialization
        void Start()
        {
            this.text = this.GetComponent<Text>();
            this.text.enabled = false;
            this.time = 0f;
            this.isShowing = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (isShowing)
            {
                this.time += Time.deltaTime;

                if (this.time >= this.length)
                {
                    this.text.enabled = false;
                    this.isShowing = false;
                }
            }
        }

        public void Launch(Common.JUDGE judge)
        {
            this.text.text = Common.JudgeToString(judge);
            this.text.enabled = true;
            this.time = 0f;
            this.isShowing = true;
        }
    }
}

