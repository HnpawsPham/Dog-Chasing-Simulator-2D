
using UnityEngine;

public class Stamina : MonoBehaviour
{

    [Header("Setting: ")]
    [SerializeField] private float total;

    [Header("Sounds: ")]
    [SerializeField] private AudioSource exhausted;

    public float current { get; private set; }
    public bool isExhausted;

    private Animator anim;
    private Health personHealth;

    void Start()
    {
        personHealth = GetComponentInParent<Health>();

        current = total;
        isExhausted = false;

        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (current > total / 10)
        {
            isExhausted = false;
        }
        else if (current > total / 3)
        {
            exhausted.Stop();
        }

        if (!personHealth.isDead)
        {
            exhausted.Stop();
        }
    }

    public void Decrease(float minus)
    {
        current = Mathf.Clamp(current - minus, 0, total);

        if (current <= 0)
        {
            if (!exhausted.isPlaying)
            {
                exhausted.Play();
            }

            isExhausted = true;
        }
    }

    public void Increase(float stamina)
    {
        current = Mathf.Clamp(current + stamina, current, total);
    }
}
