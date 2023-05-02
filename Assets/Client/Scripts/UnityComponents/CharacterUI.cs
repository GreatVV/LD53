using System.Collections;
using System.Collections.Generic;
using LeopotamGroup.Globals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD52
{
    public class CharacterUI : MonoBehaviour
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

        public void Refresh(Character character)
        {
            PlayerTitle.text = $"{character.LocalizedName} <alpha=#88>Level {character.Characteristics.Level}";
            HpBar.normalizedValue = (float) (character.Health / character.MaxHeals);
        }

        private void LookAtCamera()
        {
            if (Camera)
            {
                transform.LookAt(Camera.transform);
            }
        }
    }
}