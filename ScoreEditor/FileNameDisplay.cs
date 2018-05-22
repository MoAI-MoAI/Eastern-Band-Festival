using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.Editor
{
    public class FileNameDisplay : MonoBehaviour
    {
        public MusicFileManager musicFileManager = null;

        private Text text = null;
        private string prevText;

        // Use this for initialization
        void Start()
        {
            this.text = GetComponent<Text>();
            this.text.text = musicFileManager.GetFileName();
            this.prevText = musicFileManager.GetFileName();
        }

        // Update is called once per frame
        void Update()
        {
            if (!this.prevText.Equals(musicFileManager.GetFileName()))
            {
                this.text.text = musicFileManager.GetFileName();
                this.prevText = musicFileManager.GetFileName();
            }
        }
    }
}
