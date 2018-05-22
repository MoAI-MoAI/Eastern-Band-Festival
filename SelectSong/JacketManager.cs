using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.SelectSong
{
    public class JacketManager : MonoBehaviour
    {
        private struct Jacket
        {
            public int index;
            public GameObject jacket;
            public Common.SongData songData;
            public bool isEmpty;
            public Animator animator;
        }

        public GameObject jacketsParent = null;
        public GameObject jacketPrefab = null;
        public Texture2D emptyImage = null;
        public SEManager seManager = null;

        private List<Common.SongData> songDatas;
        private List<Jacket> jackets;
        private int select = 0;
        private bool isMoving = false;

        private string optionsPath;
        private string jacketsPath;

        // Use this for initialization
        void Start()
        {
            this.optionsPath = Application.persistentDataPath + "/options/";
            this.jacketsPath = Application.persistentDataPath + "/jackets/";

            this.songDatas = new List<Common.SongData>();
            this.jackets = new List<Jacket>();
            this.select = 0;

            LoadSongData();
            GenerateJackets();

            StartCoroutine("CheckAnims");
        }

        public void Stop()
        {
            StopCoroutine("CheckAnims");
        }

        public void ReStart()
        {
            foreach (Transform child in this.jacketsParent.transform)
            {
                Destroy(child.gameObject);
            }
            this.jackets = new List<Jacket>();

            GenerateJackets();

            StartCoroutine("CheckAnims");
        }

        public void Flicked(bool isLeft)
        {
            if (this.isMoving) return;
            Debug.Log(this.name + ": flicked " + isLeft.ToString());
            if (isLeft)
            {
                if(this.select <= this.songDatas.Count - 2)
                {
                    this.select++;
                    this.seManager.Play(3);

                    foreach (Jacket jacket in this.jackets)
                    {
                        Animator animator = jacket.animator;
                        if (jacket.index == this.select - 2)
                        {
                            animator.Play(Animator.StringToHash("Left-Out"));
                        }
                        else if (jacket.index == this.select - 1)
                        {
                            animator.Play(Animator.StringToHash("Center-Left"));
                        }
                        else if (jacket.index == this.select)
                        {
                            animator.Play(Animator.StringToHash("Right-Center"));
                        }
                    }

                    if (this.songDatas.Count <= this.select + 1)
                    {
                        GameObject prefab = Instantiate(this.jacketPrefab).gameObject;
                        RawImage rawImage = prefab.transform.Find("Jacket").GetComponent<RawImage>();
                        rawImage.texture = this.emptyImage;
                        rawImage = prefab.transform.Find("Shadow").GetComponent<RawImage>();
                        rawImage.texture = this.emptyImage;

                        prefab.transform.SetParent(this.jacketsParent.transform);
                        prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(900f, 0f);
                        prefab.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 1f);

                        Jacket jacket = new Jacket();
                        jacket.index = this.select + 1;
                        jacket.jacket = prefab;
                        jacket.isEmpty = true;
                        jacket.animator = prefab.GetComponent<Animator>();
                        this.jackets.Add(jacket);

                        jacket.animator.Play(Animator.StringToHash("Out-Right"));
                    }
                    else
                    {
                        foreach (Common.SongData songData in this.songDatas)
                        {
                            if (songData.index == this.select + 1)
                            {
                                GameObject prefab = Instantiate(this.jacketPrefab).gameObject;
                                RawImage rawImage = prefab.transform.Find("Jacket").GetComponent<RawImage>();
                                rawImage.texture = songData.jacketImage;
                                rawImage = prefab.transform.Find("Shadow").GetComponent<RawImage>();
                                rawImage.texture = songData.jacketImage;

                                prefab.transform.SetParent(this.jacketsParent.transform);
                                prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(900f, 0f);
                                prefab.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 1f);

                                Jacket jacket = new Jacket();
                                jacket.index = songData.index;
                                jacket.jacket = prefab;
                                jacket.isEmpty = false;
                                jacket.animator = prefab.GetComponent<Animator>();
                                this.jackets.Add(jacket);

                                jacket.animator.Play(Animator.StringToHash("Out-Right"));

                                break;
                            }
                        }
                    }
                }
            } else
            {
                if (this.select >= 1)
                {
                    this.select--;
                    this.seManager.Play(3);

                    foreach (Jacket jacket in this.jackets)
                    {
                        Animator animator = jacket.animator;
                        if (jacket.index == this.select)
                        {
                            animator.Play(Animator.StringToHash("Left-Center"));
                        }
                        else if (jacket.index == this.select + 1)
                        {
                            animator.Play(Animator.StringToHash("Center-Right"));
                        }
                        else if (jacket.index == this.select + 2)
                        {
                            animator.Play(Animator.StringToHash("Right-Out"));
                        }
                    }

                    if (0 > this.select - 1)
                    {
                        GameObject prefab = Instantiate(this.jacketPrefab).gameObject;
                        RawImage rawImage = prefab.transform.Find("Jacket").GetComponent<RawImage>();
                        rawImage.texture = this.emptyImage;
                        rawImage = prefab.transform.Find("Shadow").GetComponent<RawImage>();
                        rawImage.texture = this.emptyImage;

                        prefab.transform.SetParent(this.jacketsParent.transform);
                        prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(-900f, 0f);
                        prefab.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 1);

                        Jacket jacket = new Jacket();
                        jacket.index = this.select - 1;
                        jacket.jacket = prefab;
                        jacket.isEmpty = true;
                        jacket.animator = prefab.GetComponent<Animator>();
                        this.jackets.Add(jacket);

                        jacket.animator.Play(Animator.StringToHash("Out-Left"));
                    }
                    else
                    {
                        foreach (Common.SongData songData in this.songDatas)
                        {
                            if (songData.index == this.select - 1)
                            {
                                GameObject prefab = Instantiate(this.jacketPrefab).gameObject;
                                RawImage rawImage = prefab.transform.Find("Jacket").GetComponent<RawImage>();
                                rawImage.texture = songData.jacketImage;
                                rawImage = prefab.transform.Find("Shadow").GetComponent<RawImage>();
                                rawImage.texture = songData.jacketImage;

                                prefab.transform.SetParent(this.jacketsParent.transform);
                                prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(-900f, 0f);
                                prefab.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 1);

                                Jacket jacket = new Jacket();
                                jacket.index = this.select - 1;
                                jacket.jacket = prefab;
                                jacket.isEmpty = false;
                                jacket.animator = prefab.GetComponent<Animator>();
                                this.jackets.Add(jacket);

                                jacket.animator.Play(Animator.StringToHash("Out-Left"));

                                break;
                            }
                        }
                    }
                }
            }
        }

        private void LoadSongData()
        {
            if (!File.Exists(this.optionsPath + "songdata.csv"))
            {
                Debug.Log(this.name + ": NOSONGDATAFILE");
                return;
            }

            List<Common.SongData> tempDatas = new List<Common.SongData>();

            try
            {
                FileStream fs = new FileStream(this.optionsPath + "songdata.csv", FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fs);

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    Common.SongData temp = new Common.SongData();
                    temp.index = Convert.ToInt32(values[0]);
                    temp.songName = values[1];
                    tempDatas.Add(temp);
                }

                reader.Close();
                fs.Close();
            } catch (System.Exception e)
            {
                Debug.Log(this.name + ": " + e.ToString());
            }

            byte[] binJacket = null;

            foreach (Common.SongData temp in tempDatas)
            {
                if (!File.Exists(this.jacketsPath + temp.songName + ".png"))
                {
                    Debug.Log(this.name + ": NOJACKETIMAGE " + temp.songName);
                }

                try
                {
                    FileStream fs = new FileStream(this.jacketsPath + temp.songName + ".png", FileMode.Open, FileAccess.Read);
                    BinaryReader bin = new BinaryReader(fs);
                    binJacket = bin.ReadBytes((int)bin.BaseStream.Length);
                    bin.Close();
                    fs.Close();
                }
                catch (System.Exception e)
                {
                    Debug.Log(this.name + ": " + e.ToString());
                }

                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(binJacket);
                Common.SongData data = temp;
                data.jacketImage = texture;
                this.songDatas.Add(data);
            }
        }

        private void GenerateJackets()
        {
            //center
            GameObject prefab = Instantiate(this.jacketPrefab).gameObject;
            RawImage rawImage = prefab.transform.Find("Jacket").GetComponent<RawImage>();
            rawImage.texture = this.songDatas[this.select].jacketImage;
            rawImage = prefab.transform.Find("Shadow").GetComponent<RawImage>();
            rawImage.texture = this.songDatas[this.select].jacketImage;

            prefab.transform.SetParent(this.jacketsParent.transform);
            prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            prefab.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            Jacket jacket = new Jacket();
            jacket.index = this.select;
            jacket.jacket = prefab;
            jacket.songData = this.songDatas[this.select];
            jacket.isEmpty = false;
            jacket.animator = prefab.GetComponent<Animator>();
            jacket.animator.Play(Animator.StringToHash("FadeInCenter"));
            this.jackets.Add(jacket);

            //left
            prefab = Instantiate(this.jacketPrefab).gameObject;
            if (0 <= this.select - 1 && this.select - 1 <= this.songDatas.Count - 1)
            {
                rawImage = prefab.transform.Find("Jacket").GetComponent<RawImage>();
                rawImage.texture = this.songDatas[this.select - 1].jacketImage;
                rawImage = prefab.transform.Find("Shadow").GetComponent<RawImage>();
                rawImage.texture = this.songDatas[this.select - 1].jacketImage;

                prefab.transform.SetParent(this.jacketsParent.transform);
                prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(-450f, 0f);
                prefab.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

                jacket = new Jacket();
                jacket.index = this.select - 1;
                jacket.jacket = prefab;
                jacket.songData = this.songDatas[this.select - 1];
                jacket.animator = prefab.GetComponent<Animator>();
                jacket.animator.Play(Animator.StringToHash("FadeInLeft"));
                jacket.isEmpty = false;
            } else
            {
                rawImage = prefab.transform.Find("Jacket").GetComponent<RawImage>();
                rawImage.texture = this.emptyImage;
                rawImage = prefab.transform.Find("Shadow").GetComponent<RawImage>();
                rawImage.texture = this.emptyImage;

                prefab.transform.SetParent(this.jacketsParent.transform);
                prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(-450f, 0f);
                prefab.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

                jacket = new Jacket();
                jacket.index = this.select - 1;
                jacket.jacket = prefab;
                jacket.animator = prefab.GetComponent<Animator>();
                jacket.animator.Play(Animator.StringToHash("FadeInLeft"));
                jacket.isEmpty = true;
            }
            this.jackets.Add(jacket);

            //right
            prefab = Instantiate(this.jacketPrefab).gameObject;
            if (0 <= this.select + 1 && this.select + 1 <= this.songDatas.Count - 1)
            {
                rawImage = prefab.transform.Find("Jacket").GetComponent<RawImage>();
                rawImage.texture = this.songDatas[this.select + 1].jacketImage;
                rawImage = prefab.transform.Find("Shadow").GetComponent<RawImage>();
                rawImage.texture = this.songDatas[this.select + 1].jacketImage;

                prefab.transform.SetParent(this.jacketsParent.transform);
                prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(450f, 0f);
                prefab.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

                jacket = new Jacket();
                jacket.index = this.select + 1;
                jacket.jacket = prefab;
                jacket.songData = this.songDatas[this.select + 1];
                jacket.animator = prefab.GetComponent<Animator>();
                jacket.animator.Play(Animator.StringToHash("FadeInRight"));
                jacket.isEmpty = false;
            }
            else
            {
                rawImage = prefab.transform.Find("Jacket").GetComponent<RawImage>();
                rawImage.texture = this.emptyImage;
                rawImage = prefab.transform.Find("Shadow").GetComponent<RawImage>();
                rawImage.texture = this.emptyImage;

                prefab.transform.SetParent(this.jacketsParent.transform);
                prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(450f, 0f);
                prefab.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

                jacket = new Jacket();
                jacket.index = this.select + 1;
                jacket.jacket = prefab;
                jacket.animator = prefab.GetComponent<Animator>();
                jacket.animator.Play(Animator.StringToHash("FadeInRight"));
                jacket.isEmpty = true;
            }
            this.jackets.Add(jacket);
        }

        public Common.SongData GetSongData()
        {
            return this.songDatas[this.select];
        }

        private IEnumerator CheckAnims()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                this.isMoving = false;

                int deleteJacket = -1;
                for (int i = 0; i < this.jackets.Count; i++)
                {
                    int hash = this.jackets[i].animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
                    if (hash != Animator.StringToHash("Base Layer.Center") && hash != Animator.StringToHash("Base Layer.Right") && hash != Animator.StringToHash("Base Layer.Left") && hash != Animator.StringToHash("Base Layer.Out"))
                    {
                        this.isMoving = true; 
                    }
                    else if (hash == Animator.StringToHash("Base Layer.Out"))
                    {
                        deleteJacket = i;
                    }
                }

                if (deleteJacket != -1)
                {
                    //Debug.Log(this.name + ": Destroy ");
                    Destroy(this.jackets[deleteJacket].jacket);
                    this.jackets.Remove(this.jackets[deleteJacket]);
                }
            }
        }
    }
}