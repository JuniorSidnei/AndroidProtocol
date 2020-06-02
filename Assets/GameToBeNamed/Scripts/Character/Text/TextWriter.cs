using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameToBeNamed.Character {

    /// <summary>
    /// This class handle text typing effect
    /// </summary>
    public static class TextWriter {

        /// <summary>
        /// Be sure to always start the coroutine and end it on the same object, otherwise you will receive a Coroutine Continue Failure error  
        /// </summary>

        public static Coroutine WriteText(this MonoBehaviour mb, TextMeshProUGUI text, string finalText,
            AudioClip typingSound = null, Action onFinish = null) {
            return mb.StartCoroutine(DoWriteText(text, finalText, typingSound, onFinish));
        }

        private static IEnumerator DoWriteText(TextMeshProUGUI text, string finalText, AudioClip typingSound,
            Action onFinish) {
            Complete = false;
            text.text = "";
            int i = 0;
            Typing = true;
            while (text.text != finalText) {

                if (Complete) {
                    text.text = finalText;
                    break;
                }

                if (finalText[i] == '<') {

                    do {
                        text.text += finalText[i];
                        i++;
                    } while (finalText[i] != '>');
                }

                text.text += finalText[i];
                i++;
                if (typingSound != null) {
                    //todo audiocontroller
                    // AudioController.Instance.Play(typingSound, AudioController.SoundType.SoundEffect2D);    
                }

                yield return new WaitForSeconds(0.06f);
            }

            Typing = false;
            Complete = true;

            if (onFinish != null) {
                Debug.Log("invocando evento");
                onFinish();
            }
        }

        //Cut the dialog to final
        public static bool Complete { get; set; }

        //Cut the dialog to final while typing
        public static bool Typing { get; private set; }
    }
}