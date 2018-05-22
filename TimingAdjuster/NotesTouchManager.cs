using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TouchScript;
using TouchScript.Pointers;

namespace BandParty.TimingAdjuster {
    public class NotesTouchManager : MonoBehaviour
    {
        public Judge judge = null;
        public GameObject touchSensesParent = null;
        public GameObject regulationsParent = null;
        public float flickBorderDistance = 1f;
        public List<int> PointerIds = null;

        private List<GameObject> touchSenses;

        public void Init()
        {
            regulationsParent.transform.Find("FlickDist").Find("InputField").GetComponent<InputField>().text = this.flickBorderDistance.ToString();
        }

        private void OnEnable()
        {
            TouchManager.Instance.PointersPressed += this.onPressedHandler;
            TouchManager.Instance.PointersReleased += this.onReleasedHandler;

            this.touchSenses = new List<GameObject>();
            foreach (Transform child in this.touchSensesParent.transform)
            {
                this.touchSenses.Add(child.gameObject);
            }

            this.PointerIds = new List<int>();
        }

        private void Update()
        {
            foreach (Pointer point in TouchManager.Instance.PressedPointers)
            {
                Transform target = point.GetPressData().Target;
                if (target == null) continue;
                Vector2 position = point.Position;
                Vector2 prevPos = point.PreviousPosition;
                if (Mathf.Pow(position.x - prevPos.x, 2) + Mathf.Pow(position.y - prevPos.y, 2) < Mathf.Pow(this.flickBorderDistance, 2)) continue;
                for (sbyte i = 0; i < this.touchSenses.Count; i++)
                {
                    if (target.gameObject == this.touchSenses[i])
                    {
                        this.judge.EventJudge((byte)i, Common.EVENT.FLICK);
                        break;
                    }
                }
            }
        }

        public List<byte> GetPressedLanes()
        {
            List<byte> pressedLanes = new List<byte>();

            foreach (Pointer point in TouchManager.Instance.PressedPointers)
            {
                Transform target = point.GetPressData().Target;
                if (target == null) continue;
                for (byte i = 0; i < this.touchSenses.Count; i++)
                {
                    if (target.gameObject == this.touchSenses[i])
                    {
                        if (i == 0)
                        {
                            pressedLanes.Add(0);
                        } else if (1 <= i && i <= 6)
                        {
                            pressedLanes.Add((byte)(i - 1));
                            pressedLanes.Add(i);
                        } else if (i == 7)
                        {
                            pressedLanes.Add(6);
                        }
                    }
                }
            }

            return pressedLanes;
        }

        public void SetRegulations()
        {
            this.flickBorderDistance = Convert.ToSingle(regulationsParent.transform.Find("FlickDist").Find("InputField").GetComponent<InputField>().text);
        }

        private void onPressedHandler(object sender, PointerEventArgs args)
        {
            foreach (Pointer pointer in args.Pointers)
            {
                //Debug.Log(this.name + ": Pressed id=" + pointer.Id.ToString());

                Transform target = pointer.GetPressData().Target;
                if (target == null) continue;
                sbyte senseIndex = -1;
                for (sbyte i = 0; i < this.touchSenses.Count; i++)
                {
                    if (target.gameObject == this.touchSenses[i])
                    {
                        senseIndex = i;
                        break;
                    }
                }
                if (senseIndex == -1) continue;

                this.judge.EventJudge((byte)senseIndex, Common.EVENT.PRESS);

                //Debug.Log("object=" + target.ToString()
                //        + " senseIndex=" + senseIndex.ToString());

                this.PointerIds.Add(pointer.Id);
            }
        }

        private void onReleasedHandler(object sender, PointerEventArgs args)
        {
            foreach (Pointer pointer in args.Pointers)
            {
                Transform target = pointer.GetPressData().Target;
                if (target == null) continue;
                sbyte senseIndex = -1;
                for (sbyte i = 0; i < this.touchSenses.Count; i++)
                {
                    if (target.gameObject == this.touchSenses[i])
                    {
                        senseIndex = i;
                        break;
                    }
                }
                if (senseIndex == -1) continue;

                this.judge.EventJudge((byte)senseIndex, Common.EVENT.RELEASE);

                this.PointerIds.Remove(pointer.Id);
            }
        }
    }
}

