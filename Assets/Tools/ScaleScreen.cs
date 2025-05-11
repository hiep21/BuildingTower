using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tool
{
    public class ScaleScreen : MonoBehaviour
    {
        [SerializeField] bool isCanvasScaler = true, isScaleLogo = false;
        private float scaleValue, refX = 1080f, refY = 1920f;
        CanvasScaler ss;

        void Start()
        {
            if (isCanvasScaler)
            {
                ss = this.GetComponent<CanvasScaler>();
                ScaleScr();
            }
            else
            {
                ScaleUI();
            }

        }
        void ScaleScr()
        {
            float screenX = Screen.width;
            float screenY = Screen.height;

            // Tính aspect ratio của màn hình thực tế và màn hình tham chiếu
            float aspectRatioScreen = screenX / screenY;
            float aspectRatioRef = refX / refY;

            // Kiểm tra nếu màn hình thực tế rộng hơn so với tỉ lệ chuẩn
            if (aspectRatioScreen > aspectRatioRef)
            {
                ss.matchWidthOrHeight = 1f;  // Ưu tiên chiều cao
            }
            else
            {
                ss.matchWidthOrHeight = 0f;  // Ưu tiên chiều rộng
            }
        }
        void ScaleUI()
        {
            float screenX = Screen.width;
            float screenY = Screen.height;
            if (!isScaleLogo || screenX / screenY < 0.8f)
            {
                float newScale = (refY * screenX) / (refX * screenY);
                this.transform.localScale = new Vector3(newScale, newScale, 1f);
            }
        }

    }

}