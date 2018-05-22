using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class NotesSelectManager : MonoBehaviour
    {

        public Button buttonSingle = null;
        public Button buttonSlide = null;
        public Button buttonSlide_Via = null;
        public Button buttonFlick = null;
        public Button buttonDelete = null;
        public Button buttonMove = null;

        public enum MODE : byte
        {
            SINGLE,
            SLIDE,
            FLICK,
            DELETE,
            MOVE,
            SLIDE_VIA
        }

        private byte selecting = (byte)MODE.SINGLE;

        // Use this for initialization
        void Start()
        {
            this.buttonSingle.interactable = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTapped(int select)
        {
            if (this.selecting == select) return;

            switch (this.selecting)
            {
                case (byte)MODE.SINGLE:
                    this.buttonSingle.interactable = true;
                    break;
                case (byte)MODE.SLIDE:
                    this.buttonSlide.interactable = true;
                    break;
                case (byte)MODE.FLICK:
                    this.buttonFlick.interactable = true;
                    break;
                case (byte)MODE.DELETE:
                    this.buttonDelete.interactable = true;
                    break;
                case (byte)MODE.MOVE:
                    this.buttonMove.interactable = true;
                    break;
                case (byte)MODE.SLIDE_VIA:
                    this.buttonSlide_Via.interactable = true;
                    break;
            }

            switch (select)
            {
                case (byte)MODE.SINGLE:
                    this.buttonSingle.interactable = false;
                    break;
                case (byte)MODE.SLIDE:
                    this.buttonSlide.interactable = false;
                    break;
                case (byte)MODE.FLICK:
                    this.buttonFlick.interactable = false;
                    break;
                case (byte)MODE.DELETE:
                    this.buttonDelete.interactable = false;
                    break;
                case (byte)MODE.MOVE:
                    this.buttonMove.interactable = false;
                    break;
                case (byte)MODE.SLIDE_VIA:
                    this.buttonSlide_Via.interactable = false;
                    break;
            }

            this.selecting = (byte)select;
        }

        public byte GetMode()
        {
            return selecting;
        }
    }
}