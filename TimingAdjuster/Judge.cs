using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

using BandParty.Main;

namespace BandParty.TimingAdjuster {
    public class Judge : MonoBehaviour
    {
        public NotesManager notesManager = null;
        public SoundManager soundManager = null;
        public NotesTouchManager notesTouchManager = null;
        public AudioSource SEManager = null;
        public JudgeDisplay judgeDisplay = null;
        public GameObject regulationsParent = null;
        public float badRangeBefore = 0.3f;
        public float badRangeAfter = 0.5f;
        public float goodRangeBefore = 0.2f;
        public float goodRangeAfter = 0.4f;
        public float greatRangeBefore = 0.1f;
        public float greatRangeAfter = 0.2f;
        public float perfectRangeBefore = 0.03f;
        public float perfectRangeAfter = 0.1f;
        public float regulation = -1f;

        public void Init()
        {
            regulationsParent.transform.Find("Bad A").Find("InputField").GetComponent<InputField>().text = this.badRangeAfter.ToString();
            regulationsParent.transform.Find("Bad B").Find("InputField").GetComponent<InputField>().text = this.badRangeBefore.ToString();
            regulationsParent.transform.Find("Good A").Find("InputField").GetComponent<InputField>().text = this.goodRangeAfter.ToString();
            regulationsParent.transform.Find("Good B").Find("InputField").GetComponent<InputField>().text = this.goodRangeBefore.ToString();
            regulationsParent.transform.Find("Great A").Find("InputField").GetComponent<InputField>().text = this.greatRangeAfter.ToString();
            regulationsParent.transform.Find("Great B").Find("InputField").GetComponent<InputField>().text = this.greatRangeBefore.ToString();
            regulationsParent.transform.Find("Perfect A").Find("InputField").GetComponent<InputField>().text = this.perfectRangeAfter.ToString();
            regulationsParent.transform.Find("Perfect B").Find("InputField").GetComponent<InputField>().text = this.perfectRangeBefore.ToString();
            regulationsParent.transform.Find("Regulation").Find("InputField").GetComponent<InputField>().text = this.regulation.ToString();
            this.notesTouchManager.Init();
        }

        private void Update()
        {
            foreach(NoteData data in new List<NoteData>(this.notesManager.notes))
            {
                if (!data.isJudged && this.soundManager.time + this.regulation >= data.time)
                {
                    if (data.type == Common.NOTE.SLIDE_VIA)
                    {
                        List<byte> pressedLanes = this.notesTouchManager.GetPressedLanes();
                        bool isMiss = true;
                        foreach (byte lane in pressedLanes)
                        {
                            if (data.lane == lane)
                            {
                                //Debug.Log(this.name + ": Perfect");
                                judgeDisplay.Launch(Common.JUDGE.PERFECT);

                                SEManager.PlayOneShot(SEManager.clip);
                                notesManager.DestroyNote(data);
                                isMiss = false;
                                break;
                            }
                        }

                        data.isJudged = true;

                        if (isMiss) judgeDisplay.Launch(Common.JUDGE.MISS);
                    } else
                    {
                        if (this.soundManager.time + this.regulation > data.time + this.badRangeAfter)
                        {
                            judgeDisplay.Launch(Common.JUDGE.MISS);
                            data.isJudged = true;
                        }
                    }
                }
            }

        }

        public void EventJudge(byte eventLane, Common.EVENT eventTyoe)
        {
            List<byte> judgeLanes = new List<byte>();
            if (eventLane == 0)
            {
                judgeLanes.Add(0);
            } else if (1 <= eventLane && eventLane <= 6)
            {
                judgeLanes.Add((byte)(eventLane - 1));
                judgeLanes.Add(eventLane);
            } else if (eventLane == 7)
            {
                judgeLanes.Add(6);
            }

            foreach (NoteData note in this.notesManager.notes)
            {
                bool isLaneMatch = false;
                foreach (byte lane in judgeLanes) if (note.lane == lane) isLaneMatch = true;
                if (!isLaneMatch) continue;

                switch (eventTyoe)
                {
                    case Common.EVENT.PRESS:
                        if (note.type != Common.NOTE.SINGLE && note.type != Common.NOTE.SLIDE_PRESS) continue;
                        break;
                    case Common.EVENT.RELEASE:
                        if (note.type != Common.NOTE.SLIDE_RELEASE) continue;
                        break;
                    case Common.EVENT.FLICK:
                        if (note.type != Common.NOTE.FLICK) continue;
                        break;
                }

                float time = this.soundManager.time + this.regulation;
                if (time < note.time - this.badRangeBefore || note.time + this.badRangeAfter < time)
                {
                    continue;
                }
                else if (time < note.time - this.goodRangeBefore || note.time + this.goodRangeAfter < time)
                {
                    //Debug.Log(this.name + ": Bad");
                    judgeDisplay.Launch(Common.JUDGE.BAD);
                }
                else if (time < note.time - this.greatRangeBefore || note.time + this.greatRangeAfter < time)
                {
                    //Debug.Log(this.name + ": Good");
                    judgeDisplay.Launch(Common.JUDGE.GOOD);
                }
                else if (time < note.time - this.perfectRangeBefore || note.time + this.perfectRangeAfter < time)
                {
                    //Debug.Log(this.name + ": Great");
                    judgeDisplay.Launch(Common.JUDGE.GREAT);
                } else
                {
                    //Debug.Log(this.name + ": Perfect");
                    judgeDisplay.Launch(Common.JUDGE.PERFECT);
                }

                SEManager.PlayOneShot(SEManager.clip);
                notesManager.DestroyNote(note);
                break;
            }
        }

        public void SetRegulations()
        {
            this.badRangeAfter = Convert.ToSingle(regulationsParent.transform.Find("Bad A").Find("InputField").GetComponent<InputField>().text);
            this.badRangeBefore = Convert.ToSingle(regulationsParent.transform.Find("Bad B").Find("InputField").GetComponent<InputField>().text);
            this.goodRangeAfter = Convert.ToSingle(regulationsParent.transform.Find("Good A").Find("InputField").GetComponent<InputField>().text);
            this.goodRangeBefore = Convert.ToSingle(regulationsParent.transform.Find("Good B").Find("InputField").GetComponent<InputField>().text);
            this.greatRangeAfter = Convert.ToSingle(regulationsParent.transform.Find("Great A").Find("InputField").GetComponent<InputField>().text);
            this.greatRangeBefore = Convert.ToSingle(regulationsParent.transform.Find("Great B").Find("InputField").GetComponent<InputField>().text);
            this.perfectRangeAfter = Convert.ToSingle(regulationsParent.transform.Find("Perfect A").Find("InputField").GetComponent<InputField>().text);
            this.perfectRangeBefore = Convert.ToSingle(regulationsParent.transform.Find("Perfect B").Find("InputField").GetComponent<InputField>().text);
            this.regulation = Convert.ToSingle(regulationsParent.transform.Find("Regulation").Find("InputField").GetComponent<InputField>().text);
            this.notesTouchManager.SetRegulations();
        }
    }
}