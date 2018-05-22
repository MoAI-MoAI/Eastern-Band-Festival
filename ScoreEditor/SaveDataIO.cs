using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//譜面データのフォーマット
//  1       2       3       4       5           6
//  index   lane    type    time    chainBehind chainForward

using BandParty.Editor;

namespace BandParty.Editor
{
    [DefaultExecutionOrder(-1)]
    public class SaveDataIO : MonoBehaviour
    {

        public MusicFileManager musicFileManager = null;
        public NotesManager notesManager = null;
        public DifficultyManager difficultyManager = null;
        public MockToast toast = null;

        [SerializeField]
        private string dataDir;

        // Use this for initialization
        void Start()
        {
            dataDir = UnityEngine.Application.persistentDataPath + "/scores";
        }

        public void Save()
        {
            if (!Directory.Exists(this.dataDir + "/" + this.musicFileManager.GetFileName()))
            {
                Directory.CreateDirectory(this.dataDir + "/" + this.musicFileManager.GetFileName());
            }

            try
            {
                //Encoding utf8Enc = new UTF8Encoding(true);
                FileStream fs = new FileStream(this.dataDir + "/" + this.musicFileManager.GetFileName() + "/" + this.musicFileManager.GetFileName() + "-" + difficultyManager.GetValue() + ".csv", FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);

                List<NoteData> notes = this.notesManager.GetNotesList();
                for (int i = 0; i < notes.Count; i++)
                {
                    NoteData note = notes[i];
                    writer.WriteLine(note.index.ToString() + ","
                        + note.lane.ToString() + ","
                        + note.type.ToString() + ","
                        + note.time.ToString() + ","
                        + note.chainBehind + ","
                        + note.chainForward);
                }

                writer.Close();
                fs.Close();
            }
            catch (System.Exception e)
            {
                toast.Launch("Save Error: " + e.ToString());
                return;
            }

            toast.Launch("Saved (" + this.dataDir + "/" + this.musicFileManager.GetFileName() + "/" + this.musicFileManager.GetFileName() + "-" + difficultyManager.GetValue() + ".csv)");
        }

        public void Save(String difficulty)
        {
            if (!Directory.Exists(this.dataDir + "/" + this.musicFileManager.GetFileName()))
            {
                Directory.CreateDirectory(this.dataDir + "/" + this.musicFileManager.GetFileName());
            }

            try
            {
                //Encoding utf8Enc = new UTF8Encoding(true);
                FileStream fs = new FileStream(this.dataDir + "/" + this.musicFileManager.GetFileName() + "/" + this.musicFileManager.GetFileName() + "-" + difficulty + ".csv", FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);

                List<NoteData> notes = this.notesManager.GetNotesList();
                for (int i = 0; i < notes.Count; i++)
                {
                    NoteData note = notes[i];
                    writer.WriteLine(note.index.ToString() + ","
                        + note.lane.ToString() + ","
                        + note.type.ToString() + ","
                        + note.time.ToString() + ","
                        + note.chainBehind + ","
                        + note.chainForward);
                }

                writer.Close();
                fs.Close();
            }
            catch (System.Exception)
            {

            }

            toast.Launch("Saved (" + this.dataDir + "/" + this.musicFileManager.GetFileName() + "/" + this.musicFileManager.GetFileName() + "-" + difficulty + ".csv)");
        }

        public void Load(String difficulty)
        {
            Debug.Log("SaveDataIO: Load() path=" + this.dataDir + "/" + this.musicFileManager.GetFileName() + "/" + this.musicFileManager.GetFileName() + "-" + difficulty + ".csv");
            if (!File.Exists(this.dataDir + "/" + this.musicFileManager.GetFileName() + "/" + this.musicFileManager.GetFileName() + "-" + difficulty + ".csv"))
            {
                notesManager.SetNotesList(new List<NoteData>());
                return;
            }

            try
            {
                //Encoding utf8Enc = new UTF8Encoding(true);
                FileStream fs = new FileStream(this.dataDir + "/" + this.musicFileManager.GetFileName() + "/" + this.musicFileManager.GetFileName() + "-" + difficultyManager.GetValue() + ".csv", FileMode.Open, FileAccess.Read);
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

                notesManager.SetNotesList(notes);
                notesManager.SetIndex(maxIndex + 1);

                reader.Close();
                fs.Close();
            }
            catch (System.Exception)
            {

            }
        }
    }
}