using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;


[Serializable]
public struct DamageFieldData
{
    public float distance;
}

public class CharController : MonoBehaviour
{
    private const float jumpTestValue = 0.3f;
    private static readonly int Speed1 = Animator.StringToHash("Speed");
    private static readonly int Ground = Animator.StringToHash("Ground");
    [SerializeField] private float Speed = 5.0f;
    [SerializeField] private float JumpSpeed = 15.0f;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float CameraSpeed = 4.0f;
    [SerializeField] private float MaxDistence = 4.0f;
    
    private Vector3 cameraOffset;
    
    InputAction Move_Input;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private InputAction Jump_Input;

    public List<CButton> _buttons;
    public List<DamageField> _damageFields;
    public List<DamageFieldData> _damageFieldDatas;

    [Header("HP")] 
    public float maxHP = 100f;
    public float currentHP;
    RectTransform healthBar;
    public GameObject healthBarObject;
    public GameObject canvas;
    

    public bool Grounded = true;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = Instantiate(healthBarObject, canvas.transform).GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        UnityEngine.InputSystem.PlayerInput Input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        Move_Input = Input.actions["Move"];
        Jump_Input = Input.actions["Jump"];

        cameraOffset = _mainCamera.transform.position - transform.position;
        
        foreach (var cButton in _buttons)
        {
            cButton.Addlistener(() => FireSkill(cButton.index));
        }
    }

    private bool canMove = true;

    void FireDamageField(int index)
    {
        GameObject go = Instantiate(_damageFields[index].gameObject);
        go.transform.position = transform.position + transform.right * _damageFieldDatas[index].distance;
        if(_spriteRenderer.flipX)
            go.transform.position = transform.position - transform.right * _damageFieldDatas[index].distance;
        Destroy(go, 1.0f);
    }

    void CanMove(int bMove)
    {
        canMove = bMove == 1;
    }

    void FireSkill(int index)
    {
        _animator.Rebind();

        switch (index)
        {
            case 0 : _animator.Play("Attack");
                break;
            case 1: _animator.Play("AttackRed");
                break;
        }
    }

    // IEnumerator FireSkillCoroutine()
    // {
    //     yield return null;
    //     
    //     var curState = _animator.GetCurrentAnimatorStateInfo(0);
    //     
    //     while (1.0 > curState.normalizedTime)
    //     {
    //         yield return null;
    //     }
    // }
    private void FixedUpdate()
    {
        
        healthBar.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 2f));
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = Move_Input.ReadValue<Vector2>();

        if (!canMove)
        {
            moveValue = Vector2.zero;
        }
        
        if (moveValue.x != 0)
            _spriteRenderer.flipX = moveValue.x < 0;
        
        _animator.SetFloat(Speed1, Mathf.Abs(moveValue.x));
        _rigidbody.velocity = new Vector2(moveValue.x * Speed, _rigidbody.velocity.y);
        
        if (Jump_Input.triggered && Grounded)
        {
            Debug.Log("Jump");
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, 1 << LayerMask.NameToLayer("Tile"));
            
            if (hit.distance <= jumpTestValue)
            {
                _rigidbody.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
                _animator.Play("Alchemist_Jump");
                StartCoroutine(JumpEndCheck());
            }
        }
    }

    IEnumerator JumpEndCheck()
    {
        Grounded = false;
        yield return new WaitForFixedUpdate();
        
        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, 1 << LayerMask.NameToLayer("Tile"));
            
            if (hit.distance <= jumpTestValue)
            {
                _animator.CrossFade("Idles",0.0f);
                break;
            }

            yield return null;
        }

        Grounded = true;
    }
    
    private void LateUpdate()
    {
        var CharPosition = transform.position + cameraOffset;
        float speed = CameraSpeed;

        Vector3 newPosition = Vector3.zero;
        
        if (Vector3.Distance(CharPosition, _mainCamera.transform.position) >= MaxDistence)
        {
            Vector3 Gap = ((_mainCamera.transform.position) - CharPosition).normalized * MaxDistence;
            newPosition = CharPosition + Gap;
        }
        else
        {
            newPosition = Vector3.MoveTowards(_mainCamera.transform.position, 
                CharPosition, 
                speed * Time.deltaTime);
        }

        _mainCamera.transform.position = newPosition;
    }
}
