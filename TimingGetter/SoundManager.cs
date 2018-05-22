using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BandParty.TimingGetter
{
    public class SoundManager : MonoBehaviour
    {
        public float BPM = 120f;
        public float time = 0f;

        private AudioSource audioSource = null;
        private float prevTime = 0f;

        // Use this for initialization
        void Start()
        {
            this.audioSource = this.GetComponent<AudioSource>();
            this.audioSource.Play();
        }

        // Update is called once per frame
        void Update()
        {
            float addTime;
            if (this.prevTime <= this.audioSource.time)
            {
                addTime = this.audioSource.time - this.prevTime;
            } else
            {
                addTime = (this.audioSource.clip.length - this.prevTime) + this.audioSource.time;
            }
            this.prevTime = this.audioSource.time;
            this.time += addTime;
        }
    }
}