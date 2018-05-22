using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

namespace BandParty.SelectSong
{
    public class FlickPanel : MonoBehaviour
    {
        public JacketManager jacketManager = null;

        private FlickGesture gesture;

        private void OnEnable()
        {
            this.gesture = this.GetComponent<FlickGesture>();
            this.gesture.Flicked += onFlickedHandler;
        }

        private void onFlickedHandler(object sender, System.EventArgs args)
        {
            //Debug.Log(this.name + ": flicked");
            if (this.gesture.ScreenFlickVector.x < 0)
            {
                this.jacketManager.Flicked(true);
            } else
            {
                this.jacketManager.Flicked(false);
            }
        }
    }
}