using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using BandParty.TimingGetter;

//Configファイルのフォーマット
//  1       2       3       4       5       6       7           8           9           10
//  Bad_A   Bad_B   Good_A  Good_B  Great_A Great_B Perfect_A   Perfect_B   regulation  flickDist

namespace BandParty.TimingGetter
{
    public class RegulationsManager : MonoBehaviour
    {
        public Text regulation1;
        public Text regulation2;
        public Text regulation3;
        public Text regulation4;
        public Text regulationAverage;
        public List<float> regulations;

        private int prevCount;
        private string optionPath;
        private string[] values;

        // Use this for initialization
        void Start()
        {
            this.optionPath = Application.persistentDataPath + "/options/";

            this.regulations = new List<float>();
            this.regulations.Add(0f);
            this.regulations.Add(0f);
            this.regulations.Add(0f);
            this.regulations.Add(0f);

            LoadFile();

            this.regulation1.text = this.regulations[this.regulations.Count - 4].ToString();
            this.regulation2.text = this.regulations[this.regulations.Count - 3].ToString();
            this.regulation3.text = this.regulations[this.regulations.Count - 2].ToString();
            this.regulation4.text = this.regulations[this.regulations.Count - 1].ToString();
            this.regulationAverage.text = ((this.regulations[this.regulations.Count - 4]
                + this.regulations[this.regulations.Count - 3]
                + this.regulations[this.regulations.Count - 2]
                + this.regulations[this.regulations.Count - 1]) / 4).ToString();

            this.prevCount = this.regulations.Count;
        }

        // Update is called once per frame
        void Update()
        {
            if (this.prevCount == this.regulations.Count) return;

            this.regulation1.text = this.regulations[this.regulations.Count - 4].ToString();
            this.regulation2.text = this.regulations[this.regulations.Count - 3].ToString();
            this.regulation3.text = this.regulations[this.regulations.Count - 2].ToString();
            this.regulation4.text = this.regulations[this.regulations.Count - 1].ToString();
            this.regulationAverage.text = ((this.regulations[this.regulations.Count - 4]
                + this.regulations[this.regulations.Count - 3]
                + this.regulations[this.regulations.Count - 2]
                + this.regulations[this.regulations.Count - 1]) / 4).ToString();

            this.prevCount = this.regulations.Count;
        }

        private void LoadFile()
        {
            try
            {
                if (!File.Exists(this.optionPath + "config.csv")) return;

                FileStream fs = new FileStream(this.optionPath + "config.csv", FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fs);

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    this.values = line.Split(',');

                    this.regulations.Add(Convert.ToSingle(this.values[8]));
                    this.regulations.Add(Convert.ToSingle(this.values[8]));
                    this.regulations.Add(Convert.ToSingle(this.values[8]));
                    this.regulations.Add(Convert.ToSingle(this.values[8]));
                }

                reader.Close();
                fs.Close();
            } catch (System.Exception e)
            {
                Debug.Log(this.name + ": " + e.ToString());
            }
        }

        public void SaveRegulation()
        {
            if (!Directory.Exists(this.optionPath))
            {
                Directory.CreateDirectory(this.optionPath);
            }

            try
            {
                FileStream fs = new FileStream(this.optionPath + "config.csv", FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);

                this.values[8] = ((this.regulations[this.regulations.Count - 4]
                + this.regulations[this.regulations.Count - 3]
                + this.regulations[this.regulations.Count - 2]
                + this.regulations[this.regulations.Count - 1]) / 4).ToString();

                writer.WriteLine(this.values[0] + ","
                    + this.values[1] + ","
                    + this.values[2] + ","
                    + this.values[3] + ","
                    + this.values[4] + ","
                    + this.values[5] + ","
                    + this.values[6] + ","
                    + this.values[7] + ","
                    + this.values[8] + ","
                    + this.values[9]);

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

