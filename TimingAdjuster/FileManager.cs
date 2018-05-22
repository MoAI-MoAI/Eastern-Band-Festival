using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

using BandParty.Main;

//譜面データのフォーマット
//  1       2       3       4       5           6
//  index   lane    type    time    chainBehind chainForward

//Configファイルのフォーマット
//  1       2       3       4       5       6       7           8           9
//  Bad_A   Bad_B   Good_A  Good_B  Great_A Great_B Perfect_A   Perfect_B   regulation

namespace BandParty.TimingAdjuster
{
    public class FileManager : MonoBehaviour
    {
        public Main.SoundManager soundManager = null;
        public Main.NotesManager notesManager = null;
        public Judge judge = null;
        public NotesTouchManager notesTouchManager = null;

        public string musicFileName = "夢追いfighter";
        public string extention = ".wav";
        public Common.DIFFICULTY difficulty = Common.DIFFICULTY.EXPERT;

        private string scorePath;
        private string musicPath;
        private string optionPath;
        
        // Use this for initialization
        void Start()
        {
            this.scorePath = Application.persistentDataPath + "/scores/";
            this.musicPath = Application.persistentDataPath + "/music/";
            this.optionPath = Application.persistentDataPath + "/options/";
        }

        public void Load()
        {
            Debug.Log(this.name + ": Load()");

            soundManager.Load(this.musicPath + this.musicFileName + this.extention);

            if (!File.Exists(this.scorePath + "/" + this.musicFileName + "/" + this.musicFileName + "-" + Common.DifficultyToString(difficulty) + ".csv"))
            {
                Debug.Log(this.name + ": NOCSVFILE");
                notesManager.notes = new List<NoteData>();
                return;
            }

            try
            {
                FileStream fs = new FileStream(this.scorePath + "/" + this.musicFileName + "/" + this.musicFileName + "-" + Common.DifficultyToString(difficulty) + ".csv", FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fs);

                List<NoteData> notes = new List<NoteData>();
                int maxIndex = 0;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    notes.Add(new NoteData(Convert.ToInt32(values[0]),
                        Convert.ToByte(values[1]),
                        Convert.ToByte(values[2]),
                        Convert.ToSingle(values[3]),
                        Convert.ToInt32(values[4]),
                        Convert.ToInt32(values[5])
                        ));

                    if (maxIndex < Convert.ToInt32(values[0])) maxIndex = Convert.ToInt32(values[0]);
                }

                if (!File.Exists(this.optionPath + "config.csv"))
                {
                    Debug.Log(this.name + ": NOCONFIGFILE");
                    return;
                }

                notesManager.notes = notes;
                notesManager.backUp = new List<NoteData>(notes);
                
                reader.Close();
                fs.Close();

                fs = new FileStream(this.optionPath + "config.csv", FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fs);

                List<string> configValues = new List<string>();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    for (int i = 0; i < values.Length; i++)
                    {
                        configValues.Add(values[i]);
                    }
                }

                judge.badRangeAfter = Convert.ToSingle(configValues[(int)Common.CONFIG.BAD_A]);
                judge.badRangeBefore = Convert.ToSingle(configValues[(int)Common.CONFIG.BAD_B]);
                judge.goodRangeAfter = Convert.ToSingle(configValues[(int)Common.CONFIG.GOOD_A]);
                judge.goodRangeBefore = Convert.ToSingle(configValues[(int)Common.CONFIG.GOOD_B]);
                judge.greatRangeAfter = Convert.ToSingle(configValues[(int)Common.CONFIG.GREAT_A]);
                judge.greatRangeBefore = Convert.ToSingle(configValues[(int)Common.CONFIG.GREAT_B]);
                judge.perfectRangeAfter = Convert.ToSingle(configValues[(int)Common.CONFIG.PERFECT_A]);
                judge.perfectRangeBefore = Convert.ToSingle(configValues[(int)Common.CONFIG.PERFECT_B]);
                judge.regulation = Convert.ToSingle(configValues[(int)Common.CONFIG.TOUCHREGULATION]);
                notesTouchManager.flickBorderDistance = Convert.ToSingle(configValues[(int)Common.CONFIG.FLICKDISTANCE]);

                reader.Close();
                fs.Close();
            }
            catch (System.Exception)
            {

            }
        }

        public void SaveConfig()
        {
            if (!Directory.Exists(this.optionPath))
            {
                Directory.CreateDirectory(this.optionPath);
            }

            List<string> values = new List<string>();
            values.Add(this.judge.badRangeAfter.ToString());
            values.Add(this.judge.badRangeBefore.ToString());
            values.Add(this.judge.goodRangeAfter.ToString());
            values.Add(this.judge.goodRangeBefore.ToString());
            values.Add(this.judge.greatRangeAfter.ToString());
            values.Add(this.judge.greatRangeBefore.ToString());
            values.Add(this.judge.perfectRangeAfter.ToString());
            values.Add(this.judge.perfectRangeBefore.ToString());
            values.Add(this.judge.regulation.ToString());
            values.Add(this.notesTouchManager.flickBorderDistance.ToString());

            System.Text.StringBuilder line = new System.Text.StringBuilder();
            foreach(string str in values)
            {
                line.Append(str + ",");
            }
            line.Remove(line.Length - 1, 1);

            try
            {
                FileStream fs = new FileStream(this.optionPath + "config.csv", FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);

                writer.WriteLine(line.ToString());

                writer.Close();
                fs.Close();
            }
            catch (System.Exception e)
            {
                Debug.Log(this.name + ": " + e.ToString());
                return;
            }
        }
    }
}
