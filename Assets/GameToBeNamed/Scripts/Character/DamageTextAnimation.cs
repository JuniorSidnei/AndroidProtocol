using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

namespace GameToBeNamed.Utils {

    public class DamageTextAnimation : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI m_text;
        private void Start() {
            
            Sequence damageText = DOTween.Sequence();
            damageText.Join(transform.DOMoveY(5, .5f).SetRelative(true));
            m_text.material.DOColor(Color.Lerp(Color.yellow, Color.grey, .2f), .2f);
            damageText.Join(transform.DOScale(new Vector3(0, 0, 0), 1f));
            m_text.material.DOColor(Color.clear, .2f);
            damageText.OnComplete(() => {
                
                Destroy(gameObject);
            });
            damageText.Play();
        }

    }
}