using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
    public int requiredEnergy = 3;

    public GameObject portalVisul;

    public ParticleSystem activateEffect;

    public bool autoLoadNextScene = true;
    public string nextSceneManagerName = "Levl_2";
    public Transform teleportTarget;

    private bool isActive = false;

    private Collider2D portalCollider;


     void Start()
    {
        portalCollider = GetComponent<Collider2D>();
        if( portalCollider != null ) portalCollider.enabled = false;
        if (portalVisul != null ) portalVisul.SetActive( false );

        if(EnergyManager.Instance != null)
        {
            EnergyManager.Instance.OnEnergyChanged += OnEnergyChanged;
            CheckActivation(EnergyManager.Instance.GetCurrentEnergy());
        }
       
    }

    void OnEnergyChanged(int currentEnergy, int requiredGlobal)
    {
        CheckActivation(currentEnergy);
    }
    void CheckActivation(int currentEnergy)
    {
        if(!isActive && currentEnergy >= requiredEnergy)
        {
            ActivatePortal();
        }
    }

    void ActivatePortal()
    {
        isActive = true;
        if(portalCollider != null) portalCollider.enabled = true;
        if (portalVisul != null) portalVisul.SetActive(true);
        if(activateEffect != null) activateEffect.Play();
    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;
        if(!other.CompareTag("Player")) return;

        if(autoLoadNextScene && !string.IsNullOrEmpty(nextSceneManagerName))
        {
            SceneManager.LoadScene(nextSceneManagerName);
        }
        else if( teleportTarget != null)
        {
            other.transform.position = teleportTarget.position;
        }
    }

    void OnDestroy()
    {
        if(EnergyManager.Instance != null) 
            EnergyManager.Instance.OnEnergyChanged -=OnEnergyChanged;
    }


}
