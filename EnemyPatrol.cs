using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waaypoints;

    public float speed = 2f;

    private int currentIndex = 0;
    private int direction = 1;
    private Animator Animator;

     void Start()
    {
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (waaypoints.Length == 0) return;
        
        Transform target = waaypoints[currentIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        float speedParam = Vector2.Distance(transform.position, target.position) > 0.05f ? 1f : 0f;

        if (Animator != null)
            Animator.SetFloat("Particl", speedParam);

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            int nextIndex = currentIndex + direction;

            if (nextIndex < 0 || nextIndex >= waaypoints.Length)
            {
                direction *= -1;
                nextIndex = currentIndex + direction;
            }
                currentIndex = nextIndex;
        }
    }

    private void FixedUpdate()
    {
       if(waaypoints.Length == 0) return;
        Vector2 dir = (waaypoints[currentIndex].position - transform.position).normalized;
        if(dir.x != 0)
        {
            transform.localScale = new Vector3(-Mathf.Sign(dir.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
