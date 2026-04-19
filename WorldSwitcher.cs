using UnityEngine;

public class WorldSwitcher : MonoBehaviour
{
    public GameObject[] WorldAObjects;

    public GameObject[] WorldBObjects;

    public KeyCode switchKey = KeyCode.E;

    public bool startInWorldA = true;

    private bool isWorldA;

     void Start()
    {
        if (startInWorldA)
            ActivateWorldA();
        else
            ActivateWorldB();
    }

     void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            if (isWorldA)
                ActivateWorldB();
            else
                ActivateWorldA();
        }
    }
  public  void ActivateWorldA()
    {
        // world B
        foreach (GameObject obj in WorldBObjects)
        {
            if (obj != null) obj.SetActive(false);
        }
        // world A
        foreach (GameObject obj in WorldAObjects)
        {
            if (obj != null) obj.SetActive(true);
        }
        isWorldA = true;
        Debug.Log("World A activ");
    }

    public  void ActivateWorldB()
    {
        // world B
        foreach (GameObject obj in WorldBObjects)
        {
            if (obj != null) obj.SetActive(true);
        }
        // world A
        foreach (GameObject obj in WorldAObjects)
        {
            if (obj != null) obj.SetActive(false);
        }
        isWorldA = false;
        Debug.Log("World B activ");
    }
}
