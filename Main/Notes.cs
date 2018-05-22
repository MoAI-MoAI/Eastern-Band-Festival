using UnityEngine;
using System.Collections;

using BandParty;
using BandParty.Main;

namespace BandParty.Main
{
    public class Notes : MonoBehaviour
    {
        private NotesManager manager = null;
        private SoundManager soundManager = null;
        private NoteData data = null;

        [SerializeField]
        private float y = 0.0f;
        private float angle = 0.0f;

        // Update is called once per frame
        void Update()
        {
            if (CalcTime() >= this.manager.speed + this.manager.afterTime)
            {
                Destroy(this.gameObject);
                return;
            }

            this.y = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);
            this.angle = this.manager.from_angle + (this.manager.to_angle - this.manager.from_angle) * (this.CalcTime() / this.manager.speed);

            this.transform.localPosition = new Vector3(this.manager.lane_width * (this.data.lane - 3), this.y, this.transform.position.z);
            this.transform.localEulerAngles = new Vector3(this.angle, 0.0f, 0.0f);
        }

        public void Launch(NotesManager notesManager, SoundManager soundManager, NoteData noteData)
        {
            this.manager = notesManager;
            this.soundManager = soundManager;
            this.data = noteData;
            this.y = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);
            this.angle = this.manager.from_angle;

            this.transform.localPosition = new Vector3(this.manager.lane_width * (this.data.lane - 3), this.y, this.transform.position.z);
            this.transform.localEulerAngles = new Vector3(this.angle, 0.0f, 0.0f);
        }

        public NoteData GetNoteData()
        {
            return this.data;
        }

        private float CalcTime()
        {
            return soundManager.time - (this.data.time - this.manager.speed);
        }
    }

}

