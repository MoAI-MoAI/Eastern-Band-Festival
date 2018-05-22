using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.Main
{
    public class ComboManager : MonoBehaviour
    {
        public Text text = null;
        public int combo = 0;
        public int maxCombo = 0;

        // Use this for initialization
        void Start()
        {
            this.gameObject.SetActive(false);
        }

        public void AddCombo()
        {
            if (this.combo == 0) this.gameObject.SetActive(true);
            this.combo++;
            if (this.combo > this.maxCombo) this.maxCombo = this.combo;
            this.text.text = this.combo.ToString();
        }

        public void LoseCombo()
        {
            this.gameObject.SetActive(false);
            this.combo = 0;
        }
    }
}