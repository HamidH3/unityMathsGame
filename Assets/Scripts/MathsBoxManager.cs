using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathsBoxManager : MonoBehaviour
{
    public GameObject MathsBox1;
    public GameObject MathsBox2;
    public GameObject MathsBox3;

    private int randomNumber;
    // Start is called before the first frame update

    public void Start()
    {
        MathsBox1.SetActive(false);
        MathsBox2.SetActive(false);
        MathsBox3.SetActive(false);
        RandomLocation();
    }

    public void RandomLocation()
    {
        randomNumber = Random.Range(0, 3);
        Debug.Log(randomNumber);
        CheckValid();
        if (randomNumber == 0)
        {
            MathsBox1.SetActive(true);
            MathsBox2.SetActive(false);
            MathsBox3.SetActive(false);
        }
        if (randomNumber == 1)
        {
            MathsBox1.SetActive(false);
            MathsBox2.SetActive(true);
            MathsBox3.SetActive(false);
        }
        if (randomNumber == 2)
        {
            MathsBox1.SetActive(false);
            MathsBox2.SetActive(false);
            MathsBox3.SetActive(true);
        }     
    }

    public void CheckValid()
    {
        
        if ((randomNumber == 0) && (MathsBox1.activeSelf)) {

            RandomLocation();
        }
        else if ((randomNumber == 1) && (MathsBox2.activeSelf))
        {
            RandomLocation();
        }
        else if ((randomNumber == 2) && (MathsBox3.activeSelf))
        {
            RandomLocation();
        }

    }

}
