using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemoveScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private Cell _cell;

    private void OnDisable()
    {
        _yesButton.onClick.RemoveAllListeners();
        _noButton.onClick.RemoveAllListeners();
    }
    private void OnEnable()
    {
        _noButton.onClick.AddListener(() => gameObject.SetActive(false));
        _yesButton.onClick.AddListener(ConfirmRemove);
    }

    public void RemoveItem(Cell cell)
    {
        _cell = cell;
        _text.text = "удалить " + _cell.ItemType.ToString();
    }

    private void ConfirmRemove()
    {
        _cell.ClearCell();
        _cell = null;
        gameObject.SetActive(false);
    }
}
