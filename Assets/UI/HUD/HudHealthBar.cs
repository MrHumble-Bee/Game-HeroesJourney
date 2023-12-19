using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudHealthBar : MonoBehaviour
{

	public Slider hpSlider;
	public TMP_Text hpIndicator;
    
    public Slider expSlider;
    public TMP_Text expIndicator;

    public TMP_Text statsIndicator;

    public TMP_Text coinsIndicator;
	
    public Hero unit;

    public Camera miniMapCamera;  // Assign your mini-map camera in the Inspector
    public RawImage miniMapImage; // Assign your Raw Image component in the Inspector

    void Start()
    {
        if (miniMapCamera != null && miniMapImage != null)
        {
            RenderTexture renderTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
            miniMapCamera.targetTexture = renderTexture;
            miniMapImage.texture = renderTexture;
        }
        else
        {
            Debug.LogError("Mini-map setup is incomplete. Assign miniMapCamera and miniMapImage.");
        }
    }

	void Update()
	{
		SetMaxHealth(unit.maxHitPoints);
		SetHealth(unit.currentHitPoints);
        SetMaxExp(unit.nextLevelExperience);
        SetExp(unit.currentExperience);
        SetStats(unit);
        SetCoins(unit);
	}
	
    public void SetMaxExp(float expNeeded)
    {
        expSlider.maxValue = expNeeded;
    }

    public void SetExp(float currentExp)
    {
        expSlider.value = currentExp;
        expIndicator.SetText($"Experience:  {(int)currentExp} / {(int)unit.nextLevelExperience}");
    }

	public void SetMaxHealth(float health)
	{
		hpSlider.maxValue = health;
	}

    public void SetHealth(float health)
	{
		hpSlider.value = health;
        hpIndicator.SetText($"Health: {unit.currentHitPoints} / {unit.maxHitPoints}");
	}

    public void SetStats(Hero unit)
    {
        statsIndicator.SetText($"Stats\nATK: {unit.currentAttackPoints}\nDEF: {unit.currentDefense}\nSPD: {unit.currentSpeed}\nCRIT: \nCRIT DMG: \nATK SPD: ");
    }

    public void SetCoins(Hero hero)
    {
        coinsIndicator.SetText($"Coins:  {hero.coins}");
    }

}
