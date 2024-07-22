
using UnityEngine.UI;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health personHealth;
    [SerializeField] private Image currentHealthBar;

    void Awake()
    {
    }


    void Update()
    {
        currentHealthBar.fillAmount = personHealth.current / 100;
    }
}
