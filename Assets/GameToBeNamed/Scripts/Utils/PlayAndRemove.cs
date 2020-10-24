using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

namespace GameToBeNamed.Utils.Sound {
    
    public class PlayAndRemove : MonoBehaviour {
        public AudioSource audioSource;

        public bool IsPlaying {
            get { return audioSource.isPlaying; }
        }


        public void Play(AudioClip audioClip, bool loop, float volume, float duration, float pitch,
            float panStereo, float startFrom, AudioMixerGroup audioMixerGroup) {
            if (!audioSource) {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            name = audioClip.name;
            audioSource.time = startFrom;
            audioSource.playOnAwake = false;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.outputAudioMixerGroup = audioMixerGroup;

            audioSource.Play();
            audioSource.panStereo = panStereo;

            if (!loop)
                StartCoroutine(RemoveAudio(duration == 0 ? audioClip.length : duration));
            //Invoke("RemoveAudio", duration == 0 ? audioClip.length : duration);
        }

        IEnumerator RemoveAudio(float duration) {
            float t = Time.realtimeSinceStartup;

            do {
                yield return null;
            } while (Time.realtimeSinceStartup - t < duration);

            //audioSource.Stop();
        }

    }
}