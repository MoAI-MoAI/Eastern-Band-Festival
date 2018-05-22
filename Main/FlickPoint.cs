using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

using BandParty.Main;

namespace BandParty.Main
{
    public class FlickPoint : MonoBehaviour
    {
        private FlickGesture gesture = null;

        private void OnEnable()
        {
            this.gesture = this.GetComponent<FlickGesture>();
            this.gesture.Flicked += this.onFlickedHandler;
        }

        private void OnDisable()
        {
            this.gesture.Flicked -= this.onFlickedHandler;
        }

        private void onFlickedHandler(object sender, System.EventArgs e)
        {
            Debug.Log(this.name + ": Flicked");
        }
    }
}

