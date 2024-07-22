
using UnityEngine;

public class Stamina : MonoBehaviour
{

    [Header("Setting: ")]
    [SerializeField] public float total;

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
            SoundPlayer.instance.Stop(gameObject.name + " exhausted");
        }

        if (!personHealth.isDead)
        {
            SoundPlayer.instance.Stop(gameObject.name + " exhausted");
        }
        else{
            SoundPlayer.instance.Stop(gameObject.name + " exhausted");
        }
    }

    public void Decrease(float minus)
    {
        current = Mathf.Clamp(current - minus, 0, total);

        if (current <= 0 && !personHealth.isDead)
        {
            SoundPlayer.instance.Play(gameObject.name + " exhausted");

            isExhausted = true;
        }
    }

    public void Increase(float stamina)
    {
        current = Mathf.Clamp(current + stamina, current, total);
    }
}
