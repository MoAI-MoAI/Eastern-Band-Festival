using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BandParty
{
    public class SongDataSender : MonoBehaviour
    {
        public static Common.SongData songData;
        public static Common.Result result;
        public static int index = 0;
        public static int notesSum = 0;
        public static Common.DIFFICULTY difficulty;
        public static Common.SPEED speed;
    }
}
