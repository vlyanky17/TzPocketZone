using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigitBody;
    [SerializeField] private FixedJoystick _joyStick;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _firstLeg;
    [SerializeField] private Transform _secondLeg;
    [SerializeField] private Slider _hpBar;
    [SerializeField] private Gameplay _gameplay;

    public float Speed;
    public float MAXHP;
    private bool _onCoroutine;
    public Enemy Contact { get; private set; }
    public float ActualHP { get; private set; }

    private void Awake()
    {
        ActualHP = MAXHP;
    }

    private void FixedUpdate()
    {
        _rigitBody.MovePosition(new Vector2(_rigitBody.position.x+_joyStick.Horizontal*Speed, _rigitBody.position.y+ _joyStick.Vertical * Speed));
        _camera.transform.position = new Vector3(_rigitBody.position.x, _rigitBody.position.y,-10);
        if (_joyStick.Horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else transform.rotation = Quaternion.Euler(0, 0, 0);

        if (_joyStick.Horizontal != 0 || _joyStick.Vertical != 0)
        {
            if (!_onCoroutine)
            StartCoroutine(RunCoroutine());
        } else
        {
            _firstLeg.rotation = Quaternion.Euler(0, 0, 0);
            _secondLeg.rotation = Quaternion.Euler(0, 0, 0);
            _onCoroutine = false;
        }
    }

    public void SetHp(float hp)
    {
        ActualHP = hp;
        UpdateHpBar();
    }

    public void ReceiveDamage(float damage)
    {
        ActualHP = ActualHP - damage;
        UpdateHpBar();
        if (ActualHP <= 0)
        {
            Die();
        }
        else _gameplay.Save();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        _gameplay.CallDieScreen();
    }

    private void UpdateHpBar()
    {
        _hpBar.value = ActualHP / MAXHP;
    }

    public void SetContact(Enemy enemy)
    {
        Contact = enemy;
        _gameplay.CheckFire();
    }
    public void ClearContact()
    {
        Contact = null;
        _gameplay.CheckFire();
    }


    private IEnumerator RunCoroutine()
    {
        _onCoroutine = true;
        _firstLeg.rotation= Quaternion.Euler(0, 0, 9);
        _secondLeg.rotation = Quaternion.Euler(0, 0, -9);
        yield return new WaitForSeconds(0.5f);
        _firstLeg.rotation = Quaternion.Euler(0, 0, -9);
        _secondLeg.rotation = Quaternion.Euler(0, 0, 9);
        yield return new WaitForSeconds(0.5f);
        if (_joyStick.Horizontal != 0 || _joyStick.Vertical != 0)
        {
            StartCoroutine(RunCoroutine());
        }
    }
}
