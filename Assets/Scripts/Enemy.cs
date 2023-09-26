using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Gameplay _gameplay;
    [SerializeField] private Slider _hpBar;
    [SerializeField] private Item _itemToDrop;
    [SerializeField] private Rigidbody2D _rigitBody;
    [SerializeField] private Transform _firstLeg;
    [SerializeField] private Transform _secondLeg;

    public float MAXHP;
    public float Speed;
    public float ActualHP { get; private set; }
    private Player _contact;
    private bool _runToPlayer;
    private bool _onCoroutine;
    private bool _contactInHitbox;

    private void Awake()
    {
        ActualHP = MAXHP;
    }

    public void ReceiveDamage(float damage)
    {
        ActualHP = ActualHP - damage;
        UpdateHpBar();
        if (ActualHP<=0)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if (_runToPlayer && _contact!= null)
        {
            Vector2 runTo = new Vector2((_contact.transform.position.x-transform.position.x) * Speed, (_contact.transform.position.y-transform.position.y)*Speed );
            _rigitBody.velocity = runTo;
            if (!_onCoroutine)
                StartCoroutine(RunCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            _runToPlayer = false;
            _rigitBody.velocity = new Vector2(0, 0);
            _contactInHitbox = true;
            StartCoroutine(HitCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            _runToPlayer = true;
            _contactInHitbox = false;
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        _itemToDrop.transform.localPosition = Vector3.zero;
        _itemToDrop.transform.SetParent(null);
        _itemToDrop.gameObject.SetActive(true);
    }

    private void UpdateHpBar()
    {
        _hpBar.value = ActualHP / MAXHP;
    }

    public void SetContact(Player player)
    {
        _contact = player;
        _runToPlayer = true;
    }

    public void ClearContact()
    {
        _contact = null;
        _runToPlayer = false;
        _rigitBody.velocity = new Vector2(0,0);
    }

    private IEnumerator HitCoroutine()
    {
        _contact.ReceiveDamage(_gameplay.EnemyDamage);
        yield return new WaitForSeconds(0.5f);
        if (_contactInHitbox)
        {
            StartCoroutine(HitCoroutine());
        }
    }

    private IEnumerator RunCoroutine()
    {
        _onCoroutine = true;
        _firstLeg.rotation = Quaternion.Euler(0, 0, 9);
        _secondLeg.rotation = Quaternion.Euler(0, 0, -9);
        yield return new WaitForSeconds(0.5f);
        _firstLeg.rotation = Quaternion.Euler(0, 0, -9);
        _secondLeg.rotation = Quaternion.Euler(0, 0, 9);
        yield return new WaitForSeconds(0.5f);
        if (_runToPlayer)
        {
            StartCoroutine(RunCoroutine());
        }
    }
}
