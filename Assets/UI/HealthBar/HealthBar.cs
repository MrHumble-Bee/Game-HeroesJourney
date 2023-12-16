using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

	public Slider slider;
	// public Gradient gradient;
	public Image fill;
	public TMP_Text levelIndicator;
	public Unit unit;

	void Start()
	{
		SetMaxHealth(unit.maxHitPoints);
	}

	void Update()
	{
		SetMaxHealth(unit.maxHitPoints);
		SetHealth(unit.currentHitPoints);
		SetLevel(unit.level);
	}
	
	public void SetLevel(int level)
	{
		// levelIndicator.text = $"{level}";
		levelIndicator.SetText($"{level}");
		
	}

	public void SetMaxHealth(float health)
	{
		slider.maxValue = health;
	}

    public void SetHealth(float health)
	{
		slider.value = health;

		// fill.color = gradient.Evaluate(slider.normalizedValue);
	}

}
