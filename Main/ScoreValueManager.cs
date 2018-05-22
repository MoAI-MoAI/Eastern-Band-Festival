using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.Main
{
    public class ScoreValueManager : MonoBehaviour
    {
        public Text text = null;
        public Slider scoreBar = null;
        public int scoreMax = 100000;
        
        public void UpdateValue(int score)
        {
            this.text.text = score.ToString("D8");
            this.scoreBar.normalizedValue = (float)score / this.scoreMax;
        }
    }
}