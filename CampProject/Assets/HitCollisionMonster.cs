using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollisionMonster : MonoBehaviour
{
    private Monster monster;

    void Start()
    {
        monster = GetComponentInParent<Monster>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Skill"))
        {
            monster.TakeDamage(15f);
        }
    }
}
