using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BandParty.Result
{
    public class ResultManager : MonoBehaviour
    {
        public RecordManager recordManager = null;
        public GameObject score = null;
        public GameObject maxCombo = null;
        public GameObject rank = null;
        public GameObject scoreRate = null;
        public GameObject breakdown = null;
        public InputField nickname = null;

        // Use this for initialization
        void Start()
        {
            score.transform.Find("Score").GetComponent<Text>().text = SongDataSender.result.score.ToString();
            maxCombo.transform.Find("MaxCombo").GetComponent<Text>().text = SongDataSender.result.maxCombo.ToString();
            rank.transform.Find("Rank").GetComponent<Text>().text = recordManager.GetRank(SongDataSender.result).ToString();
            scoreRate.transform.Find("ScoreRate").GetComponent<Text>().text = SongDataSender.result.scoreRate.ToString();
            breakdown.transform.Find("Perfect").GetComponent<Text>().text = SongDataSender.result.perfectCount.ToString();
            breakdown.transform.Find("Great").GetComponent<Text>().text = SongDataSender.result.greatCount.ToString();
            breakdown.transform.Find("Good").GetComponent<Text>().text = SongDataSender.result.goodCount.ToString();
            breakdown.transform.Find("Bad").GetComponent<Text>().text = SongDataSender.result.badCount.ToString();
            breakdown.transform.Find("Miss").GetComponent<Text>().text = SongDataSender.result.missCount.ToString();
        }

        public void ExitResult()
        {
            SongDataSender.result.nickname = nickname.text;

            this.recordManager.RecordResult(SongDataSender.result);

            SceneManager.LoadScene("scenes/selectSong");
        }
    }
}