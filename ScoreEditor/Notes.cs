using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using TouchScript.Behaviors;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class Notes : MonoBehaviour
    {

        private NotesManager manager = null;
        private SoundManager soundManager = null;
        private EditManager editManager = null;
        private NoteData data = null;

        [SerializeField]
        private float y = 0.0f;
        private float angle = 0.0f;

        private TapGesture tapGesture;
        private TransformGesture transformGesture;
        private Transformer transformer;

        private bool isFloating = false;

        private void OnEnable()
        {
            this.tapGesture = GetComponent<TapGesture>();
            this.transformGesture = GetComponent<TransformGesture>();
            this.transformer = GetComponent<Transformer>();

            this.tapGesture.Tapped += tappedHandler;
            this.transformGesture.TransformStarted += transformStartedHandler;
            this.transformGesture.TransformCompleted += transformCompletedHandler;
        }

        private void OnDisable()
        {
            this.tapGesture.Tapped -= tappedHandler;
            this.transformGesture.TransformStarted -= transformStartedHandler;
            this.transformGesture.TransformCompleted -= transformCompletedHandler;
        }

        // Update is called once per frame
        void Update()
        {
            if (CalcTime() >= this.manager.speed + this.manager.afterTime)
            {
                Destroy(this.gameObject);
                return;
            }

            if (this.isFloating)
            {
                //Debug.Log("Notes: isFloating (x=" + this.transform.position.x.ToString() + ")");
                if (this.transform.position.x <= this.manager.lane_width * -3)
                {
                    this.data.lane = 0;
                }
                else if (this.transform.position.x >= this.manager.lane_width * 3)
                {
                    this.data.lane = 6;
                }
                else
                {
                    //Debug.Log("Notes: calc lane");
                    this.data.lane = (byte)(Math.Round(this.transform.position.x / this.manager.lane_width, MidpointRounding.AwayFromZero) + 3);
                }
                //Debug.Log("Notes: lane=" + this.data.lane.ToString());

                float prevY = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);
                float movedY = this.transform.position.y - prevY;
                this.data.time += movedY / (Math.Abs(this.manager.to - this.manager.from) / this.manager.speed);
                //float T_F = (this.manager.to - this.manager.from);
                //float PPS = T_F / this.manager.speed;
                //Debug.Log("Notes: prevY=" + prevY.ToString() + " movedY=" + movedY.ToString() + " time=" + this.data.time.ToString() + " to-from=" + T_F.ToString() + " PPS=" + PPS.ToString());
            }

            this.y = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);
            this.angle = this.manager.from_angle + (this.manager.to_angle - this.manager.from_angle) * (this.CalcTime() / this.manager.speed);

            this.transform.position = new Vector3(this.manager.lane_width * (this.data.lane - 3), this.y, this.transform.position.z);
            this.transform.eulerAngles = new Vector3(this.angle, 0.0f, 0.0f);
        }

        public void Launch(NotesManager notesManager, SoundManager soundManager, EditManager editManager, NoteData noteData)
        {
            this.manager = notesManager;
            this.soundManager = soundManager;
            this.editManager = editManager;
            this.data = noteData;
            this.y = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);
            this.angle = this.manager.from_angle;

            this.transform.position = new Vector3(this.manager.lane_width * (this.data.lane - 3), this.y, this.transform.position.z);
            this.transform.eulerAngles = new Vector3(this.angle, 0.0f, 0.0f);
        }

        void tappedHandler(object sender, System.EventArgs e)
        {
            //Debug.Log("Notes: TAPPED");
            switch (this.editManager.GetMode())
            {
                case (byte)NotesSelectManager.MODE.SLIDE:
                    if (this.data.type == (byte)NoteData.NOTE.FLICK)
                    {
                        this.data.type = (byte)NoteData.NOTE.SLIDE_RELEASE;
                        this.manager.UpdateNotes(this.data);
                    }
                    break;
                case (byte)NotesSelectManager.MODE.FLICK:
                    if (this.data.type == (byte)NoteData.NOTE.SLIDE_RELEASE)
                    {
                        //Debug.Log("Notes.tappedHandler: caseFLICK-type=SLIDE_RELEASE");
                        this.data.type = (byte)NoteData.NOTE.FLICK;
                        this.manager.UpdateNotes(this.data);
                    }
                    break;
                case (byte)NotesSelectManager.MODE.DELETE:
                    this.manager.DeleteNote(this.data);
                    break;
            }

        }

        void transformStartedHandler(object sender, System.EventArgs e)
        {
            transformer.enabled = true;
            this.isFloating = true;
        }

        void transformCompletedHandler(object sender, System.EventArgs e)
        {
            transformer.enabled = false;
            this.manager.Quantize(this.data);
            this.isFloating = false;
        }

        public NoteData GetNoteData()
        {
            return this.data;
        }

        private float CalcTime()
        {
            return soundManager.GetTime() - (this.data.time - this.manager.speed);
        }
    }
}