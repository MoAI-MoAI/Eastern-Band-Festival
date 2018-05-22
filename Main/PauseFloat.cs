using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BandParty.Main
{
    public class PauseFloat : MonoBehaviour
    {
        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}