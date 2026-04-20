using System.Collections;
using UnityEngine;


public class RandomSpellBoss : MonoBehaviour
{
    [Header("Компоненты")]
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _castPoint;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public Animator animator;
    [Header("Префабы заклинаний (необязательно)")]
    [SerializeField] private GameObject _fireballPrefab;


    [SerializeField] private GameObject _poisonCloudPrefab;


    [Header("Настройки каста")]
    [SerializeField] private float _castInterval = 2.5f;
    [SerializeField] private float _projectileSpeed = 6f;
    [SerializeField] private float _spellCooldown = 1.5f;

    [Header("Урон заклинаний")]
    [SerializeField] private int _fireballDamage = 1;

    [SerializeField] private int _poisonDamagePerSecond = 5;


    [Header("Здоровье босса")]
    [SerializeField] private int _maxHealth = 30;
    private int _currentHealth;

    [Header("Визуал")]
    [SerializeField] private Color _castColor = new Color(1f, 0.5f, 0.5f);
    [SerializeField] private Color _healColor = Color.green;
    [SerializeField] private Color _shieldColor = Color.cyan;

    private Color _defaultColor;
    private bool _canCast = true;
    private bool _isAlive = true;
    private bool _hasShield = false;

    private enum SpellType
    {
        Fireball,
        IceShard,
        Lightning,
        PoisonCloud,
        Heal,
        Shield,
        Teleport,
        SummonMinion
    }

    private void Start()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        _defaultColor = _spriteRenderer.color;
        _currentHealth = _maxHealth;

