using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HitCollision : MonoBehaviour
{
    Rigidbody2D rigid;

    private Vector3 knockBackVelocity;
    public float minVelocity = 0.1f;
    public float decelerationRate = 5f;
    public float knockBackSpeed = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 backPosition =  rigid.transform.position - other.transform.position;
        backPosition.Normalize();
        Knockback(backPosition, knockBackSpeed);
    }

    void Knockback(Vector2 direction, float knockBackPower)
    {
        StartCoroutine(KnockbackCoroutine(direction, knockBackPower));
    }

    IEnumerator KnockbackCoroutine(Vector2 direction, float knockbackPower)
    {
        knockBackVelocity = direction.normalized * knockbackPower;

        while (knockBackVelocity.magnitude > minVelocity)
        {
            // 점점 더 천천히 감속
            knockBackVelocity *= (1f - Time.deltaTime * decelerationRate);
            
            // Transform으로 이동 적용
            rigid.transform.position += knockBackVelocity * Time.deltaTime;

            yield return new WaitForFixedUpdate();  
        }
        
        knockBackVelocity = Vector2.zero;
        
    }
}
