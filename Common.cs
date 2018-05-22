using UnityEngine;

namespace BandParty
{
    public static class Common
    {
        public enum DIFFICULTY
        {
            EASY,
            NORMAL,
            HARD,
            EXPERT
        }

        public enum NOTE : byte
        {
            SINGLE,
            SLIDE_PRESS,
            SLIDE_VIA,
            SLIDE_RELEASE,
            FLICK,
            NONE
        }

        public enum EVENT : byte
        {
            PRESS,
            RELEASE,
            FLICK
        }

        public enum JUDGE : byte
        {
            MISS,
            BAD,
            GOOD,
            GREAT,
            PERFECT
        }

        public enum SPEED
        {
            SLOW,       //speed=6.0f
            NORMAL,     //speed=4.0f
            FAST        //speed=2.0f
        }

        public enum SCORERATE
        {
            C,
            B,
            A,
            S,
            SS
        }

        public enum SCOREDATA
        {
            INDEX = 0,
            LANE,
            TYPE,
            TIME,
            CHAINBEHIND,
            CHAINFORWARD
        }

        public enum RECORDDATA
        {
            INDEX = 0,
            NICKNAME,
            SCORE,
            MAXCOMBO,
            SCORERATE,
            PERFECT,
            GREAT,
            GOOD,
            BAD,
            MISS
        }

        public enum CONFIG
        {
            BAD_A = 0,
            BAD_B,
            GOOD_A,
            GOOD_B,
            GREAT_A,
            GREAT_B,
            PERFECT_A,
            PERFECT_B,
            TOUCHREGULATION,
            FLICKDISTANCE
        }

        public struct Result
        {
            public int index;
            public string nickname;
            public int score;
            public int maxCombo;
            public SCORERATE scoreRate;
            public int perfectCount;
            public int greatCount;
            public int goodCount;
            public int badCount;
            public int missCount;

            public Result(int index, int score, int maxCombo, SCORERATE scoreRate, int perfectCount, int greatCount, int goodCount, int badCount, int missCount)
            {
                this.index = index;
                this.nickname = "名無しさん";
                this.score = score;
                this.maxCombo = maxCombo;
                this.scoreRate = scoreRate;
                this.perfectCount = perfectCount;
                this.greatCount = greatCount;
                this.goodCount = goodCount;
                this.badCount = badCount;
                this.missCount = missCount;
            }
        }

        public struct SongData
        {
            public int index;
            public string songName;
            public Texture2D jacketImage;
        }

        public static float touchPointHeight = -3.83f;
        public static float touchPointLaneWidth = 126.5f;

        public static string DifficultyToString(DIFFICULTY difficulty)
        {
            switch (difficulty)
            {
                case DIFFICULTY.EASY:
                    return "EASY";
                case DIFFICULTY.NORMAL:
                    return "NORMAL";
                case DIFFICULTY.HARD:
                    return "HARD";
                case DIFFICULTY.EXPERT:
                default:
                    return "EXPERT";
            }
        }

        public static string JudgeToString(JUDGE judge)
        {
            switch (judge)
            {
                case JUDGE.MISS:
                    return "MISS";
                case JUDGE.BAD:
                    return "BAD";
                case JUDGE.GOOD:
                    return "GOOD";
                case JUDGE.GREAT:
                    return "GREAT";
                case JUDGE.PERFECT:
                    return "PERFECT";
                default:
                    return "?";
            }
        }
    }
}