        if (_player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) _player = playerObj.transform;
        }

        if (_castPoint == null)
            _castPoint = transform;

        StartCoroutine(CastRoutine());
    }

    private IEnumerator CastRoutine()
    {
        while (_isAlive)
        {
            yield return new WaitForSeconds(_castInterval);

            if (_player != null && _canCast && _isAlive)
            {
                CastRandomSpell();
            }
        }
    }

    private void CastRandomSpell()
    {
        SpellType randomSpell = (SpellType)Random.Range(0, 8);

        switch (randomSpell)
        {
            case SpellType.Fireball:
                StartCoroutine(CastFireball());
                break;
            //case SpellType.IceShard:
            //    StartCoroutine(CastIceShard());
            //    break;
            //case SpellType.Lightning:
            //    StartCoroutine(CastLightning());
            //    break;
            case SpellType.PoisonCloud:
                StartCoroutine(CastPoisonCloud());
                break;
            case SpellType.Heal:
                StartCoroutine(CastHeal());
                break;
            case SpellType.Shield:
                StartCoroutine(CastShield());
                break;
            case SpellType.Teleport:
                StartCoroutine(CastTeleport());
                break;
            //case SpellType.SummonMinion:
            //    StartCoroutine(CastSummonMinion());
            //    break;
        }

        Debug.Log($" Босс кастует: {randomSpell}");
    }



    private IEnumerator CastFireball()
    {
        yield return StartCoroutine(SpellCastAnimation(_castColor));

        GameObject fireball = CreateProjectile("Fireball", Color.red);
        if (fireball != null)
        {
            BossProjectile proj = fireball.GetComponent<BossProjectile>();
            if (proj == null) proj = fireball.AddComponent<BossProjectile>();
            proj.damage = _fireballDamage;

            LaunchProjectile(fireball);
        }
    }

    //private IEnumerator CastIceShard()
    //{
    ////    yield return StartCoroutine(SpellCastAnimation(Color.cyan));

    //    //    for (int i = -1; i <= 1; i++)
    //    //    {
    //    //        GameObject shard = CreateProjectile("IceShard", new Color(0.5f, 0.8f, 1f));
    //    //        if (shard != null)
    //    //        {
    //    //            BossProjectile proj = shard.GetComponent<BossProjectile>();
    //    //            if (proj == null) proj = shard.AddComponent<BossProjectile>();
    //    //            proj.damage = _iceShardDamage;

    //    //            Rigidbody2D rb = shard.GetComponent<Rigidbody2D>();
    //    //            if (rb != null && _player != null)
    //    //            {
    //    //                Vector2 direction = (_player.position - _castPoint.position).normalized;
    //    //                Vector2 spread = Quaternion.Euler(0, 0, i * 20f) * direction;
    //    //                rb.linearVelocity = spread * _projectileSpeed;
    //    //            }
    //    //        }
    //    //    }
    //    //}

        ////private IEnumerator CastLightning()
        ////{
        ////    yield return StartCoroutine(SpellCastAnimation(Color.yellow));


        ////    if (_player != null)
        ////    {
        ////        float distance = Vector2.Distance(transform.position, _player.position);
        ////        if (distance <= 4f)
        ////        {
        ////            DealDamageToPlayer(_lightningDamage);
        ////        }
        ////    }


        ////    StartCoroutine(DrawLightningLine());
        ////    yield return new WaitForSeconds(0.3f);
        ////}

    private IEnumerator DrawLightningLine()
    {
        if (_player == null) yield break;

        GameObject lineObj = new GameObject("LightningLine");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.yellow;
        lr.endColor = Color.white;
        lr.positionCount = 2;

        lr.SetPosition(0, _castPoint.position);
        lr.SetPosition(1, _player.position);

        yield return new WaitForSeconds(0.2f);

        Destroy(lineObj);
    }

    private IEnumerator CastPoisonCloud()
    {
        yield return StartCoroutine(SpellCastAnimation(Color.green));

        Vector3 cloudPos = _player != null ? _player.position : transform.position;
        GameObject cloud = CreatePoisonCloudObject(cloudPos);

        BossPoisonCloud cloudScript = cloud.GetComponent<BossPoisonCloud>();
        if (cloudScript == null) cloudScript = cloud.AddComponent<BossPoisonCloud>();
        cloudScript.damagePerSecond = _poisonDamagePerSecond;
    }

    private GameObject CreatePoisonCloudObject(Vector3 position)
    {
        GameObject cloud;

        if (_poisonCloudPrefab != null)
        {
            cloud = Instantiate(_poisonCloudPrefab, position, Quaternion.identity);
        }
        else
        {
            cloud = new GameObject("PoisonCloud");
            cloud.transform.position = position;

            SpriteRenderer sr = cloud.AddComponent<SpriteRenderer>();
            sr.sprite = CreateCircleSprite(64);
            sr.color = new Color(0, 1, 0, 0.3f);
            cloud.transform.localScale = Vector3.one * 3f;

            CircleCollider2D col = cloud.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 1.5f;

            StartCoroutine(PulseCloud(cloud));
        }

        Destroy(cloud, 5f);
        return cloud;
    }

    private IEnumerator CastHeal()
    {
        int healAmount = 20;
        _currentHealth = Mathf.Min(_currentHealth + healAmount, _maxHealth);

        _spriteRenderer.color = _healColor;

        
        for (int i = 0; i < 5; i++)
        {
            CreateHealParticle();
        }

        yield return new WaitForSeconds(0.5f);
        _spriteRenderer.color = _hasShield ? _shieldColor : _defaultColor;

        Debug.Log($" Босс исцелился! HP: {_currentHealth}");
    }

    private void CreateHealParticle()
    {
        GameObject heart = new GameObject("HealEffect");
        heart.transform.position = transform.position + Vector3.up * 1.5f;

        SpriteRenderer sr = heart.AddComponent<SpriteRenderer>();
        sr.sprite = CreateCircleSprite(12);
        sr.color = Color.green;
        sr.sortingOrder = 10;

        Rigidbody2D rb = heart.AddComponent<Rigidbody2D>();
        rb.gravityScale = -0.5f;
        rb.linearVelocity = new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 3f));

        Destroy(heart, 1.5f);
    }

    private IEnumerator CastShield()
    {
        _hasShield = true;
        if (animator != null)
        {
            animator.SetTrigger("Shield");
        }

        _spriteRenderer.color = _shieldColor;

        
        GameObject shield = new GameObject("ShieldVisual");
        shield.transform.position = transform.position;
        shield.transform.SetParent(transform);

        SpriteRenderer sr = shield.AddComponent<SpriteRenderer>();
        sr.sprite = CreateCircleSprite(48);
        sr.color = new Color(0, 0.8f, 1f, 0.4f);
        sr.sortingOrder = -1;
        shield.transform.localScale = Vector3.one * 2.5f;

        float timer = 0f;
        while (timer < 4f)
        {
            if (shield != null)
                shield.transform.Rotate(0, 0, 90 * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(shield);
        _hasShield = false;
        _spriteRenderer.color = _defaultColor;

        Debug.Log(" Щит спал!");
    }

    private IEnumerator CastTeleport()
    {
       
        float fadeTime = 0.3f;
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float alpha = 1 - (timer / fadeTime);
            _spriteRenderer.color = new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, alpha);
            yield return null;
        }

        
        if (_player != null)
        {
            Vector2 randomCircle = Random.insideUnitCircle * 4f;
            transform.position = _player.position + new Vector3(randomCircle.x, randomCircle.y, 0);
        }

        
        timer = 0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeTime;
            _spriteRenderer.color = new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, alpha);
            yield return null;
        }

        _spriteRenderer.color = _defaultColor;
        Debug.Log(" Босс телепортировался!");
    }

    //private IEnumerator CastSummonMinion()
    //{
    //    yield return StartCoroutine(SpellCastAnimation(Color.gray));

    //    Vector3 spawnPos = transform.position + new Vector3(Random.Range(-2f, 2f), 0, 0);
    //    GameObject minion = CreateMinion(spawnPos);

    //    BossMinion minionScript = minion.GetComponent<BossMinion>();
    //    if (minionScript == null) minionScript = minion.AddComponent<BossMinion>();

    //    minionScript.damage = _minionDamage;
    //    minionScript.player = _player;
    //    minionScript.speed = 1.5f;

    //    Destroy(minion, 8f);

    //    Debug.Log(" Призван миньон!");
    //}

    //private GameObject CreateMinion(Vector3 position)
    //{
    //    if (_minionPrefab != null)
    //        return Instantiate(_minionPrefab, position, Quaternion.identity);

    //    GameObject minion = new GameObject("Minion");
    //    minion.transform.position = position;

    //    SpriteRenderer sr = minion.AddComponent<SpriteRenderer>();
    //    sr.sprite = CreateCircleSprite(16);
    //    sr.color = Color.gray;

    //    BoxCollider2D col = minion.AddComponent<BoxCollider2D>();
    //    Rigidbody2D rb = minion.AddComponent<Rigidbody2D>();
    //    rb.gravityScale = 1f;
    //    rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    //    return minion;
    //}

    public void DealDamageToPlayer(int damage)
    {
        if (_player == null) return;

        PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log($" Игрок получил {damage} урона!");
        }
    }

    private IEnumerator SpellCastAnimation(Color flashColor)
    {
        _canCast = false;
        _spriteRenderer.color = flashColor;

        yield return new WaitForSeconds(0.3f);

        _spriteRenderer.color = _hasShield ? _shieldColor : _defaultColor;

        yield return new WaitForSeconds(_spellCooldown - 0.3f);

        _canCast = true;
    }

    private GameObject CreateProjectile(string name, Color color)
    {
        GameObject proj;

        if (name == "Fireball" && _fireballPrefab != null)
            proj = Instantiate(_fireballPrefab, _castPoint.position, Quaternion.identity);
        //else if (name == "IceShard" && _iceShardPrefab != null)
        //    proj = Instantiate(_iceShardPrefab, _castPoint.position, Quaternion.identity);
        else
        {
            proj = new GameObject(name);
            proj.transform.position = _castPoint.position;

            SpriteRenderer sr = proj.AddComponent<SpriteRenderer>();
            sr.sprite = CreateCircleSprite(16);
            sr.color = color;
            sr.sortingOrder = 5;

            CircleCollider2D col = proj.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.5f;
        }

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb == null) rb = proj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        Destroy(proj, 5f);
        return proj;
    }

    private void LaunchProjectile(GameObject proj)
    {
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null && _player != null)
        {
            Vector2 direction = (_player.position - _castPoint.position).normalized;
            rb.linearVelocity = direction * _projectileSpeed;
        }
    }

    private IEnumerator PulseCloud(GameObject cloud)
    {
        float timer = 0f;
        Vector3 baseScale = cloud.transform.localScale;

        while (cloud != null && timer < 5f)
        {
            float pulse = 1f + Mathf.Sin(timer * 3f) * 0.1f;
            cloud.transform.localScale = baseScale * pulse;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private Sprite CreateCircleSprite(int size)
    {
        Texture2D tex = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                pixels[y * size + x] = dist < radius ? Color.white : Color.clear;
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    public void TakeDamage(int damage)
    {
        if (_hasShield)
        {
            damage = Mathf.RoundToInt(damage * 0.3f);
            Debug.Log(" Щит поглотил часть урона!");
        }

        _currentHealth -= damage;
        StartCoroutine(DamageFlash());

        Debug.Log($" Босс получил {damage} урона. HP: {_currentHealth}/{_maxHealth}");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageFlash()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = _hasShield ? _shieldColor : _defaultColor;
    }

    private void Die()
    {
        _isAlive = false;
        StopAllCoroutines();
        StartCoroutine(DeathEffect());

        Debug.Log(" Босс побеждён!");
    }

    private IEnumerator DeathEffect()
    {
        float timer = 0f;
        float duration = 1.5f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            _spriteRenderer.color = Color.Lerp(_defaultColor, Color.clear, t);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

            if (Random.value < 0.3f)
            {
                GameObject particle = new GameObject("DeathParticle");
                particle.transform.position = transform.position + Random.insideUnitSphere * 1f;

                SpriteRenderer sr = particle.AddComponent<SpriteRenderer>();
                sr.sprite = CreateCircleSprite(8);
                sr.color = new Color(1f, 0.5f, 0.5f, 1f);

                Rigidbody2D rb = particle.AddComponent<Rigidbody2D>();
                rb.linearVelocity = Random.insideUnitCircle * 3f;
                rb.gravityScale = 1f;

                Destroy(particle, 1f);
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (_castPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_castPoint.position, 0.2f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 4f);
    }
}

public class BossProjectile : MonoBehaviour
{
    public int damage = 10;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($" Снаряд нанёс {damage} урона!");
            }
            Destroy(gameObject);
        }
    }
}

public class BossPoisonCloud : MonoBehaviour
{
    public int damagePerSecond = 5;

    private float _damageTimer = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _damageTimer += Time.deltaTime;
            if (_damageTimer >= 1f)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damagePerSecond);
                    Debug.Log($" Яд нанёс {damagePerSecond} урона!");
                }
                _damageTimer = 0f;
            }
        }
    }
}
public class BossMinion : MonoBehaviour
{
    public int damage = 5;
    public Transform player;
    public float speed = 1.5f;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) _rb = gameObject.AddComponent<Rigidbody2D>();

        _sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            _rb.linearVelocity = new Vector2(direction.x * speed, _rb.linearVelocity.y);

            if (_sr != null)
                _sr.flipX = direction.x < 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($" Миньон нанёс {damage} урона!");
            }
        }
    }
}
