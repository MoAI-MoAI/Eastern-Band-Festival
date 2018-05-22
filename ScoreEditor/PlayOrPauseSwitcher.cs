using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class PlayOrPauseSwitcher : MonoBehaviour
    {

        public GameObject arraw_prefab = null;
        public GameObject pause_prefab = null;

        private GameObject showing_prefab;

        // Use this for initialization
        void Start()
        {
            this.showing_prefab = (GameObject)Instantiate(this.arraw_prefab);
            this.showing_prefab.transform.SetParent(this.transform, false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SwitchToStandby()
        {
            GameObject.Destroy(this.showing_prefab);
            this.showing_prefab = (GameObject)Instantiate(this.arraw_prefab);
            this.showing_prefab.transform.SetParent(this.transform, false);
        }

        public void SwitchToPlaying()
        {
            GameObject.Destroy(this.showing_prefab);
            this.showing_prefab = (GameObject)Instantiate(this.pause_prefab);
            this.showing_prefab.transform.SetParent(this.transform, false);
        }
    }
}