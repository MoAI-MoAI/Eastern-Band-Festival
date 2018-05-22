using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class SyncLine : MonoBehaviour
    {

        public int index = 0;

        private NotesManager manager = null;
        private SoundManager soundManager = null;
        private NoteData note1 = null;
        private NoteData note2 = null;

        // Update is called once per frame
        void Update()
        {
            if (!this.note1.isEnable || !this.note2.isEnable || Mathf.Abs(note1.time - note2.time) * 1000 >= 1 || CalcTime(true) >= this.manager.speed + this.manager.afterTime || CalcTime(false) >= this.manager.speed + this.manager.afterTime)
            {
                Destroy(this.gameObject);
                return;
            }

            float x = this.manager.lane_width * ((float)(this.note1.lane + this.note2.lane) / 2f - 3f);
            float y = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);
            float scaleX = (this.manager.lane_width / 10) * Mathf.Abs(this.note1.lane - this.note2.lane);

            this.transform.position = new Vector3(x, y, this.transform.position.z);
            this.transform.localScale = new Vector3(scaleX, this.transform.localScale.y, this.transform.localScale.z);
        }

        public void Launch(int index, NotesManager notesManager, SoundManager soundManager, NoteData note1, NoteData note2)
        {
            this.index = index;
            this.manager = notesManager;
            this.soundManager = soundManager;
            this.note1 = note1;
            this.note2 = note2;

            float x = this.manager.lane_width * ((float)(this.note1.lane + this.note2.lane) / 2f - 3f);
            float y = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);
            float scaleX = (this.manager.lane_width / 10) * Mathf.Abs(this.note1.lane - this.note2.lane);

            this.transform.position = new Vector3(x, y, this.transform.position.z);
            this.transform.localScale = new Vector3(scaleX, this.transform.localScale.y, this.transform.localScale.z);
        }

        public NoteData GetNoteData(bool isNote1 = true)
        {
            return (isNote1 ? this.note1 : this.note2);
        }

        private float CalcTime(bool isNote1 = true)
        {
            return soundManager.GetTime() - ((isNote1 ? this.note1.time : this.note2.time) - this.manager.speed);
        }
    }
}