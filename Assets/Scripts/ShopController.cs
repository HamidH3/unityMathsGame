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
    //health buttons
    private int healthToBuy = 0;
    private const int healthIncrease = 5;
    private const int healthPointCost = 1; //1 cost for 5 health points
    
    public TMP_Text healthCounterText;
    public Narrator narrator;



    public QuestionGenerator questionGenerator;
    //public GameObject keyIcon;
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
        narrator.ShowMessage("Selected: +5 Health for 1 point. Click 'Buy' to confirm");
    }
    public void SelectFuel() {
        selectedItem = ShopItem.Fuel;
        narrator.ShowMessage("Selected: 1 bar of Fuel for 5 points. Click 'Buy' to confirm");
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

        //int totalHealth = questionGenerator.health + (healthToBuy + 1) * healthIncrease;

        //if (questionGenerator.points > healthToBuy && totalHealth <= questionGenerator.maxHealth)
        //{
            healthToBuy++;
            UpdateHealthCounter();
        //}
        //else
        //{
        //    ShowMessage("Can't add more — not enough points or would exceed max health.");
        //}
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

    //public void TryBuyHealth()
    //{
    //    if (questionGenerator == null || healthToBuy == 0)
    //    {
    //        ShowMessage("You must choose how much health to buy!");
    //        return;
    //    }

    //    int totalCost = healthToBuy;
    //    int totalHealth = healthToBuy * healthIncrease;
    //    Debug.LogError("totalHealth" + totalHealth);
    //    Debug.LogError("totalCost" + totalCost);

    //    if (questionGenerator.points < totalCost)
    //    //&& questionGenerator.health + totalHealth <= questionGenerator.maxHealth
    //    {
    //        ShowMessage("Not Enough Points, Sorry");
    //        return;
    //    }
    //    if (questionGenerator.health == questionGenerator.maxHealth)
    //    {
    //        ShowMessage("You have enough health! Come back later");
    //        return;
    //    }

    //    //else if (questionGenerator.points < 1 && questionGenerator.health + totalHealth > 45)
    //    //{
    //    //    ShowMessage("Not Enough Points, and you have enough health!");
    //    //}

    //    //if (questionGenerator.points >= totalCost && questionGenerator.health + totalHealth <= questionGenerator.maxHealth)
    //    //{
    //    //if(question)
    //    questionGenerator.health += totalHealth;
    //    questionGenerator.points -= totalCost;
    //    if (questionGenerator.health > 50)
    //    {
    //        questionGenerator.health = 50;
    //    }
    //    questionGenerator.UpdateMainScreenOverlay();
    //    ShowMessage($"You bought {totalHealth} Health for {totalCost} points!");
    //    healthToBuy = 0;
    //    UpdateHealthCounter();
    //}

    ////else if (questionGenerator.health + totalHealth > 45 && questionGenerator.points >= 1)
    ////{
    ////    ShowMessage("You have enough health! Come back later");
    ////}
    ////else if (questionGenerator.heaxlth + totalHealth > 45 && questionGenerator.points < 1)
    ////{
    ////    ShowMessage("You have enough health! Come back later");
    ////}

    public void TryBuyHealth()
    {
        if (questionGenerator == null || healthToBuy == 0)
        {
            narrator.ShowMessage("You must choose how much health to buy!");
            return;
        }

        int totalHealth = healthToBuy * healthIncrease;
        int missingHealth = questionGenerator.maxHealth - questionGenerator.health;

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

        questionGenerator.UpdateMainScreenOverlay();
        narrator.ShowMessage($"You bought {actualHealthToAdd} health for {actualPointsToSpend} point(s).");

        healthToBuy = 0;
        UpdateHealthCounter();
    }


    public void TryBuyFuel()
    {
        //int fuelCost = 5;
        //int maxFuelBar = 3;

        if (fuel >= maxFuelBar)
        {
            narrator.ShowMessage("Fuel tank is already full!");
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
        } 

        narrator.ShowMessage($"Purchased 1 fuel bar for {fuelCost} points. Current fuel: {fuel}/{maxFuelBar}");
    }



    private void UpdateHealthCounter()
    {
        string msg = $"{healthToBuy}";
        if (healthCounterText != null)
        {
            healthCounterText.text = msg;
        }
        
    }

    
    //private void ShowMessage(string message)
    //{
    //    if (shopMessageText != null)
    //    {
    //        if (messageCoroutine != null)
    //        {
    //            StopCoroutine(messageCoroutine);
    //        }
    //        messageCoroutine = StartCoroutine(TypeWriterEffect(message));
    //    }

    //}
    //private IEnumerator TypeWriterEffect(string msg)
    //{
    //    shopMessageText.text = "";
    //    foreach (char c in msg)
    //    {
    //        shopMessageText.text += c;
    //        yield return new WaitForSeconds(0.04f);
    //    }
    //    messageCoroutine = null;
    //}



    void Start()
    {
        questionGenerator.UpdateKeyImageColour();
        UpdateHealthCounter();
    }



}
