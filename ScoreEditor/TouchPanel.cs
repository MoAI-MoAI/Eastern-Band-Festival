using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;

namespace BandParty.Editor
{
    public class TouchPanel : MonoBehaviour
    {

        public NotesManager notesManager = null;
        public SoundManager soundManager = null;
        public RecordingManager recordingManager = null;
        public EditManager editManager = null;
        public Camera mainCamera = null;

        private TapGesture tapGesture = null;
        private PressGesture pressGesture = null;
        private ReleaseGesture releaseGesture = null;

        private void OnEnable()
        {
            tapGesture = GetComponent<TapGesture>();
            pressGesture = GetComponent<PressGesture>();
            releaseGesture = GetComponent<ReleaseGesture>();

            tapGesture.Tapped += tappedHandler;
            pressGesture.Pressed += pressedHandler;
            releaseGesture.Released += releasedHandler;
        }

        private void OnDisable()
        {
            tapGesture.Tapped -= tappedHandler;
            pressGesture.Pressed -= pressedHandler;
            releaseGesture.Released -= releasedHandler;
        }

        private void tappedHandler(object sender, System.EventArgs e)
        {
            TapGesture gesture = (TapGesture)sender;
            HitData hit = gesture.GetScreenPositionHitData();
            Vector3 point = hit.Point + hit.Normal * .5f;

            Debug.Log(this.name + ": tappedHandler time=" + Time.time.ToString()
                + " point=" + point.ToString()
                + " gesture.activePointers=" + gesture.ActivePointers.ToString());

            if (recordingManager.IsRecording())
            {
                byte lane;
                if (point.x <= this.notesManager.lane_width * -3)
                {
                    lane = 0;
                }
                else if (point.x >= this.notesManager.lane_width * 3)
                {
                    lane = 6;
                }
                else
                {
                    lane = (byte)(Math.Round(point.x / this.notesManager.lane_width, MidpointRounding.AwayFromZero) + 3);
                }
                recordingManager.OnTaoped(lane);
            }
            else if (!soundManager.IsPlaying())
            {
                editManager.OnTapped(point);
            }
        }

        private void pressedHandler(object sender, System.EventArgs e)
        {
            //Debug.Log("TouchPanel: pressed")
            PressGesture gesture = (PressGesture)sender;
            HitData hit = gesture.GetScreenPositionHitData();
            Vector3 point = hit.Point + hit.Normal * .5f;

            if (recordingManager.IsRecording())
            {
                byte lane;
                if (point.x <= this.notesManager.lane_width * -3)
                {
                    lane = 0;
                }
                else if (point.x >= this.notesManager.lane_width * 3)
                {
                    lane = 6;
                }
                else
                {
                    lane = (byte)(Math.Round(point.x / this.notesManager.lane_width, MidpointRounding.AwayFromZero) + 3);
                }
                recordingManager.OnPressed(lane);
            }
        }

        private void releasedHandler(object sender, System.EventArgs e)
        {
            //Debug.Log("TouchPanel: released");

            ReleaseGesture gesture = (ReleaseGesture)sender;
            HitData hit = gesture.GetScreenPositionHitData();
            Vector3 point = hit.Point + hit.Normal * .5f;

            if (recordingManager.IsRecording())
            {
                byte lane;
                if (point.x <= this.notesManager.lane_width * -3)
                {
                    lane = 0;
                }
                else if (point.x >= this.notesManager.lane_width * 3)
                {
                    lane = 6;
                }
                else
                {
                    lane = (byte)(Math.Round(point.x / this.notesManager.lane_width, MidpointRounding.AwayFromZero) + 3);
                }
                recordingManager.OnReleased(lane);
            }
        }
    }
}