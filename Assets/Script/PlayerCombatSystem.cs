using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCombatSystem : MonoBehaviour
{
    private Animator _animator;
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public int attackDamage = 40;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    private float _nextAttackTime = 0f;
    private int noOfClick = 0;
    private GameObject _attackPointParent;

    private void Start()
    {
        _attackPointParent = Instantiate(attackPoint.gameObject,transform.position,quaternion.identity,transform);
        attackPoint.SetParent(_attackPointParent.transform);
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log(noOfClick);
        if (Input.GetAxisRaw("Horizontal")  != 0)
            _attackPointParent.transform.right = new Vector3(Input.GetAxisRaw("Horizontal") , 0, 0);
        
        if (Time.time < _nextAttackTime || !Input.GetKeyDown(KeyCode.Mouse0)) return;
        
        Attack();
        _nextAttackTime = Time.time + (1f / attackRate);
    }

    void Attack()
    {
        // flip attack point
        if (noOfClick == 0)
        {
            _animator.SetTrigger("Attack1");
        }
        if (noOfClick == 1)
        {
            _animator.SetTrigger("Attack2");
        }
        if (noOfClick == 2)
        {
            _animator.SetTrigger("Attack3");
            noOfClick = -1;
        }
        noOfClick++;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayer);
        
        foreach (Collider2D enemy in hitEnemies) 
            enemy.GetComponent<HealthSystem>().TakeDamage(attackDamage);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
}
