using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class SlideLine : MonoBehaviour
    {

        public int index = 0;

        private NotesManager manager = null;
        private SoundManager soundManager = null;
        private NoteData fromNote = null;
        private NoteData toNote = null;

        [SerializeField]
        private float y = 0.0f;
        private NoteData prevFromNote;
        private NoteData prevToNote;

        private TapGesture gesture;

        private void OnEnable()
        {
            gesture = GetComponent<TapGesture>();
            gesture.Tapped += tappedHandler;
        }

        private void OnDisable()
        {
            gesture.Tapped -= tappedHandler;
        }

        // Update is called once per frame
        void Update()
        {
            if (!this.fromNote.isEnable || CalcTime() - (this.toNote.time - this.fromNote.time) >= this.manager.speed + this.manager.afterTime)
            {
                Destroy(this.gameObject);
                return;
            }

            if (this.fromNote != this.prevFromNote || this.toNote != this.prevToNote)
            {
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

                BoxCollider collider = GetComponent<BoxCollider>();
                collider.center = new Vector3(this.manager.lane_width * ((this.toNote.lane - this.fromNote.lane + (this.toNote.lane - this.fromNote.lane > 0 ? 1 : (this.toNote.lane - this.fromNote.lane < 0 ? -1 : 0))) / 2), (this.toNote.time - this.fromNote.time) / 2 * (Mathf.Abs(this.manager.to - this.manager.from) / this.manager.speed), 0f);
                collider.size = new Vector3((Mathf.Abs(this.toNote.lane - this.fromNote.lane) + 1) * this.manager.lane_width, Mathf.Abs(this.toNote.time - this.fromNote.time) * (Mathf.Abs(this.manager.to - this.manager.from) / this.manager.speed), collider.size.z);

                this.prevFromNote = this.fromNote.Clone();
                this.prevToNote = this.toNote.Clone();
            }

            this.y = this.manager.from + (this.manager.to - this.manager.from) * (this.CalcTime() / this.manager.speed);

            this.transform.position = new Vector3(this.manager.lane_width * (this.fromNote.lane - 3), this.y, 0.0f);
        }

        public void Launch(int index, NotesManager notesManager, SoundManager soundManager, NoteData fromNote, NoteData toNote)
        {
            this.index = index;
            this.manager = notesManager;
            this.soundManager = soundManager;
            this.fromNote = fromNote;
            this.toNote = toNote;
            this.y = this.manager.from;

            this.prevFromNote = this.fromNote.Clone();
            this.prevToNote = this.toNote.Clone();

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

            BoxCollider collider = GetComponent<BoxCollider>();
            collider.center = new Vector3(this.manager.lane_width * ((this.toNote.lane - this.fromNote.lane - 1) / 2), (this.toNote.time - this.fromNote.time) / 2 * (Mathf.Abs(this.manager.to - this.manager.from) / this.manager.speed), 0f);
            collider.size = new Vector3((Mathf.Abs(this.toNote.lane - this.fromNote.lane) + 1) * this.manager.lane_width, Mathf.Abs(this.toNote.time - this.fromNote.time) * (Mathf.Abs(this.manager.to - this.manager.from) / this.manager.speed), collider.size.z);

            this.transform.position = new Vector3(this.manager.lane_width * (this.fromNote.lane - 3), this.y, 0.0f);
        }

        public void SetToNote(NoteData toNote)
        {
            this.toNote = toNote;

            this.prevFromNote = this.fromNote.Clone();
            this.prevToNote = this.toNote.Clone();

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

            BoxCollider collider = GetComponent<BoxCollider>();
            collider.center = new Vector3(this.manager.lane_width * ((this.toNote.lane - this.fromNote.lane - 1) / 2), (this.toNote.time - this.fromNote.time) / 2 * (Mathf.Abs(this.manager.to - this.manager.from) / this.manager.speed), 0f);
            collider.size = new Vector3((Mathf.Abs(this.toNote.lane - this.fromNote.lane) + 1) * this.manager.lane_width, Mathf.Abs(this.toNote.time - this.fromNote.time) * (Mathf.Abs(this.manager.to - this.manager.from) / this.manager.speed), collider.size.z);
        }

        private void tappedHandler(object sender, System.EventArgs e)
        {
            TapGesture gesture = (TapGesture)sender;
            HitData hit = gesture.GetScreenPositionHitData();
            Vector3 point = hit.Point + hit.Normal * .5f;

            manager.editManager.OnSlideLineTapped(point, this.fromNote, this.toNote, this);
        }

        public NoteData GetNoteData()
        {
            return this.fromNote;
        }

        private float CalcTime()
        {
            return soundManager.GetTime() - (this.fromNote.time - this.manager.speed);
        }
    }
}
