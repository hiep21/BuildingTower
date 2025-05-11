using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Tool
{
    public class ActionAnimPopup : MonoBehaviour
    {
        [SerializeField] GameObject panelPopup;
        [SerializeField] Image imgDimmer;
        [SerializeField] float rateScale = 0.9f;
        float oriFadeDimmer = 0.64f;
        bool isClickable = false;
        Vector3 scaleOriginal;



        public void SetUpDimmer(UnityEngine.Events.UnityAction callHidePopup)
        {
            if (imgDimmer.GetComponent<Button>() == null)
            {
                imgDimmer.AddComponent<Button>();
            }
            imgDimmer.GetComponent<Button>().onClick.AddListener(callHidePopup);
        }
        public void ShowPopup(System.Action callback)
        {
            if (isClickable) return;
            isClickable = true;

            if (scaleOriginal == Vector3.zero)
            {
                scaleOriginal = panelPopup.transform.localScale;
            }

            this.gameObject.SetActive(true);
            panelPopup.transform.localScale = scaleOriginal * rateScale;
            imgDimmer.DOFade(0, 0);
            panelPopup.transform.DOScale(scaleOriginal + new Vector3(0.05f, 0.05f, 0), 0.15f).SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                panelPopup.transform.DOScale(scaleOriginal, 0.15f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    isClickable = false;
                    callback?.Invoke();
                });
            });
            imgDimmer.DOFade(oriFadeDimmer, 0.15f).SetEase(Ease.Linear);
        }
        public void HidePopup(System.Action callback)
        {
            if (isClickable) return;
            isClickable = true;

            panelPopup.transform.DOScale(scaleOriginal + new Vector3(0.05f, 0.05f, 0), 0.15f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                panelPopup.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutBack);
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    this.gameObject.SetActive(false);
                    isClickable = false;
                    callback?.Invoke();
                });
            });
            imgDimmer.DOFade(0f, 0.2f).SetEase(Ease.Linear).SetDelay(0.15f);
        }
    }
}