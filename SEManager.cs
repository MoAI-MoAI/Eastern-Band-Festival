using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BandParty
{
    public class SEManager : MonoBehaviour
    {
        public AudioClip se0;
        public AudioClip se1;
        public AudioClip se2;
        public AudioClip se3;
        public AudioClip se4;

        private AudioSource[] audioSources;
        private int audioSourceIndex = 0;
        private int audioSourceLoopIndex = 3;
        private List<int> loopIndexes;

        // Use this for initialization
        void Start()
        {
            this.audioSources = this.GetComponents<AudioSource>();
            this.audioSourceIndex = 0;
            this.audioSourceLoopIndex = 3;
            this.loopIndexes = new List<int>();
        }

        public void Play(int seIndex) 
        {
            switch (seIndex)
            {
                case 0:
                    this.audioSources[this.audioSourceIndex].PlayOneShot(this.se0);
                    break;
                case 1:
                    this.audioSources[this.audioSourceIndex].PlayOneShot(this.se1);
                    break;
                case 2:
                    this.audioSources[this.audioSourceIndex].PlayOneShot(this.se2);
                    break;
                case 3:
                    this.audioSources[this.audioSourceIndex].PlayOneShot(this.se3);
                    break;
                case 4:
                    this.audioSources[this.audioSourceIndex].PlayOneShot(this.se4);
                    break;
            }

            this.audioSourceIndex++;
            if (this.audioSourceIndex >= this.audioSources.Length - 2) this.audioSourceIndex = 0;
        }

        public void PlayLoop(int seIndex)
        {
            switch (seIndex)
            {
                case 0:
                    this.audioSources[this.audioSourceLoopIndex].PlayOneShot(this.se0);
                    break;
                case 1:
                    this.audioSources[this.audioSourceLoopIndex].PlayOneShot(this.se1);
                    break;
                case 2:
                    this.audioSources[this.audioSourceLoopIndex].PlayOneShot(this.se2);
                    break;
                case 3:
                    this.audioSources[this.audioSourceLoopIndex].PlayOneShot(this.se3);
                    break;
                case 4:
                    this.audioSources[this.audioSourceLoopIndex].PlayOneShot(this.se4);
                    break;
            }
            this.loopIndexes.Remove(this.audioSourceLoopIndex);
            this.loopIndexes.Add(this.audioSourceLoopIndex);

            this.audioSourceLoopIndex++;
            if (this.audioSourceLoopIndex >= this.audioSources.Length) this.audioSourceLoopIndex = 3;
        }

        public void StopLoop()
        {
            if (this.loopIndexes.Count == 0) return;
            this.audioSources[this.loopIndexes[0]].Stop();
            this.loopIndexes.Remove(this.loopIndexes[0]);
        }
    }
}