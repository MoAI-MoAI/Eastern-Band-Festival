using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Hit;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class EditManager : MonoBehaviour
    {

        public SoundManager soundManager = null;
        public NotesManager notesManager = null;
        public NotesSelectManager notesSelectManager = null;
        public GameObject notesParent = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTapped(Vector3 point)
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

            switch (this.GetMode())
            {
                case (byte)NotesSelectManager.MODE.SINGLE:
                    float time = soundManager.GetTime() + (notesManager.to - point.y) / ((notesManager.to - notesManager.from) / notesManager.speed);
                    notesManager.AddNote(new NoteData(notesManager.GetIndex(), lane, (byte)NoteData.NOTE.SINGLE, time, -1, -1));
                    break;
                case (byte)NotesSelectManager.MODE.SLIDE:
                    time = soundManager.GetTime() + (notesManager.to - point.y) / ((notesManager.to - notesManager.from) / notesManager.speed);
                    NoteData pressNote = new NoteData(notesManager.GetIndex(), lane, (byte)NoteData.NOTE.SLIDE_PRESS, time, notesManager.GetIndex() + 1, -1);
                    NoteData releaseNote = new NoteData(notesManager.GetIndex() + 1, lane, (byte)NoteData.NOTE.SLIDE_RELEASE, time + 0.5f, -1, notesManager.GetIndex());
                    notesManager.AddNote(pressNote);
                    notesManager.AddNote(releaseNote);
                    break;
                case (byte)NotesSelectManager.MODE.SLIDE_VIA:
                    break;
                case (byte)NotesSelectManager.MODE.FLICK:
                    time = soundManager.GetTime() + (notesManager.to - point.y) / ((notesManager.to - notesManager.from) / notesManager.speed);
                    notesManager.AddNote(new NoteData(notesManager.GetIndex(), lane, (byte)NoteData.NOTE.FLICK, time, -1, -1));
                    break;
            }
        }

        public void OnSlideLineTapped(Vector3 point, NoteData fromNote, NoteData toNote, SlideLine slideLine)
        {
            if (this.GetMode() != (byte)NotesSelectManager.MODE.SLIDE_VIA) return;

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

            float time = soundManager.GetTime() + (notesManager.to - point.y) / ((notesManager.to - notesManager.from) / notesManager.speed);
            NoteData note = new NoteData(notesManager.GetIndex(), lane, (byte)NoteData.NOTE.SLIDE_VIA, time, toNote.index, fromNote.index);
            notesManager.AddNote(note);
            fromNote.chainBehind = note.index;
            toNote.chainForward = note.index;
            slideLine.SetToNote(note);
        }

        public byte GetMode()
        {
            return notesSelectManager.GetMode();
        }
    }
}
