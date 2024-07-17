
using UnityEngine;

public class Stamina : MonoBehaviour
{

    [Header("Setting: ")]
    [SerializeField] private float total;

    public float current { get; private set; }
    public bool isExhausted;

    private Animator anim;


    void Start()
    {
        current = total;
        isExhausted = false;

        anim = GetComponent<Animator>();
    }


    void Update()
    {   
        if (current > 0)
        {

            isExhausted = false;
        }
    }

    public void Decrease(float minus)
    {
        current = Mathf.Clamp(current - minus, 0, total);

        if (current <= 0)
        {
            isExhausted = true;
        }
    }

    public void Increase(float stamina)
    {
        current = Mathf.Clamp(current + stamina, current, total);
    }
}
