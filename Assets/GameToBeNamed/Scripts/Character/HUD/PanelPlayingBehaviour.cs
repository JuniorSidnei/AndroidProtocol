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

        [Header("RecoverySettings")]
        [SerializeField]  private List<Image> m_recoveryIcons;

        [Header("Money Settings")]
        [SerializeField] private TextMeshProUGUI m_moneyText;

        [SerializeField] private Image m_changeClassCooldownSplashImage;

        [Header("Warrior Skills")]
        [SerializeField]  private GameObject m_warriorSkillsContainer;
        
        
        [Header("Shooter Skills")]
        [SerializeField]  private GameObject m_shooterSkillsContainer;
        [SerializeField]  private List<Image> m_ammunitionAmount;
        private bool m_isRechargeAnimationDone = true;


        private bool isWarriorSkillHudActive = false;
        private bool warriorUnlockedSkills = false;
        
        private void Awake() {
            
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterDamage>(OnCharacterDamage);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterConfigureConstitution>(OnCharacterConfigureConstitution);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnUpdateCollectable>(OnUpdateCollectable);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterChangeClass>(OnCharacterChangeClass);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterUpdateClassCooldown>(OnCharacterUpdateClassCooldown);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnUpdateAmmunitionAmount>(OnUpdateAmmunitionAmount);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnUpdateRecovery>(OnUpdateRecovery);
        }

        private void OnCharacterUpdateClassCooldown(OnCharacterUpdateClassCooldown ev) {
           
        }

        private void OnCharacterChangeClass(OnCharacterChangeClass ev) {
            m_changeClassCooldownSplashImage.DOFillAmount(0, .01f).SetEase(Ease.Linear);
            m_changeClassCooldownSplashImage.DOFillAmount(1, 4f).SetEase(Ease.Linear);
        }


        private void OnUpdateCollectable(OnUpdateCollectable ev) {
            m_moneyText.text = $"{ev.CurrentAmount.ToString()}";
        }


        private void OnCharacterConfigureConstitution(OnCharacterConfigureConstitution ev) {
            
            m_lifeText.text = $"{ev.CurrentHealth.ToString()} / {ev.MaxHealth}";
            m_lifeSplashImage.fillAmount = (float)ev.CurrentHealth / ev.MaxHealth;

            m_lifeSplashImage.sprite = ev.LifeSplash;
            m_iconSplashImage.sprite = ev.IconSplash;

            for (var i = 0; i < ev.CurrentBatteries; i++) {
                m_recoveryIcons[i].DOFade(1, 0.1f).SetEase(Ease.InQuad);
            }
            
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
            isWarriorSkillHudActive = false;
            m_warriorSkillsContainer.SetActive(false);
            m_shooterSkillsContainer.SetActive(true);
            
            foreach (var amount in m_ammunitionAmount) {
                amount.DOFade(1, 0.1f);
            }
        }

        private void showWarriorSettings() {
            m_shooterSkillsContainer.SetActive(false);
            isWarriorSkillHudActive = true;
            
            if (!warriorUnlockedSkills) return;

            m_warriorSkillsContainer.SetActive(true);
        }

        private void OnUpdateRecovery(OnUpdateRecovery ev) {
            for (var i = ev.CurrentAmount; i < m_recoveryIcons.Count; i++) {
                m_recoveryIcons[i].DOFade(0, 0.1f).SetEase(Ease.InQuad);
            }
        }
        
        private void OnUpdateAmmunitionAmount(OnUpdateAmmunitionAmount ev) {
            for (var i = ev.CurrentAmount; i < m_ammunitionAmount.Count; i++) {
                    m_ammunitionAmount[i].DOFade(0, 0.1f).SetEase(Ease.InQuad);
            }
            
            if(ev.CurrentAmount == 0 && m_isRechargeAnimationDone) {
                m_isRechargeAnimationDone = false;
                StartCoroutine(rechargeAnimation(ev.RechargeTime));
            }
        }

        private IEnumerator rechargeAnimation(float rechargeTime) {
            yield return new WaitForSeconds(rechargeTime);
            foreach (var amount in m_ammunitionAmount) {
                amount.DOFade(1, 0.2f).SetEase(Ease.InCubic);
            }
            m_isRechargeAnimationDone = true;
        }

        public  override void HandlePlayingMode() {
            base.HandlePlayingMode();
            
            if (isWarriorSkillHudActive) {
                showWarriorSettings();
            }
            else {
                showShooterSettings();
            }
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