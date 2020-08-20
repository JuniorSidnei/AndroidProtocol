using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using Rewired;
using UnityEngine;

namespace GameToBeNamed.Character{

    public class CharacterManager : MonoBehaviour{

        [SerializeField] private List<Character2D> m_characters;
        private Character2D m_currentCharacter;
        private int m_characterIndex;
        
        public int PlayerID = 0;

        public float ChangeClassCooldown;
        private float m_changeClassCooldown;
        private Player m_player;
        private bool m_onTalkingNpc;
        
        public void Start() {
            
            GameManager.Instance.GlobalDispatcher.Subscribe<OnTalking>(OnTalking);
            
            m_player = ReInput.players.GetPlayer(PlayerID);
            SpawnCharacter();
        }

        private void OnTalking(OnTalking ev) {
            m_onTalkingNpc = ev.OnTalkingNpc;
        }

        public void Update() {

            m_changeClassCooldown -= Time.deltaTime;
            
            if (m_changeClassCooldown <= 0) {
                m_changeClassCooldown = 0;
            }
           
            if (m_player.GetButtonDown("ChangeClass") && m_changeClassCooldown <= 0 &&
                !(m_currentCharacter.Controller2D.collisions.left || m_currentCharacter.Controller2D.collisions.right) && !m_onTalkingNpc) {
                SpawnCharacter();
            }
        }

        private void SpawnCharacter() {

            m_changeClassCooldown = ChangeClassCooldown;
           
            Vector3 spawnPosition = transform.position;
            
            if (m_currentCharacter != null){
                spawnPosition = m_currentCharacter.transform.position;
                
                m_currentCharacter.LocalDispatcher.Emit(new OnCharacterTransition(() => {
                    Destroy(m_currentCharacter.gameObject);
                    InstantiateCharacter(spawnPosition);
                }));
            }
            else {
                InstantiateCharacter(spawnPosition);
            }
        }

        private void InstantiateCharacter(Vector3 spawnPosition) {
            
            m_currentCharacter = Instantiate(m_characters[++m_characterIndex % m_characters.Count],
                new Vector3(spawnPosition.x, spawnPosition.y +5, spawnPosition.z), Quaternion.identity, transform);
            
            
            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterChangeClass(m_currentCharacter, m_currentCharacter.Velocity, m_changeClassCooldown));
        }
    }
}