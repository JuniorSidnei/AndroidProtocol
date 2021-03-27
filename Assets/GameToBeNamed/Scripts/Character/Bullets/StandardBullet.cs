using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character.Bullets {

    public class StandardBullet : MonoBehaviour {

        [SerializeField] private Collision2DProxy m_colliderBox;
        [SerializeField] private int m_damage;
        [SerializeField] private float m_speed;
        [SerializeField] private GameObject m_onContactExplosion;
        [SerializeField] private AnimationCurve m_curve;
        [SerializeField] private LayerMask m_targetLayer;
        [SerializeField] private LayerMask m_wallLayer;
        [SerializeField] private float m_lifeTime;
        private ParticleSystem m_onContactExplosionParticleSystem;
        
        private void Awake() {
            m_onContactExplosionParticleSystem = m_onContactExplosion.transform.GetChild(1).GetComponentInChildren<ParticleSystem>();
            m_colliderBox.OnTrigger2DEnterCallback.AddListener(OnTrigger2DEnterCallback);
            Destroy(gameObject, .2f);
        }

        private void FixedUpdate() {
            var direction = m_speed * Time.deltaTime * transform.localScale.x;
            
            transform.position += new Vector3(direction, 0, 0);
        }

        private void OnTrigger2DEnterCallback(Collider2D ev) {
            
            if (((1 << ev.gameObject.layer) & m_targetLayer) == 0  || gameObject == null) {
                return;
            }
            
            if (((1 << ev.gameObject.layer) & m_wallLayer) != 0) {
                var contactExplosion = Instantiate(m_onContactExplosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
                return;
            }
            
            var info = new OnAttackTriggerEnter.Info {
                Emiter = gameObject, Receiver = ev.gameObject
            };
            GameManager.Instance.GlobalDispatcher.Emit(new OnAttackTriggerEnter(info, m_damage, ev.bounds.center));
            var temContactExplosion = Instantiate(m_onContactExplosion, ev.bounds.center, Quaternion.identity);
        
            var forceOverLifetimeModule = m_onContactExplosionParticleSystem.forceOverLifetime;
            forceOverLifetimeModule.enabled = true;
            var b = forceOverLifetimeModule.space == ParticleSystemSimulationSpace.World;
            forceOverLifetimeModule.x = new ParticleSystem.MinMaxCurve(100 * transform.localScale.x, m_curve);
            
            GameManager.Instance.GlobalDispatcher.Emit(new OnCameraScreenshake(1, .2f));
            gameObject.SetActive(false);
            Destroy(gameObject, m_lifeTime);
        }
    }
}
