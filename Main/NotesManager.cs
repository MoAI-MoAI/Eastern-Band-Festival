using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using BandParty;
using BandParty.Main;

namespace BandParty.Main {
    public class NotesManager : MonoBehaviour
    {
        public SoundManager soundManager = null;
        public Camera mainCamera = null;
        public GameObject notesParent = null;
        public GameObject slideLinesParent = null;
        public GameObject syncLinesParent = null;
        public GameObject singlenotesPrefab = null;
        public GameObject slidenotesPressPrefab = null;
        public GameObject slidenotesViaPrefab = null;
        public GameObject slidenotesReleasePrefab = null;
        public GameObject slidelinesPrefab = null;
        public GameObject flicknotesPrefab = null;
        public GameObject syncLinesPrefab = null;
        public float lane_width = 2.5f;
        public float from = 80.0f;
        public float to = -3.79f;
        public float speed = 6.0f;
        public float afterTime = 0.5f;
        public float from_angle = -25.0f;
        public float to_angle = 0.0f;

        public float speed_slow = 7.0f;
        public float speed_normal = 5.0f;
        public float speed_fast = 3.0f;

        public List<NoteData> notes;
        public List<NoteData> backUp;
        
        // Use this for initialization
        void Start()
        {
            this.notes = new List<NoteData>();
            this.backUp = new List<NoteData>();

            //SongDataSender.speed = Common.SPEED.FAST;

            switch (SongDataSender.speed)
            {
                case Common.SPEED.SLOW:
                    this.speed = this.speed_slow;
                    break;
                case Common.SPEED.NORMAL:
                    this.speed = this.speed_normal;
                    break;
                case Common.SPEED.FAST:
                    this.speed = this.speed_fast;
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            List<int> launchedNotes = new List<int>();
            List<SyncLine> launchedSyncLines = new List<SyncLine>();

            Transform childTransform = this.notesParent.transform;
            foreach (Transform child in childTransform.transform)
            {
                NoteData data = child.gameObject.GetComponent<Notes>().GetNoteData();
                launchedNotes.Add(data.index);
            }

            childTransform = this.syncLinesParent.transform;
            foreach (Transform child in childTransform.transform)
            {
                SyncLine line = child.gameObject.GetComponent<SyncLine>();
                launchedSyncLines.Add(line);
            }

            for (int i = 0; i < this.notes.Count; i++)
            {
                NoteData note = this.notes[i];

                if (this.soundManager.time >= note.time - this.speed && this.soundManager.time <= note.time + this.afterTime)
                {
                    bool doDraw = true;
                    foreach (SyncLine launchedLine in launchedSyncLines)
                    {
                        if (launchedLine.GetNoteData(true).index == note.index || launchedLine.GetNoteData(false).index == note.index)
                        {
                            doDraw = false;
                            break;
                        }
                    }

                    if (doDraw)
                    {
                        foreach (NoteData noteData in this.notes)
                        {
                            if (noteData.index == note.index) continue;
                            if (Mathf.Abs(noteData.time - note.time) * 1000 < 1)
                            {
                                foreach (SyncLine launchedLine in launchedSyncLines)
                                {
                                    if (launchedLine.GetNoteData(true).index == noteData.index || launchedLine.GetNoteData(false).index == noteData.index)
                                    {
                                        goto ExitProcess;
                                    }
                                }

                                SyncLine syncLine = Instantiate(syncLinesPrefab).gameObject.GetComponent<SyncLine>();
                                syncLine.transform.parent = syncLinesParent.transform;
                                syncLine.Launch(note.index, this, this.soundManager, note, noteData);

                                launchedSyncLines.Add(syncLine);
                            }
                        }
                        ExitProcess:;
                    }

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

            childTransform = this.notesParent.transform;
            foreach (Transform child in childTransform.transform)
            {
                if (launchedNotes.Count == 0) break;
                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                foreach (int index in launchedNotes)
                {
                    if (childData.index == index)
                    {
                        Destroy(child.gameObject);
                        launchedNotes.Remove(index);
                        break;
                    }
                }
            }

            childTransform = this.notesParent.transform;
            foreach (Transform child in childTransform.transform)
            {
                NoteData data = child.gameObject.GetComponent<Notes>().GetNoteData();
                launchedNotes.Add(data.index);
            }

            childTransform = this.slideLinesParent.transform;
            foreach (Transform child in childTransform.transform)
            {
                SlideLine line = child.gameObject.GetComponent<SlideLine>();
                foreach (int index in launchedNotes)
                {
                    if (line.GetNoteData(true).index == index || line.GetNoteData(false).index == index)
                    {
                        goto ThroughProcess;
                    }
                }

                Destroy(child.gameObject);

                ThroughProcess:;
            }

            childTransform = this.syncLinesParent.transform;
            foreach (Transform child in childTransform.transform)
            {
                SyncLine line = child.gameObject.GetComponent<SyncLine>();
                foreach (int index in launchedNotes)
                {
                    if (line.GetNoteData(true).index == index || line.GetNoteData(false).index == index)
                    {
                        goto ThroughProcess;
                    }
                }

                Destroy(child.gameObject);

                ThroughProcess:;
            }
        }

        void LaunchNotes(NoteData note)
        {
            List<int> launchedSlideLines = new List<int>();

            Transform childTransform = this.slideLinesParent.transform;
            foreach (Transform child in childTransform.transform)
            {
                SlideLine line = child.gameObject.GetComponent<SlideLine>();
                launchedSlideLines.Add(line.index);
            }

            bool doDraw;
            switch (note.type)
            {
                case Common.NOTE.SINGLE:
                    Notes fromNote = Instantiate(singlenotesPrefab).gameObject.GetComponent<Notes>();
                    fromNote.transform.parent = notesParent.transform;
                    fromNote.Launch(this, this.soundManager, note);
                    break;
                case Common.NOTE.SLIDE_PRESS:
                    fromNote = Instantiate(slidenotesPressPrefab).gameObject.GetComponent<Notes>();
                    fromNote.transform.parent = notesParent.transform;
                    fromNote.Launch(this, this.soundManager, note);

                    doDraw = true;
                    for (int k = 0; k < launchedSlideLines.Count; k++)
                    {
                        if (note.index == launchedSlideLines[k])
                        {
                            doDraw = false;
                        }
                    }

                    if (doDraw)
                    {
                        for (int j = 0; j < this.notes.Count; j++)
                        {
                            if (note.chainBehind == notes[j].index)
                            {
                                SlideLine slideLine = Instantiate(slidelinesPrefab).gameObject.GetComponent<SlideLine>();
                                slideLine.transform.parent = slideLinesParent.transform;
                                slideLine.Launch(note.index, this, this.soundManager, note, notes[j]);
                                note.slideLine = slideLine;

                                launchedSlideLines.Add(slideLine.index);
                                break;
                            }
                        }
                    }

                    break;
                case Common.NOTE.SLIDE_VIA:
                    fromNote = Instantiate(slidenotesViaPrefab).gameObject.GetComponent<Notes>();
                    fromNote.transform.parent = notesParent.transform;
                    fromNote.Launch(this, this.soundManager, note);

                    doDraw = true;
                    for (int k = 0; k < launchedSlideLines.Count; k++)
                    {
                        if (note.index == launchedSlideLines[k]) doDraw = false;
                    }

                    if (doDraw)
                    {
                        for (int j = 0; j < this.notes.Count; j++)
                        {
                            if (note.chainBehind == notes[j].index)
                            {
                                SlideLine slideLine = Instantiate(slidelinesPrefab).gameObject.GetComponent<SlideLine>();
                                slideLine.transform.parent = slideLinesParent.transform;
                                slideLine.Launch(note.index, this, this.soundManager, note, notes[j]);
                                note.slideLine = slideLine;

                                launchedSlideLines.Add(slideLine.index);
                                break;
                            }
                        }
                    }

                    doDraw = true;
                    for (int k = 0; k < launchedSlideLines.Count; k++)
                    {
                        if (note.chainForward == launchedSlideLines[k]) doDraw = false;
                    }

                    if (doDraw)
                    {
                        for (int j = 0; j < this.notes.Count; j++)
                        {
                            if (note.chainForward == notes[j].index)
                            {
                                SlideLine slideLine = Instantiate(slidelinesPrefab).gameObject.GetComponent<SlideLine>();
                                slideLine.transform.parent = slideLinesParent.transform;
                                slideLine.Launch(notes[j].index, this, this.soundManager, notes[j], note);

                                launchedSlideLines.Add(slideLine.index);
                                break;
                            }
                        }
                    }

                    break;
                case Common.NOTE.SLIDE_RELEASE:
                    fromNote = Instantiate(slidenotesReleasePrefab).gameObject.GetComponent<Notes>();
                    fromNote.transform.parent = notesParent.transform;
                    fromNote.Launch(this, this.soundManager, note);

                    doDraw = true;
                    for (int k = 0; k < launchedSlideLines.Count; k++)
                    {
                        if (note.chainForward == launchedSlideLines[k]) doDraw = false;
                    }

                    if (doDraw)
                    {
                        for (int j = 0; j < this.notes.Count; j++)
                        {
                            if (note.chainForward == notes[j].index)
                            {
                                SlideLine slideLine = Instantiate(slidelinesPrefab).gameObject.GetComponent<SlideLine>();
                                slideLine.transform.parent = slideLinesParent.transform;
                                slideLine.Launch(notes[j].index, this, this.soundManager, notes[j], note);

                                launchedSlideLines.Add(slideLine.index);
                                break;
                            }
                        }
                    }

                    break;
                case Common.NOTE.FLICK:
                    fromNote = Instantiate(flicknotesPrefab).gameObject.GetComponent<Notes>();
                    fromNote.transform.parent = notesParent.transform;
                    GameObject grandchild = fromNote.transform.Find("Wrap").transform.Find("Arrow").gameObject;
                    grandchild.GetComponent<LookAtCamera>().targetCamera = mainCamera;
                    fromNote.Launch(this, this.soundManager, note);

                    if (note.chainForward != -1)
                    {
                        doDraw = true;
                        for (int k = 0; k < launchedSlideLines.Count; k++)
                        {
                            if (note.chainForward == launchedSlideLines[k]) doDraw = false;
                        }

                        if (doDraw)
                        {
                            for (int j = 0; j < this.notes.Count; j++)
                            {
                                if (note.chainForward == notes[j].index)
                                {
                                    SlideLine slideLine = Instantiate(slidelinesPrefab).gameObject.GetComponent<SlideLine>();
                                    slideLine.transform.parent = slideLinesParent.transform;
                                    slideLine.Launch(notes[j].index, this, this.soundManager, notes[j], note);

                                    launchedSlideLines.Add(slideLine.index);
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        public void DestroyNote(NoteData data)
        {
            try
            {
                foreach (NoteData note in this.notes)
                {
                    if (note.index == data.index)//
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
                        note.isActive = false;
                        notes.Remove(note);
                        break;
                    }
                }
            } catch (System.Exception e)
            {
                Debug.Log(this.name + ": " + e.ToString());
            }
        }

        public void RestoreNotes()
        {
            notes = new List<NoteData>(backUp);
        }
    }
}
