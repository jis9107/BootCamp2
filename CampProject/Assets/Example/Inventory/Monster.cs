using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    Rigidbody2D _rigidbody;

    [Header("HP")]
    public GameObject canvas;
    public GameObject preHPBar;
    public float maxHP = 50f;
    public float currentHP;
    private RectTransform hpBar;

    [Header("Move")]
    public float Speed = 5f;
    public int switchCount = 0;
    private int moveCount = 0;

  
    
    public Vector2 _direction;

    private LayerMask playerMask;
    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        playerMask = LayerMask.NameToLayer("Player");
        currentHP = maxHP;
    }

    void Start()
    {
        hpBar = Instantiate(preHPBar, canvas.transform).GetComponent<RectTransform>();
        hpBar.GetComponentInChildren<Image>().fillAmount = currentHP / maxHP;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(_direction.x * Speed * Time.deltaTime, 0, 0);

        moveCount++;

        if (moveCount >= switchCount)
        {
            _direction *= -1;
            _spriteRenderer.flipX = _direction.x < 0;
            moveCount = 0;
        }
        
        hpBar.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1.7f));
    }

    public void TakeDamage(float value)
    {
        currentHP -= value;
        hpBar.GetComponentInChildren<Image>().fillAmount = currentHP/ maxHP;
        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Skill"))
        {
            Debug.Log(other.name);
            TakeDamage(15f);
        }
    }


}
