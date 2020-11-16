using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int _health;

    [Space] public Sprite fullHeart, emptyHeart;
    [Space] public GameObject heartsWrapper;
    public GameObject heartPrefab;

    private readonly List<Image> _heartSprites = new List<Image>();

    private void Start()
    {
        _health = GetComponent<PlayerStats>().health;
        BuildUI();
    }

    private void Update()
    {
        UpdateUI();
    }
    
    private void BuildUI()
    {
        //Change the Width of the Heart Wrapper to fit all the hearts
        var spriteWidth = heartPrefab.GetComponent<Image>().sprite.rect.width;
        heartsWrapper.GetComponent<RectTransform>().sizeDelta = new Vector2(_health * spriteWidth * 1.5f, spriteWidth);

        //Add a Heart to the Wrapper and store the Image Component of every Heart
        for (var i = 0; i < _health; i++)
        {
            Instantiate(heartPrefab, heartsWrapper.transform);
            _heartSprites.Add(heartsWrapper.transform.GetChild(i).GetComponent<Image>());
        }
    }

    private void UpdateUI()
    {
        //Update the Sprites to reflect Player Health
        for (var i = 0; i < _heartSprites.Count; i++)
        {
            _heartSprites[i].sprite = i < _health ? fullHeart : emptyHeart;
        }
    }

    public int GetHealth() => _health;

    public void Damage(int amount)
    {
        if (_health > 0)
        {
            _health -= amount;
        }
    }

    public void Heal(int amount)
    {
        _health += amount;
    }
}