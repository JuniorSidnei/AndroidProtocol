using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameToBeNamed.Utils {

    public class LastSceneAreaManager : MonoBehaviour {

        public LayerMask PlayerLayer;
        [SerializeField] private SceneField m_lastScene;


        private void OnTriggerEnter2D(Collider2D other) {
            
            if (((1 << other.gameObject.layer) & PlayerLayer) != 0) {
                GameManager.Instance.GlobalDispatcher.Emit(new OnValidadeScene(m_lastScene));   
            }
        }
    }
}