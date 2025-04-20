using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class ShopController : MonoBehaviour
{
    public TMP_Text shopMessageText;
    private Coroutine messageCoroutine;

    //health buttons
    private int healthToBuy = 0;
    private const int healthIncrease = 5;
    private const int healthPointCost = 1; //1 cost for 5 health points
    public TMP_Text healthCounterText;


    public QuestionGenerator questionGenerator;
    //public GameObject keyIcon;
    // Start is called before the first frame update

    private enum ShopItem
    {
        None,
        Health,
        Key
    }

    private ShopItem selectedItem = ShopItem.None;

    public void SelectHealth()
    {
        selectedItem = ShopItem.Health;
        ShowMessage("Selected: +5 Health for 1 point. Click 'Buy' to confirm");
    }
    public void SelectKey() {
        selectedItem = ShopItem.Key;
        ShowMessage("Selected: Key for 10 points. Click 'Buy' to confirm");
    }

    public void BuyToConfirm()
    {
        switch (selectedItem) {
            case ShopItem.None:
                ShowMessage("Please select an item before buying!");
                break;
            case ShopItem.Health:
                TryBuyHealth();
                break;
            case ShopItem.Key:
                TryBuyKey();
                break;
        }
        selectedItem = ShopItem.None;

    }

    public void IncreaseHealth()
    {
        if (questionGenerator == null) return;

        int totalHealth = questionGenerator.health + (healthToBuy + 1) * healthIncrease;

        if (questionGenerator.points > healthToBuy && totalHealth <= questionGenerator.maxHealth)
        {
            healthToBuy++;
            //UpdateHealthCounter();
        }
        else
        {
            ShowMessage("Can't add more — not enough points or would exceed max health.");
        }
    }

    public void DecreaseHealth()
    {
        if (healthToBuy > 0)
        {
            healthToBuy--;
            //UpdateHealthCounter();

        }
        else
        {
            ShowMessage("Health counter is already 0!");
        }
    }

    public void TryBuyHealth()
    {
        if (questionGenerator == null) return;

        if (questionGenerator.points >= 1 && questionGenerator.health <= 45)
        {
            questionGenerator.health += 5;
            questionGenerator.points -= 1;
            questionGenerator.UpdateMainScreenOverlay();
            ShowMessage("Health added");
        }
        else if (questionGenerator.points >= 1 && questionGenerator.health > 45)
        {
            ShowMessage("You have enough health! Come back later");
        }
        else if (questionGenerator.points < 1)
        {
            ShowMessage("Not Enough Points, Sorry");
        }
    }

    public void TryBuyKey()
    {
        if (questionGenerator.HasKey)
        {
            ShowMessage("You already have the key");
            questionGenerator.UpdateKeyImageColour();
            return;
        }

        if (questionGenerator.points >= 2)
        {
            questionGenerator.points -= 2;
            questionGenerator.HasKeyValue();
            questionGenerator.UpdateMainScreenOverlay();

            ShowMessage("Key Purchased");
            questionGenerator.UpdateKeyImageColour();

        }
        else
        {
            ShowMessage("You need at least 10 points");
        }
    }

    
    private void ShowMessage(string message)
    {
        if (shopMessageText != null)
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }
            messageCoroutine = StartCoroutine(TypeWriterEffect(message));
        }

    }
    private IEnumerator TypeWriterEffect(string msg)
    {
        shopMessageText.text = "";
        foreach (char c in msg)
        {
            shopMessageText.text += c;
            yield return new WaitForSeconds(0.04f);
        }
        messageCoroutine = null;
    }



    void Start()
    {
        questionGenerator.UpdateKeyImageColour();
    }



}
