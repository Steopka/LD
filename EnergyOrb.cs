using UnityEngine;

public class EnergyOrb : MonoBehaviour
{
    public int energyValue = 1;
    public GameObject pickupEffect;
    public float pickupDelay = 0.5f;

    private bool canBePicked = false;

     void Start()
    {
        GetComponent<Animator>().Play("Particls");
        Invoke(nameof(EnablePickup), pickupDelay);
    }

    void EnablePickup()
    {
        canBePicked = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            EnergyManager.Instance?.AddEnergy(energyValue);
            if(pickupEffect != null) Instantiate(pickupEffect,transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


}
