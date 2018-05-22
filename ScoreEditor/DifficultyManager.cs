using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class DifficultyManager : MonoBehaviour
    {

        public SaveDataIO saveDataIO = null;

        private int value = 0;

        private void Start()
        {
            value = this.GetComponent<Dropdown>().value;
        }

        public void OnValueChanged()
        {
            saveDataIO.Save(this.GetValue());

            value = this.GetComponent<Dropdown>().value;

            saveDataIO.Load(this.GetValue());
        }

        public string GetValue()
        {
            switch (this.value)
            {
                case 0:
                    return "EASY";
                case 1:
                    return "NORMAL";
                case 2:
                    return "HARD";
                case 3:
                default:
                    return "EXPERT";
            }
        }
    }
}
