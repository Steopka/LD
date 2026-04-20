using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Atack")]
    public KeyCode attackKey = KeyCode.Mouse1;
    public float attatckCooldown = 0.5f;
    public int damage = 1;
    public float attatRange = 0.7f;
    public LayerMask enemyLayers;


    private Animator anim;
    private float nextAttackTime = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && Time.time >= nextAttackTime)
        {
            anim.SetTrigger("Attack");
            PerformAttack();
            nextAttackTime = Time.time + attatckCooldown;
        }
    }
  void PerformAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attatRange);

        foreach(Collider2D obj in hitEnemies)
        {
            if (obj.CompareTag("Enemy"))
            {

                Health enemyHealth = obj.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TaKeDamge(damage);
                    Debug.Log("Урон нанесен об: " + obj.name);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attatRange);
    }
}