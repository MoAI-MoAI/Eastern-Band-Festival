using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BandParty.TimingAdjuster
{
    public class GameManager : MonoBehaviour
    {
        public FileManager fileManager = null;
        public Main.SoundManager soundManager = null;
        public Judge judge = null;

        private enum PHASE
        {
            LOAD,
            READYTOPLAY,
            PLAY
        }

        private float time = 0f;
        private PHASE phase = PHASE.LOAD;

        // Use this for initialization
        void Start()
        {
            this.time = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            this.time += Time.deltaTime;

            switch (this.phase)
            {
                case PHASE.LOAD:
                    if (this.time >= 0f)
                    {
                        fileManager.Load();
                        judge.Init();
                        Debug.Log(this.name + ": ChangePhase LOAD-READYTOPLAY");
                        this.phase = PHASE.READYTOPLAY;
                    }
                    break;
                case PHASE.READYTOPLAY:
                    if (this.time >= 1f)
                    {
                        soundManager.Play();
                        Debug.Log(this.name + ": ChangePhase READYTOPLAY-PLAY");
                        this.phase = PHASE.PLAY;
                    }
                    break;
                case PHASE.PLAY:
                    break;
            }
        }

        public void GotoTimingGetter()
        {
            this.fileManager.SaveConfig();
            SceneManager.LoadScene("timingGetter");
        }

        public void GotoMain()
        {
            this.fileManager.SaveConfig();
            SceneManager.LoadScene("main");
        }
    }
}

