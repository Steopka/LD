using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

     void Start()
    {
        currentHealth = maxHealth;
    }

    public void TaKeDamge(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
        Debug.Log("Delet");
    }
}
