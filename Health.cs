using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public GameObject energyPrefab;

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
        if (energyPrefab != null)
        {
            Instantiate(energyPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        Debug.Log("Delet");
    }
}
