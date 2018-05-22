using UnityEngine;
using UnityEngine.UI;

namespace BandParty.Main {
    public class SlideLine : MonoBehaviour
    {
        public int index = 0;
        public bool isPressing = false;
        public RectTransform effectTransform = null;

        private NotesManager manager = null;
        private SoundManager soundManager = null;
        private NoteData fromNote = null;
        private NoteData toNote = null;

        [SerializeField]
        private float y = 0.0f;

        // Update is called once per frame
        void Update()
        {
            if (!this.toNote.isActive || CalcTime() - (this.toNote.time - this.fromNote.time) >= this.manager.speed + this.manager.afterTime)
            {
                if (this.effectTransform != null)
                {
                    Destroy(this.effectTransform.gameObject);
                }
                Destroy(this.gameObject);
                return;
            }

            if (isPressing)
            {
                if (this.effectTransform != null)
                {
                    this.effectTransform.anchoredPosition = new Vector2((this.fromNote.lane - 3) * Common.touchPointLaneWidth + (this.toNote.lane - this.fromNote.lane) * Common.touchPointLaneWidth * (Mathf.Min((this.soundManager.time - this.fromNote.time) / (this.toNote.time - this.fromNote.time), 1)), 0);
                }

                Mesh mesh = GetComponent<MeshFilter>().mesh;
                Vector3[] vertices = mesh.vertices;

                float PPS = (this.manager.to - this.manager.from) / this.manager.speed;

                vertices[0] = new Vector3((this.toNote.lane - this.fromNote.lane) * this.manager.lane_width - this.manager.lane_width / 2,
                    -(this.toNote.time - this.soundManager.time) * PPS, 0f);
                vertices[3] = new Vector3((this.toNote.lane - this.fromNote.lane) * this.manager.lane_width * ((this.soundManager.time - this.fromNote.time) / (this.toNote.time - this.fromNote.time)) - this.manager.lane_width / 2,
                    0f, 0f);
                vertices[1] = new Vector3((this.toNote.lane - this.fromNote.lane) * this.manager.lane_width + this.manager.lane_width / 2,
                    -(this.toNote.time - this.soundManager.time) * PPS, 0f);
                vertices[2] = new Vector3((this.toNote.lane - this.fromNote.lane) * this.manager.lane_width * ((this.soundManager.time - this.fromNote.time) / (this.toNote.time - this.fromNote.time)) + this.manager.lane_width / 2,
                    0f, 0f);

                mesh.vertices = vertices;
                mesh.RecalculateBounds();

                this.y = Common.touchPointHeight;
            } else
            {
                this.y = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);
            }

            this.transform.localPosition = new Vector3(this.manager.lane_width * (this.fromNote.lane - 3), this.y, 0.0f);
        }

        public void Launch(int index, NotesManager notesManager, SoundManager soundManager, NoteData fromNote, NoteData toNote)
        {
            this.index = index;
            this.manager = notesManager;
            this.soundManager = soundManager;
            this.fromNote = fromNote;
            this.toNote = toNote;
            this.y = this.manager.from;

            Mesh mesh = GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;

            float PPS = (this.manager.to - this.manager.from) / this.manager.speed;

            vertices[0] = new Vector3((this.toNote.lane - this.fromNote.lane) * this.manager.lane_width - this.manager.lane_width / 2,
                -(this.toNote.time - this.fromNote.time) * PPS, 0f);
            vertices[3] = new Vector3(-this.manager.lane_width / 2, 0f, 0f);
            vertices[1] = new Vector3((this.toNote.lane - this.fromNote.lane) * this.manager.lane_width + this.manager.lane_width / 2,
                -(this.toNote.time - this.fromNote.time) * PPS, 0f);
            vertices[2] = new Vector3(this.manager.lane_width / 2, 0f, 0f);

            mesh.vertices = vertices;
            mesh.RecalculateBounds();

            this.transform.localPosition = new Vector3(this.manager.lane_width * (this.fromNote.lane - 3), this.y, 0.0f);
        }

        public NoteData GetNoteData(bool isFrom)
        {
            if (isFrom) return this.fromNote;
            else return this.toNote;
        }

        private float CalcTime()
        {
            return soundManager.time - (this.fromNote.time - this.manager.speed);
        }
    }
}