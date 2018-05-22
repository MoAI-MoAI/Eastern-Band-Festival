using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

using BandParty.Main;

namespace BandParty.Main
{
    public class TimeDisplay : MonoBehaviour
    {

        public Text text = null;
        public SoundManager soundManager = null;

        // Use this for initialization
        void Start()
        {
            StartCoroutine("DisplayTime");
        }

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator DisplayTime()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                float time = soundManager.time;
                StringBuilder sb = new StringBuilder();

                int minute = (int)Math.Floor(time / 60);
                int second = (int)Math.Floor(time) % 60;
                int msecond = (int)Math.Floor(time * 1000) % 1000;
                //int bar = (int)Math.Ceiling(time / (60 / soundManager.BPM * 4));
                //int beat = (int)Math.Ceiling(time % (60 / soundManager.BPM * 4) / (60 / soundManager.BPM));
                //sb.Append("小節:");
                //sb.Append(bar.ToString());
                //sb.Append(" 拍:");
                //sb.Append(beat.ToString());
                //sb.Append(" ");
                sb.Append(minute.ToString());
                sb.Append(":");
                sb.Append(second.ToString());
                sb.Append(":");
                sb.Append(msecond.ToString());
                sb.Append("/");

                time = soundManager.GetLength();
                minute = (int)Math.Floor(time / 60);
                second = (int)Math.Floor(time) % 60;
                msecond = (int)Math.Floor(time * 1000) % 1000;
                sb.Append(minute.ToString());
                sb.Append(":");
                sb.Append(second.ToString());
                sb.Append(":");
                sb.Append(msecond.ToString());

                this.text.text = sb.ToString();
            }
        }
    }
}

