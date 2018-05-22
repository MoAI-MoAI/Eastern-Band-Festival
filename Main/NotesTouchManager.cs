using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TouchScript;
using TouchScript.Pointers;

using BandParty.Main;

namespace BandParty.Main {
    public class NotesTouchManager : MonoBehaviour
    {
        public Judge judge = null;
        public MockToast toast = null;
        public GameObject touchSensesParent = null;
        public float flickBorderDistance = 1f;
        
        public struct PressedLane
        {
            public byte lane;
            public int pointerId;
        }

        private struct PressAndFlick
        {
            public int pointerId;
            public bool isFlicked;
        }
        private List<PressAndFlick> oneFlickKeeper;

        private List<GameObject> touchSenses;

        private void OnEnable()
        {
            TouchManager.Instance.PointersPressed += this.onPressedHandler;
            TouchManager.Instance.PointersReleased += this.onReleasedHandler;

            this.touchSenses = new List<GameObject>();
            foreach (Transform child in this.touchSensesParent.transform)
            {
                this.touchSenses.Add(child.gameObject);
            }

            this.oneFlickKeeper = new List<PressAndFlick>();
        }

        private void Update()
        {
            foreach (Pointer pointer in TouchManager.Instance.PressedPointers)
            {
                foreach (PressAndFlick pressAndFlick in this.oneFlickKeeper)
                {
                    if (pressAndFlick.pointerId == pointer.Id)
                    {
                        //Debug.Log(this.name + ": ThroughProcess id=" + pointer.Id);
                        if (pressAndFlick.isFlicked) goto ThroughProcess;
                    }
                }

                Vector2 position = pointer.Position;
                Vector2 prevPos = pointer.PreviousPosition;
                if (Mathf.Pow(position.x - prevPos.x, 2) + Mathf.Pow(position.y - prevPos.y, 2) < Mathf.Pow(this.flickBorderDistance, 2)) continue;

                Vector2 pos = pointer.Position;
                pos.x -= Screen.width / 2;
                pos.y = Screen.height / 2 - pos.y;
                Vector2 touchPointsPos = this.touchSensesParent.GetComponent<RectTransform>().position;
                Vector2 touchPointsSize = this.touchSensesParent.GetComponent<RectTransform>().sizeDelta;
                if (pos.x < touchPointsPos.x - touchPointsSize.x || touchPointsPos.x < pos.x || pos.y < touchPointsPos.y || touchPointsPos.y + touchPointsSize.y < pos.y) continue;
                float touchPointWidth = touchPointsSize.x / 14;
                float absolutePos = pos.x - (touchPointsPos.x - touchPointsSize.x);
                byte lane = (byte)Mathf.Floor(absolutePos / touchPointWidth);

                //toast.Launch("Flicked " + Time.time.ToString());
                this.judge.EventJudge(lane, Common.EVENT.FLICK, pointer.Id);
                for (int j = 0; j < this.oneFlickKeeper.Count; j++)
                {
                    PressAndFlick pressAndFlick = this.oneFlickKeeper[j];
                    if (pressAndFlick.pointerId == pointer.Id)
                    {
                        //Debug.Log(this.name + ": Flicked id=" + pointer.Id);
                        pressAndFlick.isFlicked = true;
                    }
                }         

                ThroughProcess:;
            }
        }

