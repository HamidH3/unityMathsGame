using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class ShopController : MonoBehaviour
{
    public TMP_Text shopMessageText;
    private Coroutine messageCoroutine;
    public int fuel = 0;
    public int maxFuelBar = 3;
    public int fuelCost = 5;
    public bool hasEnoughFuel = false;
    public bool HasEnoughFuel => hasEnoughFuel;
    //health buttons
    private int healthToBuy = 0;
    private const int healthIncrease = 5;
    private const int healthPointCost = 1; //1 cost for 5 health points
    
    public TMP_Text healthCounterText;
    public Narrator narrator;



    public QuestionGenerator questionGenerator;
    // Start is called before the first frame update

    private enum ShopItem
    {
        None,
        Health,
        Fuel
    }

    private ShopItem selectedItem = ShopItem.None;

    public void SelectHealth()
    {
        selectedItem = ShopItem.Health;
        narrator.ShowMessage("Selected: +5 Health for 1 point. Click 'Buy' to confirm!");
    }
    public void SelectFuel() {
        selectedItem = ShopItem.Fuel;
        narrator.ShowMessage("Selected: +1 bar of Fuel for 5 points. Click 'Buy' to confirm!");
    }

    public void BuyToConfirm()
    {
        switch (selectedItem) {
            case ShopItem.None:
                narrator.ShowMessage("Please select an item before buying!");
                break;
            case ShopItem.Health:
                TryBuyHealth();
                break;
            case ShopItem.Fuel:
                TryBuyFuel();
                break;
        }
        selectedItem = ShopItem.None;

    }

    public void IncreaseHealth()
    {
        if (questionGenerator == null) return;
        selectedItem = ShopItem.Health;

            healthToBuy++;
            UpdateHealthCounter();

    }

    public void DecreaseHealth()
    {
        selectedItem = ShopItem.Health;
        if (healthToBuy > 0)
        {
            healthToBuy--;
            UpdateHealthCounter();

        }
        else
        {
            narrator.ShowMessage("Health counter is already 0!");
        }
    }


    public void TryBuyHealth()
    {
        if (questionGenerator == null || healthToBuy == 0)
        {
            narrator.ShowMessage("You must choose how much health to buy!");
            return;
        }

        int totalHealth = healthToBuy * healthIncrease;
        int missingHealth = questionGenerator.maxHealth - questionGenerator.health;
        //assuming the player is at full health beause missingHealth calculates the difference between max health and the health you have
        if (missingHealth == 0)
        {
            narrator.ShowMessage("You already have max health!");
            return;
        }
        //automatically detects how much health i need, despite user choosing to spend more
        int actualHealthToAdd = Mathf.Min(totalHealth, missingHealth);

        // Calculate how many points we actually need to spend for this smaller amount
        int actualPointsToSpend = Mathf.CeilToInt((float)actualHealthToAdd / healthIncrease);

        if (questionGenerator.points < actualPointsToSpend)
        {
            narrator.ShowMessage("Not enough points!");
            return;
        }

        // Deduct only what's needed
        questionGenerator.health += actualHealthToAdd;
        questionGenerator.points -= actualPointsToSpend;

        questionGenerator.UpdateHealthBarFill(questionGenerator.health);

        questionGenerator.UpdateMainScreenOverlay();
        narrator.ShowMessage($"You bought {actualHealthToAdd} health for {actualPointsToSpend} point(s).");

        healthToBuy = 0;
        UpdateHealthCounter();
    }
    private void UpdateHealthCounter()
    {
        string msg = $"{healthToBuy}";
        if (healthCounterText != null)
        {
            healthCounterText.text = msg;
        }

    }


    public void TryBuyFuel()
    {


        if (fuel >= maxFuelBar)
        {
            narrator.ShowMessage("Fuel tank is already full!");
            hasEnoughFuel = true;
            return;
        }

        if (questionGenerator.points < fuelCost)
        {
            narrator.ShowMessage("You need at least 5 points to buy 1 fuel bar.");
            return;
        }
        if (questionGenerator.points >= fuelCost && fuel < maxFuelBar)
        {
            fuel += 1;
            questionGenerator.points -= fuelCost;
            if (fuel > maxFuelBar)
            {
                fuel = maxFuelBar;
            }   
            questionGenerator.UpdateFuelBars(fuel);
            questionGenerator.UpdateMainScreenOverlay();
            if (fuel >= maxFuelBar)
            {
                hasEnoughFuel = true;
            }
        } 

        narrator.ShowMessage($"Purchased 1 fuel bar for {fuelCost} points. Current fuel: {fuel}/{maxFuelBar}");
    }




    void Start()
    {
        questionGenerator.UpdateKeyImageColour();
        UpdateHealthCounter();
        questionGenerator.UpdateHealthBarFill(questionGenerator.health);
    }



}
