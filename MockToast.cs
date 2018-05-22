using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BandParty
{
    [DefaultExecutionOrder(-2)]
    public class MockToast : MonoBehaviour
    {

        public Text text = null;
        public float showTime = 6f;
        public float fadeTime = 2f;

        private Image background = null;
        private float time;
        private bool isShowing = false;
        private float backgroundDefaultAlpha = 1.0f;

        private void Start()
        {
            this.background = this.GetComponent<Image>();
            backgroundDefaultAlpha = background.color.a;

            background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
            isShowing = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isShowing) return;

            this.time += Time.deltaTime;

            if (this.time >= this.showTime + this.fadeTime)
            {
                background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
                isShowing = false;
            }
            else if (this.time >= this.showTime)
            {
                float elapsedTime = this.time - this.showTime;
                background.color = new Color(background.color.r, background.color.g, background.color.b, backgroundDefaultAlpha - backgroundDefaultAlpha * (elapsedTime / this.fadeTime));
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1f - (elapsedTime / this.fadeTime));
            }
        }

        public void Launch(string text)
        {
            this.text.text = text;
            this.time = 0f;
            this.isShowing = true;

            this.background.color = new Color(this.background.color.r, this.background.color.g, this.background.color.b, this.backgroundDefaultAlpha);
            this.text.color = new Color(this.text.color.r, this.text.color.g, this.text.color.b, 1f);
        }
    }
}
