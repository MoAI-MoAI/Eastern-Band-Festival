using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class RecordSwitcher : MonoBehaviour
    {

        public GameObject standby_prefab = null;
        public GameObject recording_prefab = null;
        public GameObject button = null;

        private GameObject showing_prefab;

        // Use this for initialization
        void Start()
        {
            this.showing_prefab = (GameObject)Instantiate(this.standby_prefab);
            this.showing_prefab.transform.SetParent(this.transform, false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SwitchToStandby()
        {
            GameObject.Destroy(this.showing_prefab);
            this.showing_prefab = (GameObject)Instantiate(this.standby_prefab);
            this.showing_prefab.transform.SetParent(this.transform, false);
            button.GetComponentInChildren<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
        }

        public void SwitchToRecording()
        {
            GameObject.Destroy(this.showing_prefab);
            this.showing_prefab = (GameObject)Instantiate(this.recording_prefab);
            this.showing_prefab.transform.SetParent(this.transform, false);
            button.GetComponentInChildren<Image>().color = new Color(255.0f / 255.0f, 20.0f / 255.0f, 20.0f / 255.0f, 255.0f / 255.0f);
        }
    }
}