        public List<PressedLane> GetPressedLanes()
        {
            List<PressedLane> pressedLanes = new List<PressedLane>();

            foreach (Pointer pointer in TouchManager.Instance.PressedPointers)
            {
                Vector2 pos = pointer.Position;
                pos.x -= Screen.width / 2;
                pos.y = Screen.height / 2 - pos.y;
                Vector2 touchPointsPos = this.touchSensesParent.GetComponent<RectTransform>().position;
                Vector2 touchPointsSize = this.touchSensesParent.GetComponent<RectTransform>().sizeDelta;
                //Debug.Log(this.name + ": " + (pos.x < touchPointsPos.x - touchPointsSize.x) + "," + (touchPointsPos.x < pos.x) + "," + (pos.y < touchPointsPos.y) + "," + (touchPointsPos.y + touchPointsSize.y < pos.y) + " " + pos.ToString() + "," + touchPointsPos.ToString() + "," + touchPointsSize.ToString());
                if (pos.x < touchPointsPos.x - touchPointsSize.x || touchPointsPos.x < pos.x || pos.y < touchPointsPos.y|| touchPointsPos.y + touchPointsSize.y < pos.y) continue;
                float touchPointWidth = touchPointsSize.x / 14;
                float absolutePos = pos.x - (touchPointsPos.x - touchPointsSize.x);
                byte lane = (byte)Mathf.Floor(absolutePos / touchPointWidth);
                //Debug.Log(this.name + ": lane=" + lane);

                if (lane == 0)
                {
                    PressedLane pressedLane;
                    pressedLane.lane = 0;
                    pressedLane.pointerId = pointer.Id;
                    pressedLanes.Add(pressedLane);
                    //Debug.Log(this.name + ": lane" + pressedLane.lane.ToString() + " i=" + lane.ToString());
                }
                else if (1 <= lane && lane <= 12)
                {
                    PressedLane pressedLane1;
                    pressedLane1.pointerId = pointer.Id;
                    if (lane % 2 == 0)
                    {
                        pressedLane1.lane = (byte)(Mathf.Floor(lane / 2) - 1);
                    }
                    else
                    {
                        pressedLane1.lane = (byte)(Mathf.Floor(lane / 2) + 1);
                    }
                    pressedLanes.Add(pressedLane1);
                    //Debug.Log(this.name + ": lane" + pressedLane1.lane.ToString() + " i=" + lane.ToString());

                    PressedLane pressedLane2;
                    pressedLane2.lane = (byte)Mathf.Floor(lane / 2);
                    pressedLane2.pointerId = pointer.Id;
                    pressedLanes.Add(pressedLane2);
                    //Debug.Log(this.name + ": lane" + pressedLane2.lane.ToString() + " i=" + lane.ToString());
                }
                else if (lane == 13)
                {
                    PressedLane pressedLane;
                    pressedLane.lane = 6;
                    pressedLane.pointerId = pointer.Id;
                    pressedLanes.Add(pressedLane);
                }
            }

            return pressedLanes;
        }

        private void onPressedHandler(object sender, PointerEventArgs args)
        {
            foreach (Pointer pointer in args.Pointers)
            {
                //Debug.Log(this.name + ": Pressed id=" + pointer.Id.ToString());

                Transform target = pointer.GetPressData().Target;
                if (target == null) continue;
                sbyte senseIndex = -1;
                for(sbyte i = 0; i < this.touchSenses.Count ; i++ )
                {
                    if (target.gameObject == this.touchSenses[i])
                    {
                        senseIndex = i;
                        break;
                    }
                }
                if (senseIndex == -1) continue;

                this.judge.EventJudge((byte)senseIndex, Common.EVENT.PRESS, pointer.Id);

                //Debug.Log("object=" + target.ToString()
                //        + " senseIndex=" + senseIndex.ToString());

                PressAndFlick pressAndFlick;
                pressAndFlick.pointerId = pointer.Id;
                pressAndFlick.isFlicked = false;
                this.oneFlickKeeper.Add(pressAndFlick);
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

                //Debug.Log(this.name + ": Released id=" + pointer.Id);
                this.judge.EventJudge((byte)senseIndex, Common.EVENT.RELEASE, pointer.Id);

                for (int i = 0; i < this.oneFlickKeeper.Count; i++)
                {
                    if (this.oneFlickKeeper[i].pointerId == pointer.Id)
                    {
                        this.oneFlickKeeper.Remove(this.oneFlickKeeper[i]);
                        break;
                    }
                }
            }
        }
    }
}

