using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image currentHealthBar;

    void Awake()
    {
    }


    void Update()
    {
        currentHealthBar.fillAmount = playerHealth.current / 100;
    }
}
