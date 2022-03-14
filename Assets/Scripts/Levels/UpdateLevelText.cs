using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TheFrozenBanana
{
    public class UpdateLevelText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _levelNumberTextBox;

        private void Start()
        {
            _levelNumberTextBox.text = GameManager.Instance.CurrentLevel.ToString();
        }
    }
}