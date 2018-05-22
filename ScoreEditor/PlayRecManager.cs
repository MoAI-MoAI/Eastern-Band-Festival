using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BandParty.Editor
{
    public class PlayRecManager : MonoBehaviour
    {

        public SoundManager soundManager = null;
        public RecordingManager recordingManager = null;
        public PlayOrPauseSwitcher playSwither = null;
        public RecordSwitcher recSwitcher = null;

        private bool isPlayButton_Standby = true;
        private bool isRecButton_Standby = true;

        public void OnPlayBottonTapped()
        {
            if (this.isPlayButton_Standby)
            {
                playSwither.SwitchToPlaying();
                soundManager.Play();

                this.isPlayButton_Standby = false;
            }
            else
            {
                playSwither.SwitchToStandby();
                recSwitcher.SwitchToStandby();
                recordingManager.StopRecording();
                soundManager.Pause();

                this.isPlayButton_Standby = true;
                this.isRecButton_Standby = true;
            }
        }

        public void OnRecBottonTapped()
        {
            if (this.isRecButton_Standby)
            {
                playSwither.SwitchToPlaying();
                recSwitcher.SwitchToRecording();
                recordingManager.StartRecording();
                soundManager.Play();

                this.isPlayButton_Standby = false;
                this.isRecButton_Standby = false;
            }
            else
            {
                recSwitcher.SwitchToStandby();
                recordingManager.StopRecording();

                this.isRecButton_Standby = true;
            }
        }

        public void MusicEnded()
        {
            playSwither.SwitchToStandby();
            recSwitcher.SwitchToStandby();
            soundManager.Pause();

            this.isPlayButton_Standby = true;
            this.isRecButton_Standby = true;
        }

        public void ResetStates()
        {
            playSwither.SwitchToStandby();
            recSwitcher.SwitchToStandby();
            this.isPlayButton_Standby = true;
            this.isRecButton_Standby = true;
        }
    }
}