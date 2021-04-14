using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

namespace GameToBeNamed.Utils {

    public class DamageTextAnimation : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private float m_speed_y;
        private void Start() {
            
            Sequence damageText = DOTween.Sequence();
            damageText.Join(transform.DOMoveY(m_speed_y, .5f).SetRelative(true));
            damageText.Join(transform.DOScale(new Vector3(0, 0, 0), 1f));
            m_text.material.DOColor(Color.clear, .2f);
            damageText.OnComplete(() => {
                
                Destroy(gameObject);
            });
            damageText.Play();
        }

    }
}