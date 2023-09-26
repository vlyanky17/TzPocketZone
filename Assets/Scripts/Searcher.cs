using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Searcher : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            _player.SetContact(enemy);
            enemy.SetContact(_player);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            _player.ClearContact();
            enemy.ClearContact();
        }
    }
}
