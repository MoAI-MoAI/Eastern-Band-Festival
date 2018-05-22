using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BandParty.Main
{
    [DefaultExecutionOrder(1)]
    public class ScoreManager : MonoBehaviour
    {
        public ScoreValueManager scoreValueManager = null;
        public NotesManager notesManager = null;
        public ComboManager comboManager = null;
        public int score = 0;
        public int perfectCount = 0;
        public int greatCount = 0;
        public int goodCount = 0;
        public int badCount = 0;
        public int missCount = 0;
        public int badPoint = 58;
        public int goodPoint = 129;
        public int greatPoint = 183;
        public int perfectPoint = 210;

        public void SetMaxScore()
        {
            this.scoreValueManager.scoreMax = this.notesManager.notes.Count * this.perfectPoint;
        }

        public void ScoreAdd(Common.JUDGE judge)
        {
            //Debug.Log(this.name + ": " + judge.ToString());
            switch (judge)
            {
                case Common.JUDGE.MISS:
                    this.missCount++;
                    break;
                case Common.JUDGE.BAD:
                    score += this.badPoint;
                    this.badCount++;
                    break;
                case Common.JUDGE.GOOD:
                    score += this.goodPoint;
                    this.goodCount++;                    
                    break;
                case Common.JUDGE.GREAT:
                    score += this.greatPoint;
                    this.greatCount++;
                    break;
                case Common.JUDGE.PERFECT:
                    score += this.perfectPoint;
                    this.perfectCount++;
                    break;
            }

            this.scoreValueManager.UpdateValue(this.score);
        }

        public Common.Result GetResult()
        {
            Common.Result result = new Common.Result();
            result.score = this.score;
            result.maxCombo = this.comboManager.maxCombo;
            float scoreRatio = this.score / (this.notesManager.backUp.Count * this.perfectPoint);
            if (scoreRatio < 0.3f)
            {
                result.scoreRate = Common.SCORERATE.C;
            } else if (scoreRatio < 0.6f)
            {
                result.scoreRate = Common.SCORERATE.B;
            } else if (scoreRatio < 0.8f)
            {
                result.scoreRate = Common.SCORERATE.A;
            } else if (scoreRatio < 0.99f)
            {
                result.scoreRate = Common.SCORERATE.S;
            } else
            {
                result.scoreRate = Common.SCORERATE.SS;
            } 
            result.perfectCount = this.perfectCount;
            result.greatCount = this.greatCount;
            result.goodCount = this.goodCount;
            result.badCount = this.badCount;
            result.missCount = this.missCount;

            return result;
        }
    }
}