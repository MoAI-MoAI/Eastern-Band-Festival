using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty.Main
{
    public class EffectManager : MonoBehaviour
    {
        public GameObject effectsParent;
        public GameObject effectPrefab;
        
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GenerateEffect(Common.NOTE type, byte lane)
        {
            GameObject effect = Instantiate(this.effectPrefab).gameObject;
            effect.transform.SetParent(this.effectsParent.transform);

            RectTransform rect = effect.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(Common.touchPointLaneWidth * (lane - 3), 0f);

            Animator animator = effect.GetComponent<Animator>();
            switch (type) {
                case Common.NOTE.NONE:
                    animator.Play(Animator.StringToHash("Hit 1"));
                    break;
                case Common.NOTE.SINGLE:
                    animator.Play(Animator.StringToHash("Hit 2"));
                    break;
                case Common.NOTE.SLIDE_PRESS:
                case Common.NOTE.SLIDE_VIA:
                case Common.NOTE.SLIDE_RELEASE:
                    animator.Play(Animator.StringToHash("SlideHit"));
                    break;
                case Common.NOTE.FLICK:
                    animator.Play(Animator.StringToHash("Flick"));
                    break;
            }
        }

        public GameObject GenerateSlideEffect(byte lane)
        {
            GameObject effect = Instantiate(this.effectPrefab).gameObject;
            effect.transform.SetParent(this.effectsParent.transform);

            RectTransform rect = effect.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(Common.touchPointLaneWidth * (lane - 3), 0f);

            Animator animator = effect.GetComponent<Animator>();
            animator.Play(Animator.StringToHash("Slide"));

            return effect;
        }
    }
}