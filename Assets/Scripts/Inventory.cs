
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using static Gameplay;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;
    [SerializeField] private Button _button;
    [SerializeField] private Gameplay _gameplay;

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
    private void OnEnable()
    {
        _button.onClick.AddListener(()=>gameObject.SetActive(false));
    }

    public void RemoveItem(ItemType Type, int Count)
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            if (_cells[i].ItemType == Type)
            {
                _cells[i].Count = _cells[i].Count- Count;
                if (_cells[i].Count <= 0)
                {
                    _cells[i].ClearCell();
                }
                else _cells[i].UpdateCell();
                break;
            }
        }
        UpdateInventoryList();
    }

    public bool HasAmmo()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            if (_cells[i].ItemType== ItemType.Bullet && _cells[i].Count>0)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(Item item)
    {
        AddItemToCell(item);
        UpdateInventoryList();
    }

    public void AddItemToCell(Item item)
    {
        Cell EmptyCell = null;
        Cell CurrentCell = null;
        for (int i = 0; i < _cells.Length; i++)
        {
            if (_cells[i].ItemType == item.ItemType)
            {
                CurrentCell= _cells[i];
                break;
            }
            if (_cells[i].ItemType == ItemType.None && EmptyCell== null)
            {
                EmptyCell = _cells[i];
            }
        }
        if (CurrentCell != null)
        { 
            CurrentCell.AddItem(item);
            item.gameObject.SetActive(false);
        }
        else if (EmptyCell != null)
        {
            EmptyCell.AddItem(item);
            item.gameObject.SetActive(false);
        }
        _gameplay.CheckFire();
    }

    public void UpdateInventoryList()
    {
        List<ItemToSave> inventoryList= new List<ItemToSave>();
        for (int i = 0; i < _cells.Length; i++)
        {
            if (_cells[i].ItemType != ItemType.None)
            {
                ItemToSave item;
                item.Type = _cells[i].ItemType; 
                item.Count= _cells[i].Count; 
                inventoryList.Add(item);
            }
        }
        _gameplay.SetItemList(inventoryList);
    }
}
