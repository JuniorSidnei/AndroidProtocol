using System;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameToBeNamed.Character {
    
    [Serializable]
    public class OscilateAction : CharacterAction {

        private Character2D m_char;
        public List<PropertyName> UnallowedStatus;
        [SerializeField] private SpriteRenderer m_sprt;
        [SerializeField] private float m_speed;
        
        public float MaxRange;
        public float MinRange;
        private float m_maxRange;
        private float m_minRange;

        [Range(0f, 6.283185f)] public float Offset;

        //Pivot
        private Vector2 m_initialPosition;


        protected override void OnConfigure() {

            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
            
            m_maxRange = m_char.transform.position.x + MaxRange;
            m_minRange = m_char.transform.position.x - MinRange;

            Offset = Random.Range(0, 360 * Mathf.Deg2Rad);
            m_speed += Offset;

        }

        private void OnCharacterFixedUpdate(OnCharacterFixedUpdate ev) {

            if (m_char.ActionStates.AllNotDefault(UnallowedStatus).Any()) {
                return;
            }
            
            if (m_char.transform.position.x < m_maxRange) {//right
                
                m_char.Velocity += new Vector2(Mathf.Cos(m_speed * Time.deltaTime), 0);
                m_sprt.flipX = false;
            }
            else if(m_char.transform.position.x > m_minRange) { //left
                
                m_char.Velocity -= new Vector2(Mathf.Cos(m_speed * Time.deltaTime), 0);
                m_sprt.flipX = true;
            }
        }
    }
}