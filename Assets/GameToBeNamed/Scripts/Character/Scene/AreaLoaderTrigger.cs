using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Utils {

    public class AreaLoaderTrigger : MonoBehaviour {

        public LayerMask PlayerLayer;
        [SerializeField] private SceneField m_sceneToLoad;
        [SerializeField] private SceneField m_sceneToUnload;

        private void OnTriggerEnter2D(Collider2D other) {
            
            if (((1 << other.gameObject.layer) & PlayerLayer) != 0) {
                GameManager.Instance.GlobalDispatcher.Emit(new OnValidateScene(m_sceneToLoad, m_sceneToUnload));   
            }
        }
    }
}