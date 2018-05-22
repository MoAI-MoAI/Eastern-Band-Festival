using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine;

namespace BandParty.Result
{
    [DefaultExecutionOrder(-1)]
    public class RecordManager : MonoBehaviour
    {
        public List<Common.Result> recordList;

        private string recordPath;

        // Use this for initialization
        void Start()
        {
            this.recordPath = Application.persistentDataPath + "/record/";

            this.recordList = new List<Common.Result>();
            LoadRecord();
        }

        public int GetRank(Common.Result result)
        {
            int rank = 1;
            foreach (Common.Result record in this.recordList)
            {
                if (record.score > result.score) rank++;
            }

            return rank;
        }

        private void LoadRecord()
        {
            if (!File.Exists(this.recordPath + SongDataSender.songData.songName + "/" + SongDataSender.songData.songName + "-" + SongDataSender.difficulty.ToString() + ".csv"))
            {
                Debug.Log(this.name + ": NOCSVFILE " + this.recordPath + SongDataSender.songData.songName + "/" + SongDataSender.songData.songName + "-" + SongDataSender.difficulty.ToString() + ".csv");
                return;
            }

            int maxIndex = 0;
            try
            {
                FileStream fs = new FileStream(this.recordPath + SongDataSender.songData.songName + "/" + SongDataSender.songData.songName + "-" + SongDataSender.difficulty.ToString() + ".csv", FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fs);

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    Common.Result record = new Common.Result(
                        Convert.ToInt32(values[(int)Common.RECORDDATA.INDEX]),
                        Convert.ToInt32(values[(int)Common.RECORDDATA.SCORE]),
                        Convert.ToInt32(values[(int)Common.RECORDDATA.MAXCOMBO]),
                        (Common.SCORERATE)Enum.ToObject(typeof(Common.SCORERATE), Convert.ToInt32(values[(int)Common.RECORDDATA.SCORERATE])),
                        Convert.ToInt32(values[(int)Common.RECORDDATA.PERFECT]),
                        Convert.ToInt32(values[(int)Common.RECORDDATA.GREAT]),
                        Convert.ToInt32(values[(int)Common.RECORDDATA.GOOD]),
                        Convert.ToInt32(values[(int)Common.RECORDDATA.BAD]),
                        Convert.ToInt32(values[(int)Common.RECORDDATA.MISS]));

                    this.recordList.Add(record);

                    if (record.index > maxIndex) maxIndex = record.index;
                }

                reader.Close();
                fs.Close();
            }
            catch (System.Exception e)
            {
                Debug.Log(this.name + ": " + e.ToString());
                return;
            }

            SongDataSender.index = maxIndex + 1;
        }

        public void RecordResult(Common.Result result)
        {
            if (!Directory.Exists(this.recordPath + SongDataSender.songData.songName))
            {
                Directory.CreateDirectory(this.recordPath + SongDataSender.songData.songName);
            }

            try
            {
                FileStream fs = new FileStream(this.recordPath + SongDataSender.songData.songName + "/" + SongDataSender.songData.songName + "-" + SongDataSender.difficulty.ToString() + ".csv", FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);

                StringBuilder sb = new StringBuilder();
                sb.Append(SongDataSender.index.ToString() + ",");
                sb.Append(result.nickname + ",");
                sb.Append(result.score.ToString() + ",");
                sb.Append(result.maxCombo.ToString() + ",");
                sb.Append(((int)result.scoreRate).ToString() + ",");
                sb.Append(result.perfectCount.ToString() + ",");
                sb.Append(result.greatCount.ToString() + ",");
                sb.Append(result.goodCount.ToString() + ",");
                sb.Append(result.badCount.ToString() + ",");
                sb.Append(result.missCount.ToString());
                writer.WriteLine(sb.ToString());

                writer.Close();
                fs.Close();
            }
            catch (System.Exception e)
            {
                Debug.Log(this.name +  ": " + e.ToString());
                return;
            }

            SongDataSender.index++;
        } 
    }
}