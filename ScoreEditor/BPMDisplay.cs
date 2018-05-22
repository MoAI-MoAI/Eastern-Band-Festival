using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.Editor
{
    public class BPMDisplay : MonoBehaviour
    {

        public SoundManager soundManager;

        private Text text;

        // Use this for initialization
        void Start()
        {
            text = this.GetComponent<Text>();
            text.text = "Quantize(BPM=" + soundManager.BPM.ToString() + ")";
        }

        // Update is called once per frame
        void Update()
        {
            text.text = "Quantize(BPM=" + soundManager.BPM.ToString() + ")";
        }
    }
}
