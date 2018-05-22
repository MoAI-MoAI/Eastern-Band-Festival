using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using BandParty.TimingGetter;

namespace BandParty.TimingGetter
{
    public class GameManager : MonoBehaviour
    {
        public void ExitScene()
        {
            SceneManager.LoadScene("scenes/selectSong");
            //SceneManager.LoadScene("scenes/main");
        }
    }
}
