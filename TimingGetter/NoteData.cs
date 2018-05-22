using UnityEngine;
using System.Collections;

using BandParty.TimingGetter;

namespace BandParty.TimingGetter
{
    public class NoteData
    {
        public int index;
        public float time;

        public NoteData(int index, float time)
        {
            this.index = index;
            this.time = time;
        }
    }
}
