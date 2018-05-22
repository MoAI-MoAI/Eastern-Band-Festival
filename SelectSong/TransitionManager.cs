using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BandParty.SelectSong
{
    public class TransitionManager : MonoBehaviour
    {
        public JacketManager jacketManager;
        public DifficultyManager difficultyManager;
        public GameObject selectSong;
        public GameObject selectDifficulty;

        private GameObject endingObject;
        private Animator endingAnim;

        // Use this for initialization
        void Start()
        {
            this.selectSong.SetActive(true);
            this.selectDifficulty.SetActive(false);
        }

        public void GotoSelectDifficulty()
        {
            SongDataSender.songData = this.jacketManager.GetSongData();
            this.endingObject = this.selectSong;
            this.endingAnim = this.selectSong.GetComponent<Animator>();
            this.endingAnim.Play(Animator.StringToHash("SelectSongFadeOut"));
            StartCoroutine("WaitForAnim");
            this.jacketManager.Stop();
            this.selectDifficulty.SetActive(true);
        }

        public void GotoSelectSong()
        {
            this.selectSong.SetActive(true);
            this.jacketManager.ReStart();
            this.endingObject = this.selectDifficulty;
            this.endingAnim = this.selectDifficulty.GetComponent<Animator>();
            this.endingAnim.Play(Animator.StringToHash("SelectDifficultyFadeOut"));
            StartCoroutine("WaitForAnim");
        }

        public void GameStart()
        {
            SongDataSender.songData = this.jacketManager.GetSongData();
            SongDataSender.difficulty = this.difficultyManager.difficulty;
            SongDataSender.speed = this.difficultyManager.speed;

            SceneManager.LoadScene("scenes/main");
        }

        public void GotoTimingGetter()
        {
            SceneManager.LoadScene("scenes/timingGetter");
        }

        private IEnumerator WaitForAnim()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                if (this.endingAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) break;
            }
            this.endingObject.SetActive(false);
        }
    }
}