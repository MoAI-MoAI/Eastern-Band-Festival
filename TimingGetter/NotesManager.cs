using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BandParty.TimingGetter {
    public class NotesManager : MonoBehaviour
    {
        public SoundManager soundManager = null;
        public Camera mainCamera = null;
        public GameObject notesParent = null;
        public GameObject singlenotesPrefab = null;
        public float from = 80.0f;
        public float to = -3.79f;
        public float speed = 4.0f;
        public float afterTime = 0.5f;
        public float from_angle = -25.0f;
        public float to_angle = 0.0f;
        
        public List<NoteData> notes;

        private int count = 0;
        
        // Use this for initialization
        void Start()
        {
            this.count = 0;
            
            this.notes = new List<NoteData>();
        }

        // Update is called once per frame
        void Update()
        {
            List<int> launchedNotes = new List<int>();
            
            Transform childTransform = this.notesParent.transform;
            foreach (Transform child in childTransform.transform)
            {
                NoteData data = child.gameObject.GetComponent<Notes>().GetNoteData();
                launchedNotes.Add(data.index);
            }

            if (launchedNotes.Count == 0)
            {
                float nextTime = Mathf.Ceil(this.soundManager.time / (60f / this.soundManager.BPM * 4)) * (60f / this.soundManager.BPM * 4);
                this.notes.Add(new NoteData(this.count, nextTime));
                this.count++;
            }

            for (int i = 0; i < this.notes.Count; i++)
            {
                NoteData note = this.notes[i];

                if (this.soundManager.time >= note.time - this.speed && this.soundManager.time <= note.time + this.afterTime)
                {
                    for (int j = 0; j < launchedNotes.Count; j++)
                    {
                        if (note.index == launchedNotes[j])
                        {
                            launchedNotes.Remove(launchedNotes[j]);
                            goto ThroughProcess;
                        }
                    }

                    LaunchNotes(note);
                }

                ThroughProcess:;
            }

            
        }

        void LaunchNotes(NoteData note)
        {
            Notes fromNote = Instantiate(singlenotesPrefab).gameObject.GetComponent<Notes>();
            fromNote.transform.parent = notesParent.transform;
            fromNote.Launch(this, note);
        }

        public void DestroyNote(NoteData data)
        {
            foreach (NoteData note in this.notes)
            {
                if (note.index == data.index)
                {
                    Transform childTransform = this.notesParent.transform;
                    foreach (Transform child in childTransform.transform)
                    {
                        NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                        if (childData.index == note.index)
                        {
                            Destroy(child.gameObject);
                            break;
                        }
                    }
                    notes.Remove(note);
                    break;
                }
            }
        }
    }
}
