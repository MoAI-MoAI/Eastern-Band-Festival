using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

using BandParty.TimingGetter;

namespace BandParty.TimingGetter
{
    public class TouchPoint : MonoBehaviour
    {
        public NotesManager notesManager;
        public RegulationsManager regulations;

        private PressGesture gesture;

        private void OnEnable()
        {
            this.gesture = this.GetComponent<PressGesture>();
            this.gesture.Pressed += this.onPressedHandler;
        }

        private void OnDisable()
        {
            this.gesture.Pressed -= this.onPressedHandler;
        }

        private void onPressedHandler(object sender, System.EventArgs args)
        {
            this.regulations.regulations.Add(this.notesManager.notes[0].time - this.notesManager.soundManager.time);
            this.notesManager.DestroyNote(this.notesManager.notes[0]);
        }
    }
}
