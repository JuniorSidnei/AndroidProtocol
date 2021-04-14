using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Utils {

    public class AreaLoaderTrigger : MonoBehaviour {

        public LayerMask PlayerLayer;
        [SerializeField] private SceneFieldController m_sceneToLoad;
        [SerializeField] private SceneFieldController m_sceneToUnload;
        [SerializeField] private AudioClip m_areaBgMusic;

        private void OnTriggerEnter2D(Collider2D other) {
            
            if (((1 << other.gameObject.layer) & PlayerLayer) != 0) {
                
                GameManager.Instance.GlobalDispatcher.Emit(new OnValidateScene(m_sceneToLoad, m_sceneToUnload, m_areaBgMusic));
            }
        }
    }
}