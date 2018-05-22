using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BandParty.Main
{
    public class HpManager : MonoBehaviour
    {
        public HPValueManager hpValueManager = null;
        public GameManager gameManager = null;
        public int hp = 1000;
        public int missPoint = 200;
        public int badPoint = 50;

        private void Start()
        {
            this.hp = this.hpValueManager.HpMax;
        }

        public void Damage(Common.JUDGE judge)
        {
            if (this.hp <= 0) return;
            switch (judge)
            {
                case Common.JUDGE.MISS:
                    this.hp -= this.missPoint;
                    if (this.hp <= 0) this.hp = 0;
                    break;
                case Common.JUDGE.BAD:
                    this.hp -= this.badPoint;
                    if (this.hp <= 0) this.hp = 0;
                    break;
            }

            this.hpValueManager.UpdateValue(this.hp);

            //if (this.hp == 0) this.gameManager.GameOver();
        }
    }
}