using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tool
{
    public class FPSController : MonoBehaviour
    {
        [Header("FPS Settings")]
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private bool showFpsCounter = true;

        [Header("UI Reference")]
        [SerializeField] private Text fpsText; // Optional - để hiển thị FPS
        private float deltaTime = 0.0f;

        private void Awake()
        {
            SetTargetFPS();
            // SetTargetFPS(targetFrameRate);
        }

        private void Update()
        {
            if (showFpsCounter)
            {
                // Tính toán FPS
                deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
                float fps = 1.0f / deltaTime;

                // Hiển thị FPS nếu có UI Text
                if (fpsText != null)
                {
                    fpsText.text = $"FPS: {Mathf.Round(fps)}";
                }
            }
        }

        public void SetTargetFPS()
        {
            // targetFrameRate = (int)sliderFPS.value;
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = 0; // Tắt VSync để FPS limit hoạt động
        }

        public void ToggleFPSCounter(bool show)
        {
            showFpsCounter = show;
            if (fpsText != null)
            {
                fpsText.gameObject.SetActive(show);
            }
        }
    }

}