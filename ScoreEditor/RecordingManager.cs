using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class RecordingManager : MonoBehaviour
    {

        public SoundManager soundManager = null;
        public NotesManager notesManager = null;
        public NotesSelectManager notesSelectManager = null;

        private bool isRecording = false;
        private NoteData releaseNote;

        public void StartRecording()
        {
            isRecording = true;
        }

        public void StopRecording()
        {
            isRecording = false;
        }

        public void OnTaoped(int lane)
        {
            switch (this.notesSelectManager.GetMode())
            {
                case (byte)NotesSelectManager.MODE.SINGLE:
                    NoteData note = new NoteData(notesManager.GetIndex(), (byte)lane, (byte)NoteData.NOTE.SINGLE, soundManager.GetTime(), -1, -1);
                    notesManager.AddNote(note);
                    break;
                case (byte)NotesSelectManager.MODE.SLIDE:
                    break;
                case (byte)NotesSelectManager.MODE.FLICK:
                    note = new NoteData(notesManager.GetIndex(), (byte)lane, (byte)NoteData.NOTE.FLICK, soundManager.GetTime(), -1, -1);
                    notesManager.AddNote(note);
                    break;

            }
        }

        public void OnPressed(byte lane)
        {
            if (this.notesSelectManager.GetMode() != (byte)NotesSelectManager.MODE.SLIDE) return;

            NoteData pressNote = new NoteData(notesManager.GetIndex(), lane, (byte)NoteData.NOTE.SLIDE_PRESS, soundManager.GetTime(), notesManager.GetIndex() + 1, -1);
            releaseNote = new NoteData(notesManager.GetIndex() + 1, lane, (byte)NoteData.NOTE.SLIDE_RELEASE, soundManager.GetTime() + 0.1f, -1, notesManager.GetIndex());
            notesManager.AddNote(pressNote);
            notesManager.AddNote(releaseNote);
        }

        public void OnReleased(byte lane)
        {
            //Debug.Log("RecordingManager: OnReleased(lane=" + lane.ToString() + ")");

            if (this.notesSelectManager.GetMode() != (byte)NotesSelectManager.MODE.SLIDE) return;

            releaseNote.time = soundManager.GetTime();
            releaseNote.lane = lane;
        }

        public bool IsRecording()
        {
            return isRecording;
        }
    }
}