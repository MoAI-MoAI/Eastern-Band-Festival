using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class NoteData
    {

        public enum NOTE : byte
        {
            SINGLE,
            SLIDE_PRESS,
            SLIDE_VIA,
            SLIDE_RELEASE,
            FLICK
        }

        public int index;
        public byte lane;
        public byte type;
        public float time;
        public int chainBehind;
        public int chainForward;
        public bool isEnable;
        public SlideLine slideLine;

        public NoteData(int index, byte lane, byte type, float time, int chainBehind, int chainForword)
        {
            this.index = index;
            this.lane = lane;
            this.type = type;
            this.time = time;
            this.chainBehind = chainBehind;
            this.chainForward = chainForword;

            this.isEnable = true;
            this.slideLine = null;
        }

        public NoteData Clone()
        {
            return (NoteData)MemberwiseClone();
        }
    }
}

