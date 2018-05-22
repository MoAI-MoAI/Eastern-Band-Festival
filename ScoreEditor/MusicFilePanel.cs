using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class MusicFilePanel : MonoBehaviour
    {

        public MusicFileManager.MusicFile musicFile;
        public MusicFileManager musicFileManager = null;

        private Text fileNameText = null;
        private GameObject easyText = null;
        private GameObject normalText = null;
        private GameObject hardText = null;
        private GameObject expertText = null;

        public void Launch(MusicFileManager.MusicFile musicFile, MusicFileManager musicFileManager)
        {
            this.musicFile = musicFile;
            this.musicFileManager = musicFileManager;

            this.fileNameText = this.transform.Find("FileName").GetComponent<Text>();
            this.easyText = this.transform.Find("Easy").gameObject;
            this.normalText = this.transform.Find("Normal").gameObject;
            this.hardText = this.transform.Find("Hard").gameObject;
            this.expertText = this.transform.Find("Expert").gameObject;
            this.fileNameText.text = this.musicFile.filePath.Substring(Mathf.Max(this.musicFile.filePath.LastIndexOf('/'), this.musicFile.filePath.LastIndexOf('\\')) + 1);
            this.easyText.SetActive(this.musicFile.difficultyExists[0]);
            this.normalText.SetActive(this.musicFile.difficultyExists[1]);
            this.hardText.SetActive(this.musicFile.difficultyExists[2]);
            this.expertText.SetActive(this.musicFile.difficultyExists[3]);
        }

        public void OnTappedHandler()
        {
            musicFileManager.OpenFile(this.musicFile.filePath);
        }
    }
}
