using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertical_CameraResolution : MonoBehaviour
{
    /// <summary>
    /// 세로버전
    /// </summary>
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();

        Rect rt = cam.rect;

        // 현재 가로 모드 16:9, 반대 세로모드는 9:16 입력
        float scale_height  = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (가로 / 세로)
        float scale_width = 1f / scale_height;

        if (scale_height < 1)
        {
            rt.height = scale_height;
            rt.y = (1f - scale_height) / 2f;
        }

        else
        {
            rt.width = scale_width;
            rt.x = (1f - scale_width) / 2f;
        }

        cam.rect = rt;
    }
}
