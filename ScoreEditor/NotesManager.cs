using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.Editor
{
    public class NotesManager : MonoBehaviour
    {

        public SoundManager soundManager = null;
        public EditManager editManager = null;
        public DifficultyManager difficulty;
        public SaveDataIO saveDataIO = null;
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
        public Toggle quantize = null;
        public float lane_width = 2.5f;
        public float from = 80.0f;
        public float to = -3.79f;
        public float speed = 6.0f;
        public float afterTime = 0.5f;
        public float from_angle = -25.0f;
        public float to_angle = 0.0f;

        public List<NoteData> notes;
        private int indexCount = 0;

        // Use this for initialization
        void Start()
        {
            notes = new List<NoteData>();
            saveDataIO.Load(difficulty.GetValue());
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

                if (this.soundManager.GetTime() >= note.time - this.speed && this.soundManager.GetTime() <= note.time + this.afterTime)
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
                case (byte)NoteData.NOTE.SINGLE:
                    Notes fromNote = Instantiate(singlenotesPrefab).gameObject.GetComponent<Notes>();
                    fromNote.transform.parent = notesParent.transform;
                    fromNote.Launch(this, this.soundManager, this.editManager, note);
                    break;
                case (byte)NoteData.NOTE.SLIDE_PRESS:
                    fromNote = Instantiate(slidenotesPressPrefab).gameObject.GetComponent<Notes>();
                    fromNote.transform.parent = notesParent.transform;
                    fromNote.Launch(this, this.soundManager, this.editManager, note);

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
                case (byte)NoteData.NOTE.SLIDE_VIA:
                    fromNote = Instantiate(slidenotesViaPrefab).gameObject.GetComponent<Notes>();
                    fromNote.transform.parent = notesParent.transform;
                    fromNote.Launch(this, this.soundManager, this.editManager, note);

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
                case (byte)NoteData.NOTE.SLIDE_RELEASE:
                    fromNote = Instantiate(slidenotesReleasePrefab).gameObject.GetComponent<Notes>();
                    fromNote.transform.parent = notesParent.transform;
                    fromNote.Launch(this, this.soundManager, this.editManager, note);

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
                case (byte)NoteData.NOTE.FLICK:
                    fromNote = Instantiate(flicknotesPrefab).gameObject.GetComponent<Notes>();
                    fromNote.transform.parent = notesParent.transform;
                    GameObject grandchild = fromNote.transform.Find("Wrap").transform.Find("Arrow").gameObject;
                    grandchild.GetComponent<LookAtCamera>().targetCamera = mainCamera;
                    fromNote.Launch(this, this.soundManager, this.editManager, note);

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

        public void AddNote(NoteData noteData)
        {
            if (noteData.time < 0f) return;

            if (quantize.isOn)
            {
                float time = noteData.time;
                float SPQ = (60f / soundManager.BPM) / 4; // second per quater
                time = (float)Math.Round(time / SPQ, MidpointRounding.AwayFromZero) * SPQ;

                noteData.time = time;
            }

            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].time == noteData.time && notes[i].lane == noteData.lane) return;
            }

            notes.Add(noteData);
            indexCount++;
        }

        public void DeleteNote(NoteData noteData)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                if (this.notes[i].index == noteData.index)
                {
                    NoteData deletedNote = this.notes[i];
                    this.notes[i].isEnable = false;
                    this.notes.Remove(this.notes[i]);

                    switch (noteData.type)
                    {
                        case (byte)NoteData.NOTE.SLIDE_PRESS:
                            for (int j = 0; j < notes.Count; j++)
                            {
                                if (deletedNote.chainBehind == this.notes[j].index)
                                {
                                    this.DeleteRelatedNote(this.notes[j]);
                                    break;
                                }
                            }

                            Transform childTransform = this.notesParent.transform;
                            foreach (Transform child in childTransform.transform)
                            {
                                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                                if (childData.index == deletedNote.index)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            break;
                        case (byte)NoteData.NOTE.SLIDE_VIA:
                            NoteData behindNote = null, forwardNote = null;

                            for (int j = 0; j < notes.Count; j++)
                            {
                                if (deletedNote.chainBehind == this.notes[j].index) behindNote = this.notes[j];
                                else if (deletedNote.chainForward == this.notes[j].index) forwardNote = this.notes[j];
                            }
                            behindNote.chainForward = forwardNote.index;
                            forwardNote.chainBehind = behindNote.index;
                            forwardNote.slideLine.SetToNote(behindNote);

                            childTransform = this.notesParent.transform;
                            foreach (Transform child in childTransform.transform)
                            {
                                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                                if (childData.index == deletedNote.index)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            break;
                        case (byte)NoteData.NOTE.SLIDE_RELEASE:
                            for (int j = 0; j < notes.Count; j++)
                            {
                                if (deletedNote.chainForward == this.notes[j].index)
                                {
                                    this.DeleteRelatedNote(this.notes[j]);
                                    break;
                                }
                            }

                            childTransform = this.notesParent.transform;
                            foreach (Transform child in childTransform.transform)
                            {
                                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                                if (childData.index == deletedNote.index)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            break;
                        default:
                            childTransform = this.notesParent.transform;
                            foreach (Transform child in childTransform.transform)
                            {
                                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                                if (childData.index == deletedNote.index)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            break;
                    }
                    break;
                }
            }
        }

        private void DeleteRelatedNote(NoteData noteData)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                if (this.notes[i].index == noteData.index)
                {
                    NoteData deletedNote = this.notes[i];
                    this.notes[i].isEnable = false;
                    this.notes.Remove(this.notes[i]);

                    switch (noteData.type)
                    {
                        case (byte)NoteData.NOTE.SLIDE_PRESS:
                            for (int j = 0; j < notes.Count; j++)
                            {
                                if (deletedNote.chainBehind == this.notes[j].index)
                                {
                                    this.DeleteRelatedNote(this.notes[j]);
                                    break;
                                }
                            }

                            Transform childTransform = this.notesParent.transform;
                            foreach (Transform child in childTransform.transform)
                            {
                                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                                if (childData.index == deletedNote.index)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            break;
                        case (byte)NoteData.NOTE.SLIDE_VIA:
                            for (int j = 0; j < notes.Count; j++)
                            {
                                if (deletedNote.chainBehind == this.notes[j].index || deletedNote.chainForward == this.notes[j].index)
                                {
                                    this.DeleteRelatedNote(this.notes[j]);
                                    break;
                                }
                            }

                            childTransform = this.notesParent.transform;
                            foreach (Transform child in childTransform.transform)
                            {
                                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                                if (childData.index == deletedNote.index)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            break;
                        case (byte)NoteData.NOTE.SLIDE_RELEASE:
                            for (int j = 0; j < notes.Count; j++)
                            {
                                if (deletedNote.chainForward == this.notes[j].index)
                                {
                                    this.DeleteRelatedNote(this.notes[j]);
                                    break;
                                }
                            }

                            childTransform = this.notesParent.transform;
                            foreach (Transform child in childTransform.transform)
                            {
                                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                                if (childData.index == deletedNote.index)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            break;
                        case (byte)NoteData.NOTE.FLICK:
                            if (noteData.chainForward != -1)
                            {
                                for (int j = 0; j < notes.Count; j++)
                                {
                                    if (deletedNote.chainForward == this.notes[j].index)
                                    {
                                        this.DeleteRelatedNote(this.notes[j]);
                                        break;
                                    }
                                }
                            }

                            childTransform = this.notesParent.transform;
                            foreach (Transform child in childTransform.transform)
                            {
                                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                                if (childData.index == deletedNote.index)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            break;
                        default:
                            childTransform = this.notesParent.transform;
                            foreach (Transform child in childTransform.transform)
                            {
                                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                                if (childData.index == deletedNote.index)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            break;
                    }
                    break;
                }
            }
        }

        public void UpdateNotes(NoteData noteData)
        {
            Transform childTransform = this.notesParent.transform;
            foreach (Transform child in childTransform.transform)
            {
                NoteData childData = child.gameObject.GetComponent<Notes>().GetNoteData();
                if (childData.index == noteData.index)
                {
                    Destroy(child.gameObject);
                    this.Update();
                    break;
                }
            }
        }

        public void Quantize(NoteData noteData)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].index == noteData.index)
                {
                    if (quantize.isOn)
                    {
                        float time = noteData.time;
                        float SPQ = (60f / soundManager.BPM) / 4; // second per quater
                        time = (float)Math.Round(time / SPQ, MidpointRounding.AwayFromZero) * SPQ;

                        noteData.time = time;
                    }
                    break;
                }
            }
        }

        public int GetIndex()
        {
            return this.indexCount;
        }

        public void SetIndex(int indexCount)
        {
            this.indexCount = indexCount;
        }

        public List<NoteData> GetNotesList()
        {
            return notes;
        }

        public void SetNotesList(List<NoteData> notes)
        {
            this.notes = notes;

            Transform childTransform = this.slideLinesParent.transform;
            foreach (Transform child in childTransform)
            {
                Destroy(child.gameObject);
            }

            childTransform = this.syncLinesParent.transform;
            foreach (Transform child in childTransform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}