using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class TimingLineManager : MonoBehaviour
    {

        public SoundManager soundManager = null;
        public NotesManager notesManager = null;
        public GameObject timingLineParent = null;
        public GameObject barLinePrefab = null;
        public GameObject beatLinePrefab = null;
        public GameObject quarterLinePrefab = null;
        public Toggle showGrid = null;
        public float range = 4.0f;
        public float afterRange = 0.5f;

        private float prevTime = 0.0f;
        private GameObject barLineParent = null;
        private GameObject beatLineParent = null;
        private GameObject quarterLineParent = null;
        private bool isHidden = false;

        // Use this for initialization
        void Start()
        {
            barLineParent = timingLineParent.transform.Find("BarLines").gameObject;
            beatLineParent = timingLineParent.transform.Find("BeatLines").gameObject;
            quarterLineParent = timingLineParent.transform.Find("QuarterLines").gameObject;

            StartCoroutine("UpdateLines");
        }

        private IEnumerator UpdateLines()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                if (showGrid.isOn)
                {
                    float time = soundManager.GetTime();
                    if (this.prevTime == time && !this.isHidden) continue;
                    this.prevTime = time;
                    this.isHidden = false;

                    float SPB = 60.0f / soundManager.BPM; // Second Per Beat

                    //BarLine
                    List<TimingLine> Lines = new List<TimingLine>();

                    int from = (int)Math.Ceiling((time - this.afterRange) / (SPB * 4));
                    int to = (int)Math.Ceiling((time + this.range) / (SPB * 4));
                    for (int i = from; i < to; i++)
                    {
                        float y = this.notesManager.from + ((this.notesManager.to - this.notesManager.from) / this.notesManager.speed)
                            * ((time + this.notesManager.speed) - (i * (SPB * 4)));
                        Lines.Add(new TimingLine(y, i + 1));
                    }

                    Transform childTransform = barLineParent.transform;
                    foreach (Transform child in childTransform.transform)
                    {
                        TimingLineID timingLineID;
                        if ((timingLineID = child.GetComponent<TimingLineID>()) != null)
                        {
                            bool isUsing = false;

                            for (int i = 0; i < Lines.Count; i++)
                            {
                                if (Lines[i].visible) continue;
                                if (timingLineID.bar == Lines[i].index)
                                {
                                    Lines[i].visible = true;
                                    Vector3 vector = new Vector3(child.transform.position.x, Lines[i].y, child.transform.position.z);
                                    child.transform.position = vector;
                                    isUsing = true;
                                    break;
                                }
                            }

                            if (!isUsing) Destroy(child.gameObject);
                        }
                    }

                    for (int i = 0; i < Lines.Count; i++)
                    {
                        if (Lines[i].visible) continue;
                        Vector3 vector = new Vector3(0.0f, Lines[i].y, 0.0f);
                        GameObject obj = (GameObject)Instantiate(barLinePrefab, vector, Quaternion.identity);
                        obj.transform.parent = barLineParent.transform;
                        obj.GetComponent<TimingLineID>().bar = Lines[i].index;
                        obj.GetComponent<TimingLineID>().beat = 0;
                        obj.GetComponent<TimingLineID>().quarter = 0;
                        TextMesh text = obj.transform.Find("BarNumber").gameObject.GetComponent<TextMesh>();
                        GameObject go = obj.transform.Find("BarNumber").gameObject;
                        text.text = Lines[i].index.ToString();
                    }

                    //BeatLine
                    Lines = new List<TimingLine>();

                    from = (int)Math.Ceiling((time - this.afterRange) / SPB);
                    to = (int)Math.Ceiling((time + this.range) / SPB);
                    for (int i = from; i < to; i++)
                    {
                        if (i % 4 == 0) continue;
                        float y = this.notesManager.from + ((this.notesManager.to - this.notesManager.from) / this.notesManager.speed)
                            * ((time + this.notesManager.speed) - (i * SPB));
                        Lines.Add(new TimingLine(y, i + 4));
                    }

                    childTransform = beatLineParent.transform;
                    foreach (Transform child in childTransform.transform)
                    {
                        TimingLineID timingLineID;
                        if ((timingLineID = child.GetComponent<TimingLineID>()) != null)
                        {
                            bool isUsing = false;

                            for (int i = 0; i < Lines.Count; i++)
                            {
                                if (Lines[i].visible) continue;
                                if (timingLineID.bar == Lines[i].index)
                                {
                                    Lines[i].visible = true;
                                    Vector3 vector = new Vector3(child.transform.position.x, Lines[i].y, child.transform.position.z);
                                    child.transform.position = vector;
                                    isUsing = true;
                                    break;
                                }
                            }

                            if (!isUsing) Destroy(child.gameObject);
                        }
                    }

                    for (int i = 0; i < Lines.Count; i++)
                    {
                        if (Lines[i].visible) continue;
                        Vector3 vector = new Vector3(0.0f, Lines[i].y, 0.0f);
                        GameObject obj = (GameObject)Instantiate(beatLinePrefab, vector, Quaternion.identity);
                        obj.transform.parent = beatLineParent.transform;
                        obj.GetComponent<TimingLineID>().bar = Lines[i].index / 4 + 1;
                        obj.GetComponent<TimingLineID>().beat = Lines[i].index % 4;
                        obj.GetComponent<TimingLineID>().quarter = 0;
                    }

                    //QuarterLine
                    Lines = new List<TimingLine>();

                    from = (int)Math.Ceiling((time - this.afterRange) / (SPB / 4));
                    to = (int)Math.Ceiling((time + this.range) / (SPB / 4));
                    for (int i = from; i < to; i++)
                    {
                        if (i % 4 == 0) continue;
                        float y = this.notesManager.from + ((this.notesManager.to - this.notesManager.from) / this.notesManager.speed)
                            * ((time + this.notesManager.speed) - (i * (SPB / 4)));
                        Lines.Add(new TimingLine(y, i + 16));
                    }

                    childTransform = quarterLineParent.transform;
                    foreach (Transform child in childTransform.transform)
                    {
                        TimingLineID timingLineID;
                        if ((timingLineID = child.GetComponent<TimingLineID>()) != null)
                        {
                            bool isUsing = false;

                            for (int i = 0; i < Lines.Count; i++)
                            {
                                if (Lines[i].visible) continue;
                                if (timingLineID.bar == Lines[i].index)
                                {
                                    Lines[i].visible = true;
                                    Vector3 vector = new Vector3(child.transform.position.x, Lines[i].y, child.transform.position.z);
                                    child.transform.position = vector;
                                    break;
                                }
                            }

                            if (!isUsing) Destroy(child.gameObject);
                        }
                    }

                    for (int i = 0; i < Lines.Count; i++)
                    {
                        if (Lines[i].visible) continue;
                        Vector3 vector = new Vector3(0.0f, Lines[i].y, 0.0f);
                        GameObject obj = (GameObject)Instantiate(quarterLinePrefab, vector, Quaternion.identity);
                        obj.transform.parent = quarterLineParent.transform;
                        obj.GetComponent<TimingLineID>().quarter = Lines[i].index / 16 + 1;
                        obj.GetComponent<TimingLineID>().beat = Lines[i].index / 4 + 1;
                        obj.GetComponent<TimingLineID>().quarter = Lines[i].index % 4;
                    }
                }
                else
                {
                    this.isHidden = true;

                    Transform childrenTransform = barLineParent.transform;
                    foreach (Transform child in childrenTransform)
                    {
                        Destroy(child.gameObject);
                    }

                    childrenTransform = beatLineParent.transform;
                    foreach (Transform child in childrenTransform)
                    {
                        Destroy(child.gameObject);
                    }

                    childrenTransform = quarterLineParent.transform;
                    foreach (Transform child in childrenTransform)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }

        class TimingLine
        {
            public float y;
            public int index;
            public bool visible = false;

            public TimingLine(float y, int index)
            {
                this.y = y;
                this.index = index;
            }
        }
    }
}