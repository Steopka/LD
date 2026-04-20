using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waaypoints;

    public float speed = 2f;

    private int currentIndex = 0;
    private int direction = 1;

     void Update()
    {
        if (waaypoints.Length == 0) return;
        
        Transform target = waaypoints[currentIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

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
       
        Vector2 dir = (waaypoints[currentIndex].position - transform.position).normalized;
        if(dir.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(dir.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
