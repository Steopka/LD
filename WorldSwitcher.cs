using System.Collections;
using UnityEngine;

public class WorldSwitcher : MonoBehaviour
{
    public GameObject[] WorldAObjects;
    public GameObject[] WorldBObjects;

    public KeyCode skillKey = KeyCode.E;
    public float skillDuration = 5f;
    public float skillCooldwn = 3f;

    private bool isWorldA = true;
    private bool isSkillOnCooldwn = false;
    private bool isInWorldB = false;




     void Start()
    {
       ActivateWorldA();
    }

    void Update()
    {
        if (Input.GetKeyDown(skillKey) && isWorldA && !isSkillOnCooldwn)
        {
            StartCoroutine(UseSkill());
        }
    }
    IEnumerator UseSkill()
    {
        ActivateWorldB();
        isWorldA = false;
       


        yield return new WaitForSeconds(skillDuration);

        ActivateWorldA();
        isWorldA = true;
   

        StartCoroutine(CooldownRoutine());

    }

    IEnumerator CooldownRoutine()
    {
        isSkillOnCooldwn = true;
        yield return new WaitForSeconds(skillCooldwn);
        isSkillOnCooldwn = false;
        Debug.Log("Skill ready to using");
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
