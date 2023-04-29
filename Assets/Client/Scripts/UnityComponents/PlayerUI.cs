using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text PlayerTitle;
    public Slider HpBar;
    public Camera Camera;

    private void Start()
    {
        Camera = Camera.main;
    }

    private void Update()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        var visible = Camera.rect.Contains(Camera.WorldToViewportPoint(transform.parent.position));
        transform.gameObject.SetActive(visible);
        if(visible)
        {
            transform.LookAt(Camera.transform);
        }
        
    }
}
