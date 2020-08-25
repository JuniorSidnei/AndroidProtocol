using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameToBeNamed.Character {

    public class PanelPlayingBehaviour : BaseUIBehavior {
        
        [Header("Life Settings")]
        [SerializeField] private TextMeshProUGUI m_lifeText;
        [SerializeField] private Image m_lifeSplashImage;
        [SerializeField] private Image m_iconSplashImage;

        
        [Header("Money Settings")]
        [SerializeField] private TextMeshProUGUI m_moneyText;

        [SerializeField] private Image m_changeClassCooldownSplashImage;
        private void Awake() {
            
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterDamage>(OnCharacterDamage);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterConfigureConstitution>(OnCharacterConfigureConstitution);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnUpdateCollectable>(OnUpdateCollectable);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterChangeClass>(OnCharacterChangeClass);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterUpdateClassCooldown>(OnCharacterUpdateClassCooldown);
        }

        private void OnCharacterUpdateClassCooldown(OnCharacterUpdateClassCooldown ev) {
           
        }

        private void OnCharacterChangeClass(OnCharacterChangeClass ev) {
            m_changeClassCooldownSplashImage.DOFillAmount(0, .01f).SetEase(Ease.Linear);
            m_changeClassCooldownSplashImage.DOFillAmount(1, 10f).SetEase(Ease.Linear);
        }


        private void OnUpdateCollectable(OnUpdateCollectable ev) {
            m_moneyText.text = $"{ev.CurrentAmount.ToString()}";
        }


        private void OnCharacterConfigureConstitution(OnCharacterConfigureConstitution ev) {
            
            m_lifeText.text = $"{ev.CurrentHealth.ToString()} / {ev.MaxHealth}";
            m_lifeSplashImage.fillAmount = (float)ev.CurrentHealth / ev.MaxHealth;

            m_lifeSplashImage.sprite = ev.LifeSplash;
            m_iconSplashImage.sprite = ev.IconSplash;
        }

        private void OnCharacterDamage(OnCharacterDamage ev) {
            
            if (!ev.IsPlayer) {
                return;
            }
            
            m_lifeText.text = $"{ev.CurrentHealth.ToString()} / {ev.MaxHealth}";
            m_lifeSplashImage.DOFillAmount((float)ev.CurrentHealth / ev.MaxHealth, .2f);
        }
        
        public  override void HandlePlayingMode() {
            base.HandlePlayingMode();
        }
        
        public  override void HandleConversationMode() {
            base.HandleConversationMode();
        }
        
        public  override void HandleShopMode() {
            base.HandleShopMode();
        }
        
        public  override void HandleCinematicMode() {
            base.HandleCinematicMode();
        }
    }
}