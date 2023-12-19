using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

	public Slider slider;
	public Image fill;
	public TMP_Text levelIndicator;
	private Unit unit;

	void Start()
	{
		unit = FindParentUnit();
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
	}

	private Unit FindParentUnit()
    {
        Transform parentTransform = transform.parent;

        while (parentTransform != null)
        {
            Unit parentUnit = parentTransform.GetComponent<Unit>();
            if (parentUnit != null)
            {
                return parentUnit;
            }

            // Move up to the next parent in the hierarchy
            parentTransform = parentTransform.parent;
        }

        return null; // If no parent Unit is found
    }

}
