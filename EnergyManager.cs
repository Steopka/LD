using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;

    public int requiredEnergy = 3;
    public int currentEnergy = 0;

    public System.Action<int, int>  OnEnergyChanged;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;
        OnEnergyChanged?.Invoke(currentEnergy, requiredEnergy);
    }
    public int GetCurrentEnergy() => currentEnergy;
}
