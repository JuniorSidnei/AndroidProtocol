using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameToBeNamed.Character {

    public class AreaController : MonoBehaviour {
        
        private Dictionary<string, bool> m_loadedScenesMap = new Dictionary<string, bool>();

        private AsyncOperation m_loadScene;
        
        private void Awake() {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            GameManager.Instance.GlobalDispatcher.Subscribe<OnValidateScene>(OnValidateScene);
        }

        private void OnSceneUnloaded(Scene arg0) {
            m_loadedScenesMap.Remove(arg0.name);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            m_loadedScenesMap[arg0.name] = true;
        }


        private void OnValidateScene(OnValidateScene ev) {

            if (m_loadedScenesMap.ContainsKey(ev.SceneToLoad)) {
                return;
            }

            StartCoroutine(LoadScene(ev.SceneToLoad));
            m_loadedScenesMap[ev.SceneToLoad] = false;

            if (m_loadedScenesMap.ContainsKey(ev.SceneToUnload)) {
                
                if (m_loadedScenesMap[ev.SceneToUnload]) {
                    SceneManager.UnloadSceneAsync(ev.SceneToUnload);
                    m_loadedScenesMap[ev.SceneToUnload] = false;
                }
            }
        }

        IEnumerator LoadScene(string sceneToLoad) {
            m_loadScene = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            while (!m_loadScene.isDone) {
                yield return null;
            }
        }
    }
}