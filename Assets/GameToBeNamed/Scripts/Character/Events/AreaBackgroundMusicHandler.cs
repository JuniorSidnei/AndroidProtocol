using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils.Sound;
using UnityEngine;

namespace GameToBeNamed.Character.Sounds {
    
    public class AreaBackgroundMusicHandler : MonoBehaviour {
        
        [SerializeField] private AudioClip m_areaBgMusic;
        
        private void Awake() {
            StartCoroutine(StopAndPlayMusicArea());
        }

        private IEnumerator StopAndPlayMusicArea() {
            yield return new WaitForSeconds(0.5f);
            AudioController.Instance.Stop();
            AudioController.Instance.Play(m_areaBgMusic, AudioController.SoundType.Music, 0.4f, true);
        }
    }
}