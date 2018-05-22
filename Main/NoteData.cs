using UnityEngine;
using System.Collections;

using BandParty.Main;

namespace BandParty.Main
{
    public class NoteData
    {
        public int index;
        public byte lane;
        public Common.NOTE type;
        public float time;
        public int chainBehind;
        public int chainForward;
        public bool isActive;
        public SlideLine slideLine;
        public bool isJudged;

        public NoteData(int index, byte lane, byte type, float time, int chainBehind, int chainForword)
        {
            this.index = index;
            this.lane = lane;
            this.type = (Common.NOTE)type;
            this.time = time;
            this.chainBehind = chainBehind;
            this.chainForward = chainForword;

            this.isActive = true;
            this.slideLine = null;
            this.isJudged = false;
        }
    }
}
