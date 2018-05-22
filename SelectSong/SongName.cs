using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.SelectSong
{
    public class SongName : MonoBehaviour
    {
        private Text text;

        // Use this for initialization
        void Start()
        {
            this.text = this.GetComponent<Text>();
            this.text.text = SongDataSender.songData.songName + " >>";
        }

        // Update is called once per frame
        void Update()
        {
            this.text.text = SongDataSender.songData.songName + " >>";
        }
    }
}