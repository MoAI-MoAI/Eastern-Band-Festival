using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BandParty.Main;

namespace BandParty.Main
{
    public class SoundManager : MonoBehaviour
    {
        public float beforeTime = -6.5f;
        public float time;
        public NotesManager notesManager = null;
        public GameManager gameManager = null;

        private AudioSource audioSource = null;
        private AudioClip clip = null;
        private string filePath;
        private bool isPlaying = false;
        private bool isYetPlayed = false;

        private IEnumerator coroutineMethod = null;

        // Use this for initialization
        void Start()
        {
            this.audioSource = gameObject.GetComponent<AudioSource>();
            
            this.coroutineMethod = Checking(() => {
                StopCoroutine(this.coroutineMethod);
                this.time = this.audioSource.clip.length;
                this.isPlaying = false;
                this.isYetPlayed = false;

                this.gameManager.MusicEnd();
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

                        this.coroutineMethod = Checking(() => {
                            StopCoroutine(this.coroutineMethod);
                            this.time = this.audioSource.clip.length;
                            this.isPlaying = false;
                            this.isYetPlayed = false;

                            this.gameManager.MusicEnd();
                        });
                        StartCoroutine(this.coroutineMethod);
                    }
                }
            }
        }

        public void Load(string filePath)
        {
            this.filePath = filePath;
            StartCoroutine("LoadFile");
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
            this.isPlaying = false;
            this.time = this.audioSource.time;
            if (this.isYetPlayed) StopCoroutine(this.coroutineMethod);
            this.audioSource.Pause();
        }

        public void ReStart()
        {
            this.isPlaying = true;
            if (this.isYetPlayed) this.audioSource.time = this.time;
            this.audioSource.Play();
            StartCoroutine(this.coroutineMethod);
        }

        public void Replay()
        {
            StopCoroutine(this.coroutineMethod);
            this.audioSource.Stop();
            this.time = this.beforeTime;

            this.isPlaying = true;
            this.isYetPlayed = false;

            this.notesManager.RestoreNotes();
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
            Debug.Log(this.name + ": " + "file:///" + filePath);
            WWW www = new WWW("file:///" + filePath);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("SoundManager: " + www.error);
            }

            this.clip = www.GetAudioClip(true, false);
            this.audioSource.clip = clip;
        }
    }
}
