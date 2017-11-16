﻿using DataManagement;

using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    private SceneManager _sceneManager;
    private GameManager _gameManager;

    [Header("Characters."), SerializeField]
    private List<Character> _characters = new List<Character>();
    private int _characterID;

    [Header("UI Elements - Characters.")]
    [SerializeField]
    private GameObject _characterField;
    [SerializeField] private Image _icon;
    [SerializeField] private Text _title;

    [Header("UI Elements - Create.")]
    [SerializeField]
    private GameObject _createField;
    [SerializeField] private InputField _inputName;
    [SerializeField] private Dropdown _classSelect;

    [Header("Prefab.")]
    public Character current;

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _sceneManager = SceneManager.Instance;
        _gameManager = GameManager.Instance;

        if (_gameManager.CurrentAccount.SaveData.ids.Count == 0) return;

        _characters.Clear();

        for (var i = 0; i < _gameManager.CurrentAccount.SaveData.ids.Count; i++)
            _characters.Add(_gameManager.CurrentAccount.FindElement<Character>(i));

        current = _characters[0];
        _title.text = current.Name;
        _icon.sprite = current.FindElementsOfType<Class>()[0].Icon;

        if (current == null)
            _characterField.SetActive(false);

        else _characterField.SetActive(true);

    }

    public Character GetCharacter(int p_id)
    {
        if (p_id <= _characters.Count)
            return _characters[p_id];

        return null;
    }

    public void Next()
    {
        _characterID++;
        if (_characterID > _characters.Count - 1)
            _characterID = 0;

        _icon.sprite = _characters[_characterID].FindElementsOfType<Class>()[0].Icon;
        _title.text = _characters[_characterID].Name;
    }

    public void Previous()
    {
        _characterID--;
        if (_characterID < 0)
            _characterID = _characters.Count - 1;

        _icon.sprite = _characters[_characterID].FindElementsOfType<Class>()[0].Icon;
        _title.text = _characters[_characterID].Name;
    }

    public void Select()
    {
        _characters[_characterID].Select();
    }

    public void Create()
    {
        _gameManager = GameManager.Instance;

        _characterField.SetActive(false);
        _createField.SetActive(true);

        for (var i = 0; i < _sceneManager.DataReferences.SaveData.ids.Count; i++)
        {
            if (_inputName.text != "" && _sceneManager.DataReferences.FindElement<Character>(_inputName.text.ToUpper()) == null)
            {
                _sceneManager.DataReferences.FindElement<Account>(_gameManager.CurrentAccount.ID).AddElement<Character>(_inputName.text.ToUpper());

                var t_character = _sceneManager.DataReferences.FindElement<Account>(_gameManager.CurrentAccount.ID).FindElement<Character>(_inputName.text.ToUpper());
                t_character.Name = _inputName.text;

                t_character.AddElement<Class>(_sceneManager.DataReferences.FindDataElement<Class>(_classSelect.value).ID);

                var t_class = t_character.FindElement<Class>(_sceneManager.DataReferences.FindDataElement<Class>(_classSelect.value).ID);
                t_class.Icon = _sceneManager.DataReferences.FindDataElement<Class>(_classSelect.value).Icon;
                t_class.Save();

                t_character.Save();


                _characterField.SetActive(true);
                _createField.SetActive(false);

                Init();

                _inputName.text = "";
            }
            else return;
        }
    }
}
