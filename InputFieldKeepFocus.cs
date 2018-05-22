using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldKeepFocus : MonoBehaviour {

    private void OnEnable()
    {
        this.GetComponent<InputField>().ActivateInputField();

        this.GetComponent<InputField>().onEndEdit.AddListener(
            delegate (string text) {
                this.GetComponent<InputField>().ActivateInputField();
            }
        );
    }
}
