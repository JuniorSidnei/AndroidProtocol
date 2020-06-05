using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameToBeNamed.Character {

    public class SceneAreaController : MonoBehaviour {
        
        [SerializeField] private SceneField m_nextScene;
        [SerializeField] private SceneField m_lastScene;

        private bool m_loaded;
        [SerializeField] private bool m_hasLastScene;

        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnValidadeScene>(OnLoadNextScene);
        }

       
        private void OnLoadNextScene(OnValidadeScene ev) {

            //next scene to load, unload last
            if (ev.SceneField.SceneName == m_nextScene.SceneName && !m_loaded) {
                SceneManager.LoadSceneAsync(ev.SceneField, LoadSceneMode.Additive);
                
                if (m_hasLastScene) {
                    SceneManager.UnloadSceneAsync(m_lastScene);
                }
                m_loaded = true;
            }
            //load last, unload next
            else if (ev.SceneField.SceneName == m_lastScene.SceneName && m_loaded) {
                SceneManager.LoadSceneAsync(m_lastScene, LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync(m_nextScene);
                m_loaded = false;
            }
        }
    }
}