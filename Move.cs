using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer; 

    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _horizontalInput;
    private bool _isGrounded;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {

        _horizontalInput = Input.GetAxis("Horizontal");

  
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

  
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
            _animator.SetTrigger("Jump");
        }

        _animator.SetFloat("Speed", Mathf.Abs(_horizontalInput));

        if (_horizontalInput != 0)
        {
            _spriteRenderer.flipX = _horizontalInput < 0;
           
        }


    }

    private void FixedUpdate()
    {
      
        _rb.linearVelocity = new Vector2(_horizontalInput * _speed, _rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
