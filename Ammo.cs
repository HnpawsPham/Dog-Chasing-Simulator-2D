
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;


    private bool hit;
    private float direction;
    private float flyDuration;

    private BoxCollider2D boxCollider;
    private Animator anim;
    
    void Start()
    {
        speed = 10;
        direction = 1;
        flyDuration = 0;
    }
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(hit){    // IF AMMO HIT SOMETHING, IT DOESNT MOVE ANYMORE
            return;
        }

        float speedRealTime = speed * Time.deltaTime * direction;
        transform.Translate(speedRealTime, 0, 0);
        

        flyDuration += Time.deltaTime;
        if(flyDuration > 5){
            gameObject.SetActive(false);
        }
    }

    // WHEN THE AMMO HIT SOMETHING
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false; // AFTER HIT SOMETHING, TURN OFF PHYSIC OF THE AMMO
        anim.SetTrigger("hit");     // ENABLE CONDITION FOR HIT ANIMATION

        if(collision.tag == "enemy"){
            collision.GetComponent<Health>().Decrease(damage);
        }
    }
    public void SetDirection(float _direction){ // _direction equals 1 or -1
        direction = _direction;
        gameObject.SetActive(true);     
        hit = false;
        boxCollider.enabled = true;

        flyDuration = 0;

        // FLIP AMMO
        float localScaleX = transform.localScale.x;

        if(Mathf.Sign(localScaleX) != _direction){
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    // FREEZE AMMO: MAKE IT TRASH
    private void Deativate(){
        gameObject.SetActive(false);
    }
}
