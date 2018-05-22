using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.Main {
    public class Judge : MonoBehaviour
    {
        public NotesManager notesManager = null;
        public SoundManager soundManager = null;
        public NotesTouchManager notesTouchManager = null;
        public SEManager SEManager = null;
        public JudgeDisplay judgeDisplay = null;
        public ScoreManager scoreManager = null;
        public HpManager hpManager = null;
        public ComboManager comboManager = null;
        public EffectManager effectManager = null;
        public MockToast mockToast = null;
        public float badRangeBefore = 0.3f;
        public float badRangeAfter = 0.5f;
        public float goodRangeBefore = 0.2f;
        public float goodRangeAfter = 0.4f;
        public float greatRangeBefore = 0.1f;
        public float greatRangeAfter = 0.2f;
        public float perfectRangeBefore = 0.03f;
        public float perfectRangeAfter = 0.1f;
        public float regulation = -1f;

        private struct AimedRelease
        {
            public int pointerId;
            public NoteData note;
        }
        private List<AimedRelease> aimedRelease;

        private void Start()
        {
            this.aimedRelease = new List<AimedRelease>();
        }

        private void Update()
        {
            try
            {
                for (int j = 0; j < this.notesManager.notes.Count; j++)
                {
                    NoteData data = this.notesManager.notes[j];
                    if (!data.isJudged && this.soundManager.time + this.regulation >= data.time)
                    {
                        if (data.type == Common.NOTE.SLIDE_VIA)
                        {
                            List<NotesTouchManager.PressedLane> pressedLanes = this.notesTouchManager.GetPressedLanes();
                            bool isMiss = true;
                            foreach (NotesTouchManager.PressedLane pLane in pressedLanes)
                            {
                                if (data.lane == pLane.lane)
                                {
                                    Debug.Log(this.name + ": SlideViaHit lane=" + pLane.lane.ToString());
                                    this.judgeDisplay.Launch(Common.JUDGE.PERFECT);
                                    this.scoreManager.ScoreAdd(Common.JUDGE.PERFECT);
                                    this.comboManager.AddCombo();
                                    this.SEManager.Play(2);
                                    this.effectManager.GenerateEffect(data.type, pLane.lane);

                                    data.slideLine.isPressing = true;//
                                    data.slideLine.effectTransform = this.effectManager.GenerateSlideEffect(pLane.lane).GetComponent<RectTransform>();

                                    bool isAimed = false;
                                    for (int i = 0; i < this.aimedRelease.Count; i++)
                                    {
                                        AimedRelease aim = this.aimedRelease[i];
                                        if (aim.pointerId != pLane.pointerId) continue;
                                        foreach (NoteData aimNote in this.notesManager.notes)
                                        {
                                            if (aimNote.index == data.chainBehind)
                                            {
                                                aim.note = aimNote;
                                                this.aimedRelease[i] = aim;
                                                isAimed = true;
                                                Debug.Log(this.name + ": aimChange index=" + aim.note.index + " lane=" + aim.note.lane + " id=" + aim.pointerId + " ex=" + data.index);
                                                break;
                                            }
                                        }
                                    }
                                    if (!isAimed)
                                    {
                                        AimedRelease aim = new AimedRelease();
                                        aim.pointerId = pLane.pointerId;
                                        foreach (NoteData aimNote in this.notesManager.notes)
                                        {
                                            if (aimNote.index == data.chainBehind)
                                            {
                                                aim.note = aimNote;
                                                Debug.Log(this.name + ": aim lane=" + aim.note.lane + " id=" + aim.pointerId);
                                                break;
                                            }
                                        }
                                        if (aim.note == null)
                                        {
                                            int a = 0;
                                        }
                                        this.aimedRelease.Add(aim);
                                    }

                                    this.notesManager.DestroyNote(data);

                                    isMiss = false;
                                    break;
                                }
                            }

                            data.isJudged = true;

                            if (isMiss)
                            {
                                //Debug.Log(this.name + ": Miss");
                                this.judgeDisplay.Launch(Common.JUDGE.MISS);
                                this.scoreManager.ScoreAdd(Common.JUDGE.MISS);
                                this.hpManager.Damage(Common.JUDGE.MISS);
                                this.SEManager.Play(0);
                                this.comboManager.LoseCombo();

                                foreach (AimedRelease aim in this.aimedRelease)
                                {

                                    if (aim.note.index == data.index) //
                                    {
                                        this.aimedRelease.Remove(aim);
                                        Debug.Log(this.name + ": aimDelete lane=" + aim.note.lane + " id=" + aim.pointerId);
                                        break;
                                    }
                                }

                                this.notesManager.DestroyNote(data);
                            }
                        }
                        else
                        {
                            if (this.soundManager.time + this.regulation > data.time + this.badRangeAfter)
                            {
                                //Debug.Log(this.name + ": Miss");
                                this.judgeDisplay.Launch(Common.JUDGE.MISS);
                                this.scoreManager.ScoreAdd(Common.JUDGE.MISS);
                                this.hpManager.Damage(Common.JUDGE.MISS);
                                this.SEManager.Play(0);
                                this.comboManager.LoseCombo();
                                data.isJudged = true;

                                if (data.type == Common.NOTE.SLIDE_RELEASE)
                                {
                                    foreach (AimedRelease aim in this.aimedRelease)
                                    {
                                        if (aim.note.index == data.index)//
                                        {
                                            this.aimedRelease.Remove(aim);
                                            Debug.Log(this.name + ": aimDelete lane=" + aim.note.lane + " id=" + aim.pointerId);
                                            break;
                                        }
                                    }
                                }

                                this.notesManager.DestroyNote(data);
                            }
                        }
                    }
                }

            } catch (System.Exception e)
            {
                this.mockToast.Launch(e.ToString());
                Debug.Log(this.name + ": " + e.ToString());
            }
        }

        public void EventJudge(byte eventLane, Common.EVENT eventTyoe, int pointerId)
        {
            List<byte> judgeLanes = new List<byte>();
            byte trueLane = 0;
            if (eventLane == 0)
            {
                judgeLanes.Add(0);
                trueLane = 0;
            }
            else if (1 <= eventLane && eventLane <= 12)
            {
                if (eventLane % 2 == 0)
                {
                    judgeLanes.Add((byte)(Mathf.Floor(eventLane / 2) - 1));
                }
                else
                {
                    judgeLanes.Add((byte)(Mathf.Floor(eventLane / 2) + 1));
                }
                judgeLanes.Add((byte)Mathf.Floor(eventLane / 2));
                trueLane = (byte)Mathf.Floor(eventLane / 2);
            }
            else if (eventLane == 13)
            {
                judgeLanes.Add(6);
                trueLane = 6;
            }

            NoteData nearestNote = null;
            float time = this.soundManager.time + this.regulation;
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

                if (note.time - this.badRangeBefore < time && time < note.time + this.badRangeAfter)
                {
                    if (note.type == Common.NOTE.SLIDE_RELEASE)
                    {
                        bool isForwardExists = false;
                        foreach (NoteData nd in this.notesManager.notes)
                        {
                            if (note.chainForward == nd.index)
                            {
                                isForwardExists = true;
                            }
                        }
                        if (isForwardExists)
                        {
                            continue;
                        }
                    }

                    if (nearestNote == null) nearestNote = note;
                    else if (Mathf.Abs(nearestNote.time - time) > Mathf.Abs(note.time - time)) nearestNote = note;
                }
            }

            if (nearestNote != null)
            {
                Debug.Log(this.name + ": JudgeEnter index=" + nearestNote.index + " type=" + nearestNote.type + " lane=" + nearestNote.lane);
                if (time < nearestNote.time - this.goodRangeBefore || nearestNote.time + this.goodRangeAfter < time)
                {
                    //Debug.Log(this.name + ": Bad");
                    judgeDisplay.Launch(Common.JUDGE.BAD);
                    this.scoreManager.ScoreAdd(Common.JUDGE.BAD);
                    this.hpManager.Damage(Common.JUDGE.BAD);
                    this.comboManager.LoseCombo();
                    this.SEManager.Play(0);
                }
                else if (time < nearestNote.time - this.greatRangeBefore || nearestNote.time + this.greatRangeAfter < time)
                {
                    //Debug.Log(this.name + ": Good");
                    judgeDisplay.Launch(Common.JUDGE.GOOD);
                    this.scoreManager.ScoreAdd(Common.JUDGE.GOOD);
                    this.comboManager.LoseCombo();
                    this.SEManager.Play(1);
                }
                else if (time < nearestNote.time - this.perfectRangeBefore || nearestNote.time + this.perfectRangeAfter < time)
                {
                    //Debug.Log(this.name + ": Great");
                    judgeDisplay.Launch(Common.JUDGE.GREAT);
                    this.scoreManager.ScoreAdd(Common.JUDGE.GREAT);
                    this.comboManager.AddCombo();
                    this.SEManager.Play(1);
                }
                else
                {
                    //Debug.Log(this.name + ": Perfect");
                    judgeDisplay.Launch(Common.JUDGE.PERFECT);
                    this.scoreManager.ScoreAdd(Common.JUDGE.PERFECT);
                    this.comboManager.AddCombo();
                    this.SEManager.Play(2);
                }

                notesManager.DestroyNote(nearestNote);
                this.effectManager.GenerateEffect(nearestNote.type, nearestNote.lane);

                if (nearestNote.type == Common.NOTE.SLIDE_PRESS)
                {
                    nearestNote.slideLine.isPressing = true;
                    nearestNote.slideLine.effectTransform = this.effectManager.GenerateSlideEffect(nearestNote.lane).GetComponent<RectTransform>();
                    this.SEManager.PlayLoop(3);

                    AimedRelease aim = new AimedRelease();
                    aim.pointerId = pointerId;
                    foreach (NoteData aimNote in this.notesManager.notes)
                    {
                        if (aimNote.index == nearestNote.chainBehind)
                        {
                            aim.note = aimNote;
                            Debug.Log(this.name + ": aim lane=" + aim.note.lane + " id=" + aim.pointerId);
                            break;
                        }
                    }
                    this.aimedRelease.Add(aim);
                } else if (nearestNote.type == Common.NOTE.SLIDE_RELEASE || nearestNote.type == Common.NOTE.FLICK)
                {
                    this.SEManager.StopLoop();

                    foreach (AimedRelease aim in this.aimedRelease)
                    {
                        if (aim.pointerId != pointerId) continue;
                        this.aimedRelease.Remove(aim);
                        Debug.Log(this.name + ": aimRelease lane=" + aim.note.lane + " id=" + aim.pointerId);
                        return;
                    }
                }
            } else
            {
                if (eventTyoe == Common.EVENT.PRESS)
                {
                    //Debug.Log("pressed");
                    this.effectManager.GenerateEffect(Common.NOTE.NONE, trueLane);
                }

                if (eventTyoe == Common.EVENT.RELEASE && this.aimedRelease.Count != 0)
                {
                    foreach (AimedRelease aim in this.aimedRelease)
                    {
                        if (aim.pointerId != pointerId) continue;
                        Debug.Log(this.name + ": aimDelete lane=" + aim.note.lane + " id=" + aim.pointerId);
                        this.notesManager.DestroyNote(aim.note);//
                        this.aimedRelease.Remove(aim);

                        //Debug.Log(this.name + ": Miss");
                        this.judgeDisplay.Launch(Common.JUDGE.MISS);
                        this.scoreManager.ScoreAdd(Common.JUDGE.MISS);
                        this.hpManager.Damage(Common.JUDGE.MISS);
                        this.SEManager.Play(0);
                        this.SEManager.StopLoop();
                        this.comboManager.LoseCombo();
                        return;
                    }
                }
            }
        }
    }
}