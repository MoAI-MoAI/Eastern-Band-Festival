using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using BandParty;

namespace BandParty.Main
{
    public class GameManager : MonoBehaviour
    {
        public FileManager fileManager = null;
        public SoundManager soundManager = null;
        public Judge judge = null;
        public ScoreManager scoreManager = null;
        public GameObject countDown = null;
        public GameObject gameOverDisplay = null;

        private Animator countDownAnim = null;

        private enum PHASE
        {
            LOAD,
            READYTOPLAY,
            PLAY,
            PAUSE,
            RESTARTCOUNT,
            INTERRUPTION,
            GAMEOVER,
            MUSICEND,
            EXIT,
            DIED
        }

        private float time = 0f;
        private PHASE phase = PHASE.LOAD;

        // Use this for initialization
        void Start()
        {
            this.time = 0f;
            this.gameOverDisplay.SetActive(false);

            this.countDownAnim = this.countDown.transform.Find("Image").GetComponent<Animator>();
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
                        this.fileManager.Load();
                        this.scoreManager.SetMaxScore();
                        Debug.Log(this.name + ": ChangePhase LOAD-READYTOPLAY");
                        this.phase = PHASE.READYTOPLAY;
                    }
                    break;
                case PHASE.READYTOPLAY:
                    if (this.time >= 1f)
                    {
                        this.soundManager.Play();
                        Debug.Log(this.name + ": ChangePhase READYTOPLAY-PLAY");
                        this.phase = PHASE.PLAY;
                    }
                    break;
                case PHASE.PLAY:
                    break;
                case PHASE.PAUSE:
                    break;
                case PHASE.INTERRUPTION:
                    SceneManager.LoadScene("scenes/selectSong");

                    this.phase = PHASE.DIED;
                    break;
                case PHASE.GAMEOVER:
                    break;
                case PHASE.RESTARTCOUNT:
                    break;
                case PHASE.MUSICEND:
                    Debug.Log(this.name + ": ChangePhase MUSICEND-EXIT");
                    this.phase = PHASE.EXIT;
                    break;
                case PHASE.EXIT:
                    SongDataSender.result = scoreManager.GetResult();
                    SceneManager.LoadScene("scenes/result");
                    
                    this.phase = PHASE.DIED;
                    break;
            }
        }

        public void Pause()
        {
            this.soundManager.Pause();
            this.phase = PHASE.PAUSE;
        }

        public void ReStart()
        {
            this.countDown.SetActive(true);
            StartCoroutine("CountDownWait");
            this.phase = PHASE.RESTARTCOUNT;
        }

        public void MusicEnd()
        {
            Debug.Log(this.name + ": ChangePhase " + this.phase.ToString() + "-MUSICEND" );
            this.phase = PHASE.MUSICEND;
        }

        public void Interruption()
        {
            this.phase = PHASE.INTERRUPTION;
        }

        public void GameOver()
        {
            this.gameOverDisplay.SetActive(true);
            this.soundManager.Pause();

            this.phase = PHASE.GAMEOVER;
        }

        public void GotoTimingGetter()
        {
            SceneManager.LoadScene("scenes/timingGetter");
        }

        private IEnumerator CountDownWait()
        {
            this.countDownAnim.Play(Animator.StringToHash("countDown"));
            while (true)
            {
                yield return new WaitForFixedUpdate();
                if (this.countDownAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) break;
            }
            this.countDown.SetActive(false);
            this.soundManager.ReStart();
            this.phase = PHASE.PLAY;
        }
    }
}

