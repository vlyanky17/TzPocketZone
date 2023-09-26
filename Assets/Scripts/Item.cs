using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Item : MonoBehaviour
{
    public ItemType ItemType;
    public int Count;
    public Sprite Image;
    [SerializeField] private Inventory _inventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            _inventory.AddItem(this);
        }
    }

    public Item(Sprite image, int count, ItemType itemType)
    {
        Image= image;
        Count= count;
        ItemType= itemType;
    }
}


public enum ItemType
{
    None,
    Bullet
}