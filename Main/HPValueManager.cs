using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.Main
{
    public class HPValueManager : MonoBehaviour
    {
        public Slider HpBar = null;
        public Text text = null;
        public int HpMax = 1000;

        public void UpdateValue(int hp)
        {
            this.text.text = hp.ToString() + "/" + this.HpMax;
            this.HpBar.normalizedValue = (float)hp / this.HpMax;
        }
    }
}

