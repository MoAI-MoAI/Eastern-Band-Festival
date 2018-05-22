using UnityEngine;
using System.Collections;

using BandParty.TimingGetter;

namespace BandParty.TimingGetter
{
    public class Notes : MonoBehaviour
    {
        private NotesManager manager = null;
        private NoteData data = null;

        [SerializeField]
        private float y = 0.0f;
        private float angle = 0.0f;

        // Update is called once per frame
        void Update()
        {
            if (CalcTime() >= this.manager.speed + this.manager.afterTime)
            {
                Debug.Log(this.name + ": Destroy index=" + this.data.index.ToString());
                this.manager.DestroyNote(this.data);
                return;
            }

            this.y = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);
            this.angle = this.manager.from_angle + (this.manager.to_angle - this.manager.from_angle) * (this.CalcTime() / this.manager.speed);

            this.transform.position = new Vector3(0, this.y, this.transform.position.z);
            this.transform.eulerAngles = new Vector3(this.angle, 0.0f, 0.0f);
        }

        public void Launch(NotesManager notesManager, NoteData noteData)
        {
            this.manager = notesManager;
            this.data = noteData;
            this.y = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);
            this.angle = this.manager.from_angle;

            this.transform.position = new Vector3(0, this.y, this.transform.position.z);
            this.transform.eulerAngles = new Vector3(this.angle, 0.0f, 0.0f);
        }

        public NoteData GetNoteData()
        {
            return this.data;
        }

        private float CalcTime()
        {
            Debug.Log(this.name + ": CalcTime() index=" + this.data.index.ToString()
                + " smTime=" + this.manager.soundManager.time
                + " dataTime=" + this.data.time
                + " speed=" + this.manager.speed
                + " return=" + (this.manager.soundManager.time - (this.data.time - this.manager.speed)).ToString());
            return this.manager.soundManager.time - (this.data.time - this.manager.speed);
        }
    }

}

