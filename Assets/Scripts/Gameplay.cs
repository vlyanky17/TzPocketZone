using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Player _player;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _fireButton;
    [SerializeField] private TextMeshProUGUI _noEnemyText;
    [SerializeField] private TextMeshProUGUI _noAmmoText;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _dieScreen;
    [SerializeField] private int _leftEmenySpawnBorder;
    [SerializeField] private int _rightEmenySpawnBorder;
    [SerializeField] private int _upEmenySpawnBorder;
    [SerializeField] private int _downEmenySpawnBorder;


    public float PlayerDamage;
    public float EnemyDamage;
    public Enemy[] _enemies;
    public Item[] _exampleItems;

    private List<ItemToSave> _itemToSaves =  new List<ItemToSave>();
    private bool _canFire;
    private string Path;
    private Color32 _noFireColor = new Color32(248,172,172,255);
    private Color32 _fireColor = new Color32(175, 248, 172, 255);

    private void OnDisable()
    {
        _inventoryButton.onClick.RemoveAllListeners();
        _fireButton.onClick.RemoveAllListeners();
    }
    private void OnEnable()
    {
        _inventoryButton.onClick.AddListener(() => _inventory.gameObject.SetActive(true));
        _fireButton.onClick.AddListener(Fire);
    }

    public void SetItemList(List<ItemToSave> list)
    {
        _itemToSaves = list;
        Save();
    }

    public void Save()
    {
        var jsonString = JsonConvert.SerializeObject(new SessionData(_player.ActualHP, _itemToSaves));
          using StreamWriter writer= new StreamWriter(Path);
          writer.Write(jsonString);
        writer.Close();
    }

    private void Load()
    {
        using StreamReader reader = new StreamReader(Path);
        var jsonString = reader.ReadToEnd();
        reader.Close();
        if (jsonString.Length > 0)
        {
            var session = JsonConvert.DeserializeObject<SessionData>(jsonString);
            _player.SetHp(session.PlayerHp);
            for (int i = 0; i < session.ItemsInInventory.Count; i++)
            {
                for (int j = 0; j < _exampleItems.Length; j++)
                {
                    if (_exampleItems[j].ItemType == session.ItemsInInventory[i].Type)
                    {
                        _exampleItems[j].Count = session.ItemsInInventory[i].Count;
                        _inventory.AddItemToCell(_exampleItems[j]);
                        break;
                    }
                }
                       
            }
        }
    }

    private void Awake()
    {
        Path = Application.persistentDataPath + "saveData.json";
        GenerateEnemyPlace();
        Load();
    }

    public void CallDieScreen()
    {
        _dieScreen.SetActive(true);
    }

    public void CheckFire()
    {
        _canFire = true;
        if (_player.Contact==null)
        {
            _canFire = false;
            _noEnemyText.gameObject.SetActive(true);
        }
        else _noEnemyText.gameObject.SetActive(false);
        if (!_inventory.HasAmmo())
        {
            _canFire = false;
            _noAmmoText.gameObject.SetActive(true);
        } else _noAmmoText.gameObject.SetActive(false);

        if (_canFire)
        {
            _image.color = _fireColor;
        }
        else _image.color = _noFireColor;
    }

    private void GenerateEnemyPlace()
    {
        for (int i = 0; i < _enemies.Length; i++)
        {
            float addX=  Random.Range(_leftEmenySpawnBorder, _rightEmenySpawnBorder);
            float addY = Random.Range(_downEmenySpawnBorder, _upEmenySpawnBorder);
            _enemies[i].transform.position = new Vector3(addX,-addY,0);
        }
    }

    private void Fire()
    {
        if (_canFire)
        {
            _inventory.RemoveItem(ItemType.Bullet,1);
            _player.Contact.ReceiveDamage(PlayerDamage);
            CheckFire();
        }
    }

    public class SessionData
    {
        public float PlayerHp;
        public List<ItemToSave> ItemsInInventory;

        public SessionData(float Hp, List<ItemToSave> items)
        {
            PlayerHp = Hp;
            ItemsInInventory = items;
        }
    }
    public struct ItemToSave
    {
        public ItemType Type;
        public int Count;
    }
}
