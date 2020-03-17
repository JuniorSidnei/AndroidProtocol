using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace GameToBeNamed.Character
{
    public class AnimatorProxy : MonoBehaviour
    {
        [SerializeField] private Animator m_anim;
        [SerializeField] private Character2D m_char2D;
        
        public UnityEvent OnBlockFinish;
        

        private void Update() {
            
            //OnGround
            m_anim.SetBool("OnGround", m_char2D.Controller2D.collisions.below);
            
            //Iddle
            m_anim.SetFloat("VelX", Mathf.Abs(m_char2D.Velocity.x));
            
            //Jump
            m_anim.SetFloat("VelY", m_char2D.Velocity.y);
            
            //Attack
            if (m_char2D.HasStatus(Character2D.Status.Attack)) {
                m_anim.SetTrigger("Attack");
            }
            
            //Block
            if(m_char2D.HasStatus(Character2D.Status.Blocking)){
                m_anim.SetTrigger("Blocking");
            }
            
            //3 funções de animação, com 3 eventos
            //atack com referencia pro proxy para se inscrever no eventos
            //animação de começo, execute(criar o overlapcircle e tudo mais) e fim
        }

        public void DeactiveBlockBox() {
            Debug.Log("emitindo evento");
            OnBlockFinish?.Invoke();
            
            //GameManager.Instance.GlobalDispatcher.Emit(new OnDeactiveBlockBox());
        }
    }
}