using Unity.VisualScripting;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class StaminaBar : MonoBehaviour
{

    [SerializeField] private Stamina personStamina;
    [SerializeField] private Image currentStaminaBar;

    void Start()
    {
        currentStaminaBar.fillAmount = 1;
    }


    void Update()
    {
        currentStaminaBar.fillAmount = personStamina.current / 100;
    }
}
