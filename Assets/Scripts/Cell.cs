using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;
    [SerializeField] private RemoveScreen _removeScreen;
    [SerializeField] private Inventory _inventory;
    public ItemType ItemType  { get; private set; }
    public int Count;

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
    private void OnEnable()
    {
        _button.onClick.AddListener(AskRemove);
    }

    private void AskRemove()
    {
        if (ItemType != ItemType.None)
        {
            _removeScreen.gameObject.SetActive(true);
            _removeScreen.RemoveItem(this);
        }
    }

    public void UpdateCell()
    {
        if (Count > 1)
        {
            _text.text = Count.ToString();
            _text.gameObject.SetActive(true);
        }
        else _text.gameObject.SetActive(false);

        if (_image.sprite != null)
        {
            _image.gameObject.SetActive(true);
        }
        else _image.gameObject.SetActive(false);


    }

    public void AddItem(Item item)
    {
        ItemType = item.ItemType;
        Count = Count + item.Count;
        _image.sprite = item.Image;
        UpdateCell();

    }

    public void ClearCell()
    {
        ItemType = ItemType.None;
        Count = 0;
        _image.sprite = null;
        _image.gameObject.SetActive(false);
        _text.text = "";
        _text.gameObject.SetActive(false);
        _inventory.UpdateInventoryList();
    }
}
