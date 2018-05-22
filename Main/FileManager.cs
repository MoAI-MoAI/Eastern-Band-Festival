using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

//譜面データのフォーマット
//  1       2       3       4       5           6
//  index   lane    type    time    chainBehind chainForward

//Configファイルのフォーマット
//  1       2       3       4       5       6       7           8           9
//  Bad_A   Bad_B   Good_A  Good_B  Great_A Great_B Perfect_A   Perfect_B   regulation

namespace BandParty.Main
{
    public class FileManager : MonoBehaviour
    {
        public SoundManager soundManager = null;
        public NotesManager notesManager = null;
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

            //SongDataSender.songData.songName = this.musicFileName;
            //SongDataSender.difficulty = this.difficulty;
            
            this.musicFileName = SongDataSender.songData.songName;
            this.difficulty = SongDataSender.difficulty;
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
                
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    notes.Add(new NoteData(Convert.ToInt32(values[(int)Common.SCOREDATA.INDEX]),
                        Convert.ToByte(values[(int)Common.SCOREDATA.LANE]),
                        Convert.ToByte(values[(int)Common.SCOREDATA.TYPE]),
                        Convert.ToSingle(values[(int)Common.SCOREDATA.TIME]),
                        Convert.ToInt32(values[(int)Common.SCOREDATA.CHAINBEHIND]),
                        Convert.ToInt32(values[(int)Common.SCOREDATA.CHAINFORWARD])
                        ));
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
    }
}
