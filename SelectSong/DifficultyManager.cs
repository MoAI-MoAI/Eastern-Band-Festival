using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.SelectSong
{
    public class DifficultyManager : MonoBehaviour
    {
        public Text difficultyText = null;
        public Text speedText = null;
        public Common.DIFFICULTY difficulty = Common.DIFFICULTY.NORMAL;
        public Common.SPEED speed = Common.SPEED.NORMAL;

        // Use this for initialization
        void Start()
        {
            this.difficultyText.text = this.difficulty.ToString();
            this.speedText.text = this.speed.ToString();
        }

        public void SetDifficulty(int difficulty)
        {
            this.difficulty = (Common.DIFFICULTY)difficulty;
            this.difficultyText.text = this.difficulty.ToString();
        }

        public void SetSpeed(int speed)
        {
            this.speed = (Common.SPEED)speed;
            this.speedText.text = this.speed.ToString();
        }
    }
}