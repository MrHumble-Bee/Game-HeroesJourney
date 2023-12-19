using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Coin : MonoBehaviour
{
    public AudioSource coinPickup;
    public int lowerBoundValue = 1;
    public int upperBoundValue = 5;

    public TMP_Text coinText;
    public GameObject coin;

    void Start()
    {
        // coinText = GetComponent<TMP_Text>();
        coinText.enabled = false; // Initially, hide the text
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            coinPickup.Play();
            // Invoke("Collected", 0.125f);
            Collected();
            Invoke("DelayedDestroy", 1.0f);
            Hero hero = other.gameObject.GetComponent<Hero>();
            int value = Random.Range(lowerBoundValue, upperBoundValue);
            hero.coins += value;
            ShowCoinValue(value);
        }
    }

    private void Collected()
    {
        Destroy(coin);
    }

    private void DelayedDestroy()
    {
        Destroy(gameObject);
    }

    public void ShowCoinValue(int value)
    {
        coinText.SetText($"+{value} coins");
        coinText.enabled = true;

        // You can add animations or other effects here if needed

        Invoke("HideCoinValue", 2f); // Hide the text after 2 seconds
    }

    void HideCoinValue()
    {
        coinText.enabled = false;
    }

    
}
