using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using BandParty.Editor;

namespace BandParty.Editor
{
    [DefaultExecutionOrder(-1)]
    public class MusicFileManager : MonoBehaviour
    {

        public SoundManager soundManager = null;
        public MockToast toast = null;
        public SaveDataIO saveDataIO = null;
        public PlayRecManager playRecManager = null;
        public GameObject fileView = null;
        public GameObject musicFilePrefab = null;

        private GameObject container;
        private string musicFilePath;
        private string scoreFilePath;
        private List<MusicFile> musicFiles;
        private int selectIndex = 0;

        // Use this for initialization
        void Start()
        {
            fileView.SetActive(false);
            container = fileView.transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;

            musicFiles = new List<MusicFile>();
            musicFilePath = UnityEngine.Application.persistentDataPath + "/music";
            scoreFilePath = UnityEngine.Application.persistentDataPath + "/scores";
            Load();
        }

        public void ViewOpen()
        {
            if (fileView.activeSelf) return;

            Load();

            fileView.SetActive(true);

            RectTransform containerTransform = this.container.GetComponent<RectTransform>();
            containerTransform.sizeDelta = new Vector2(containerTransform.sizeDelta.x, 5f + 102f * musicFiles.Count);

            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < musicFiles.Count; i++)
            {
                MusicFile data = musicFiles[i];

                GameObject panel = Instantiate(this.musicFilePrefab).gameObject;
                Transform transform = panel.transform;
                RectTransform rect = panel.GetComponent<RectTransform>();
                MusicFilePanel musicFilePanel = panel.GetComponent<MusicFilePanel>();

                transform.SetParent(this.container.transform);
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -55f + -102f * i);
                rect.offsetMin = new Vector2(3f, rect.offsetMin.y);
                rect.offsetMax = new Vector2(-3f, rect.offsetMax.y);
                rect.localScale = new Vector3(1f, 1f, 1f);
                musicFilePanel.Launch(data, this);
            }
        }

        public void ViewClose()
        {
            fileView.SetActive(false);
        }

        public void OpenFile(string filePath)
        {
            this.playRecManager.ResetStates();
            this.soundManager.Pause();

            this.saveDataIO.Save();

            Debug.Log(this.name + ": filePath=" + filePath);
            for (int i = 0; i < this.musicFiles.Count; i++)
            {
                if (this.musicFiles[i].filePath == filePath)
                {
                    Debug.Log(this.name + ": equals(selectIndex=" + i.ToString() + ")");
                    this.selectIndex = i;
                    break;
                }
            }

            this.soundManager.LoadNewMusic();

            this.ViewClose();
        }

        private void Load()
        {
            musicFiles.Clear();

            try
            {
                if (!Directory.Exists(musicFilePath))
                {
                    //Debug.Log("MusicFileManager: NoDirectory(" + musicFilePath + ")");
                    Directory.CreateDirectory(musicFilePath);
                    throw new NoMusicFileException();
                }

                string[] wavFiles = Directory.GetFiles(musicFilePath, "*.wav");
                string[] oggFiles = Directory.GetFiles(musicFilePath, "*.ogg");
                for (int i = 0; i < wavFiles.Length; i++)
                {
                    MusicFile data = new MusicFile();
                    data.filePath = wavFiles[i];
                    bool[] exists = { false, false, false, false };

                    Debug.Log("MusicFileManager: " + scoreFilePath + "/" + GetRawFileName(wavFiles[i]) + "/" + GetRawFileName(wavFiles[i]) + "-EASY.csv");
                    if (Directory.Exists(scoreFilePath + "/" + GetRawFileName(wavFiles[i])))
                    {
                        if (File.Exists(scoreFilePath + "/" + GetRawFileName(wavFiles[i]) + "/" + GetRawFileName(wavFiles[i]) + "-EASY.csv")) exists[0] = true;
                        if (File.Exists(scoreFilePath + "/" + GetRawFileName(wavFiles[i]) + "/" + GetRawFileName(wavFiles[i]) + "-NORMAL.csv")) exists[1] = true;
                        if (File.Exists(scoreFilePath + "/" + GetRawFileName(wavFiles[i]) + "/" + GetRawFileName(wavFiles[i]) + "-HARD.csv")) exists[2] = true;
                        if (File.Exists(scoreFilePath + "/" + GetRawFileName(wavFiles[i]) + "/" + GetRawFileName(wavFiles[i]) + "-EXPERT.csv")) exists[3] = true;
                    }

                    data.difficultyExists = exists;

                    musicFiles.Add(data);
                }
                for (int i = 0; i < oggFiles.Length; i++)
                {
                    MusicFile data = new MusicFile();
                    data.filePath = oggFiles[i];
                    bool[] exists = { false, false, false, false };

                    if (Directory.Exists(scoreFilePath))
                    {
                        if (File.Exists(scoreFilePath + "/" + wavFiles[i] + "-EASY.csv")) exists[0] = true;
                        if (File.Exists(scoreFilePath + "/" + wavFiles[i] + "-NORMAL.csv")) exists[1] = true;
                        if (File.Exists(scoreFilePath + "/" + wavFiles[i] + "-HARD.csv")) exists[2] = true;
                        if (File.Exists(scoreFilePath + "/" + wavFiles[i] + "-EXPERT.csv")) exists[3] = true;
                    }

                    data.difficultyExists = exists;

                    musicFiles.Add(data);
                }
                if (musicFiles.Count == 0) throw new NoMusicFileException();
            }
            catch (NoMusicFileException)
            {
                toast.Launch("音声データがありません。\n" + musicFilePath + " に手動で追加してください。");
            }
            catch (System.Exception e)
            {
                toast.Launch("Error: " + e.ToString());
            }
        }

        public string GetPath()
        {
            if (this.selectIndex >= this.musicFiles.Count) return "NODATA";
            else return this.musicFiles[this.selectIndex].filePath;
        }

        public string GetFileName()
        {
            if (this.selectIndex >= this.musicFiles.Count) return "NODATA";
            else return GetRawFileName(this.musicFiles[this.selectIndex].filePath);
        }

        public float GetBPM()
        {
            if (this.selectIndex >= this.musicFiles.Count) return 60f;
            else
            {
                string fileName = this.musicFiles[this.selectIndex].filePath;
                int index = fileName.LastIndexOf("BPM=") + 4;
                int length = fileName.LastIndexOf(')') - (fileName.LastIndexOf("BPM=") + 4);
                string bpmText = fileName.Substring(index, length);
                float bpm;
                float.TryParse(bpmText, out bpm);
                if (bpm == 0) return 60f;
                else return bpm;
            }
        }

        private string GetRawFileName(string fileName)
        {
            return fileName.Substring(Mathf.Max(fileName.LastIndexOf('/'), fileName.LastIndexOf('\\')) + 1, fileName.LastIndexOf('.') - (Mathf.Max(fileName.LastIndexOf('/'), fileName.LastIndexOf('\\')) + 1));
        }

        public struct MusicFile
        {
            public string filePath;
            public bool[] difficultyExists;
        }

        private class NoMusicFileException : System.Exception
        {
            public NoMusicFileException()
            {

            }

            public NoMusicFileException(string message) : base(message)
            {

            }

            public NoMusicFileException(string message, System.Exception inner) : base(message, inner)
            {

            }
        }
    }
}
