using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameToBeNamed.Character {

    public class PanelPlayingBehaviour : BaseUIBehavior {

        public enum CharacterType {
            Warrior, Shooter    
        }
        
        [Header("Life Settings")]
        [SerializeField] private TextMeshProUGUI m_lifeText;
        [SerializeField] private Image m_lifeSplashImage;
        [SerializeField] private Image m_iconSplashImage;

        
        [Header("Money Settings")]
        [SerializeField] private TextMeshProUGUI m_moneyText;

        [SerializeField] private Image m_changeClassCooldownSplashImage;

        [Header("Warrior Skills")]
        [SerializeField]  private GameObject m_warriorSkillsContainer;
        
        
        [Header("Shooter Skills")]
        [SerializeField]  private GameObject m_shooterSkillsContainer;
        [SerializeField]  private List<Image> m_ammunitionAmount;
        private bool m_isRechargeAnimationDone = true;

        
        private void Awake() {
            
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterDamage>(OnCharacterDamage);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterConfigureConstitution>(OnCharacterConfigureConstitution);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnUpdateCollectable>(OnUpdateCollectable);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterChangeClass>(OnCharacterChangeClass);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterUpdateClassCooldown>(OnCharacterUpdateClassCooldown);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnUpdateAmmunitionAmount>(OnUpdateAmmunitionAmount);
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
            
            switch (ev.Type) {
                case CharacterType.Shooter:
                    showShooterSettings();
                    break;
                case CharacterType.Warrior:
                    showWarriorSettings();
                    break;
            }
            
        }

        private void OnCharacterDamage(OnCharacterDamage ev) {
            
            if (!ev.IsPlayer) {
                return;
            }
            
            m_lifeText.text = $"{ev.CurrentHealth.ToString()} / {ev.MaxHealth}";
            m_lifeSplashImage.DOFillAmount((float)ev.CurrentHealth / ev.MaxHealth, .2f);
        }

        private void showShooterSettings() {
            m_warriorSkillsContainer.SetActive(false);
            m_shooterSkillsContainer.SetActive(true);
            
            foreach (var amount in m_ammunitionAmount) {
                amount.DOFade(1, 0.1f);
            }
        }

        private void showWarriorSettings() {
            m_shooterSkillsContainer.SetActive(false);
            m_warriorSkillsContainer.SetActive(true);
        }

        private void hideSettings() {
            
        }

        private void OnUpdateAmmunitionAmount(OnUpdateAmmunitionAmount ev) {
            for (var i = ev.CurrentAmount; i <= m_ammunitionAmount.Count - 1; i++) {
                    m_ammunitionAmount[i].DOFade(0, 0.1f);
            }
            
            if(ev.CurrentAmount == 0 && m_isRechargeAnimationDone) {
                m_isRechargeAnimationDone = false;
                StartCoroutine(rechargeAnimation());
            }
        }

        private IEnumerator rechargeAnimation() {
            yield return new WaitForSeconds(4f);
            foreach (var amount in m_ammunitionAmount) {
                amount.DOFade(1, 0.2f);
            }
            m_isRechargeAnimationDone = true;
        }
//        private void animateRechargeAmmunition(int index) {
//            if (index > 4) {
//                m_isRechargeAnimationDone = true;
//                return;
//            }
//            
//            m_ammunitionAmount[index].DOFade(1, 1f).OnComplete(() => {
//                index++;
//                animateRechargeAmmunition(index);
//            });
//        }
        
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