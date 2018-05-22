using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class SeekBarManager : MonoBehaviour
    {

        public Slider slider = null;
        public SoundManager soundManager = null;

        private IEnumerator coroutineMethod = null;

        // Use this for initialization
        void Start()
        {
            this.coroutineMethod = MonitoringPlayTime();
            StartCoroutine(this.coroutineMethod);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnDown()
        {
            StopCoroutine(this.coroutineMethod);
        }

        public void OnUp()
        {
            this.soundManager.SetTime(this.slider.value * this.soundManager.GetLength());
            StartCoroutine(this.coroutineMethod);
        }

        private IEnumerator MonitoringPlayTime()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                //if (!this.soundManager.IsPlaying()) continue;
                if (this.soundManager.GetTime() >= 0.0f)
                {
                    this.slider.value = this.soundManager.GetTime() / this.soundManager.GetLength();
                }
                else
                {
                    this.slider.value = 0.0f;
                }
            }
        }
    }
}