using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameToBeNamed.Utils {
    
    public class GameManager : Singleton<GameManager> {
        
        public QueuedEventDispatcher GlobalDispatcher = new QueuedEventDispatcher();
        
        private void Awake() {
            GlobalDispatcher.Emit(new OnGameStart());
            UIManager.Show();
        }

        private void Update(){
            GlobalDispatcher.DispatchAll();
        }
    }
}