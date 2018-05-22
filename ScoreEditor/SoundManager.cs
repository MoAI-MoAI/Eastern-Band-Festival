using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BandParty.Editor;

namespace BandParty.Editor
{
    public class SoundManager : MonoBehaviour
    {

        public float beforeTime = -6.5f;

        public MusicFileManager musicFileManager = null;
        public PlayRecManager playRecManager = null;
        public AudioClip clip = null;
        public float BPM = 60f;

        private float time;
        private AudioSource audioSource = null;
        private bool isPlaying = false;
        private bool isYetPlayed = false;

        private IEnumerator coroutineMethod = null;

        // Use this for initialization
        void Start()
        {
            this.audioSource = gameObject.GetComponent<AudioSource>();
            //this.audioSource.clip = this.clip;

            StartCoroutine("LoadFile");

            this.coroutineMethod = Checking(() =>
            {
                StopCoroutine(this.coroutineMethod);
                this.time = this.audioSource.clip.length;
                this.isPlaying = false;
                this.isYetPlayed = false;
                this.playRecManager.MusicEnded();
            });

            this.time = this.beforeTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (this.isPlaying)
            {
                if (!this.isYetPlayed)
                {
                    this.time += Time.deltaTime;

                    if (time >= 0.0f)
                    {
                        this.audioSource.time = this.time;
                        this.audioSource.Play();
                        this.isYetPlayed = true;

                        this.coroutineMethod = Checking(() =>
                        {
                            StopCoroutine(this.coroutineMethod);
                            this.time = this.audioSource.clip.length;
                            this.isPlaying = false;
                            this.isYetPlayed = false;
                            this.playRecManager.MusicEnded();
                        });
                        StartCoroutine(this.coroutineMethod);
                    }
                }
            }
        }

        public void Play()
        {
            if (this.isYetPlayed) return;

            if (this.time == this.audioSource.clip.length) this.time = beforeTime;

            this.isPlaying = true;
            this.isYetPlayed = false;
        }

        public void Pause()
        {
            if (this.time != this.clip.length && this.time >= 0.0f) this.time = this.audioSource.time;
            StopCoroutine(coroutineMethod);
            this.audioSource.Stop();

            this.isPlaying = false;
            this.isYetPlayed = false;
        }

        public void AddTime(float addTime)
        {
            this.time += addTime;
            if (this.time < 0.0f) this.time = 0.0f;
            else if (this.time >= this.clip.length)
            {
                this.time = this.clip.length;

                StopCoroutine(coroutineMethod);
                this.audioSource.Stop();

                this.isPlaying = false;
                this.isYetPlayed = false;
                this.playRecManager.MusicEnded();
                return;
            }

            this.audioSource.time = this.time;
        }

        public void AddBeat(float addBeat)
        {
            this.time += 60f / this.BPM * addBeat;
            if (this.time >= this.clip.length)
            {
                this.time = this.clip.length;

                StopCoroutine(coroutineMethod);
                this.audioSource.Stop();

                this.isPlaying = false;
                this.isYetPlayed = false;
                this.playRecManager.MusicEnded();
                return;
            }

            this.audioSource.time = this.time;
        }

        public void AddFlame(int flame)
        {
            this.time += Time.deltaTime * flame;
            if (this.time >= this.clip.length)
            {
                this.time = this.clip.length;

                StopCoroutine(coroutineMethod);
                this.audioSource.Stop();

                this.isPlaying = false;
                this.isYetPlayed = false;
                this.playRecManager.MusicEnded();
                return;
            }

            this.audioSource.time = this.time;
        }

        public void BackToStart()
        {
            this.time = beforeTime;

            StopCoroutine(coroutineMethod);
            this.audioSource.Stop();

            this.isYetPlayed = false;
            return;
        }

        public void LoadNewMusic()
        {
            StartCoroutine("LoadFile");
        }

        public void SetTime(float time)
        {
            this.time = time;
            if (this.time == this.audioSource.clip.length)
            {
                this.time = this.clip.length;

                StopCoroutine(coroutineMethod);
                this.audioSource.Stop();

                this.isPlaying = false;
                this.isYetPlayed = false;
                this.playRecManager.MusicEnded();
            }
            else
            {
                this.audioSource.time = time;
            }
        }

        public float GetTime()
        {
            return this.time;
        }

        public float GetLength()
        {
            if (this.clip != null) return this.clip.length;
            else return 0f;
        }

        public bool IsPlaying()
        {
            return this.isPlaying;
        }

        public delegate void functionType();
        private IEnumerator Checking(functionType callback)
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                this.time = this.audioSource.time;
                if (!this.audioSource.isPlaying)
                {
                    callback();
                    break;
                }
            }
        }

        private IEnumerator LoadFile()
        {
            Debug.Log(this.name + ": LoadFile()");
            string path = musicFileManager.GetPath();
            //path = System.Uri.EscapeUriString(path);
            Debug.Log(this.name + ": " + "file:///" + path);
            WWW www = new WWW("file:///" + path);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("SoundManager: " + www.error);
            }

            this.clip = www.GetAudioClip(true, false);
            //Debug.Log(this.name + ": " + this.clip.loadState.ToString());
            //Debug.Log(this.name + ": clip.name=" + this.clip.name + "\n clip.length=" + this.clip.length.ToString()
            //    + "\n clip.samples=" + this.clip.samples.ToString());
            this.audioSource.clip = clip;
            this.BPM = musicFileManager.GetBPM();
        }
    }
}