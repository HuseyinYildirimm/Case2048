using UnityEngine;
using UnityEngine.UI;

public class ChangingState : MonoBehaviour
{
    [System.Serializable]
    public struct NumberPercentagePair
    {
        public int number;
        public float percentage;
        public Sprite numberImage;
    }

    public enum States { State0, State1, State2, State3 }
    public States currentState;
    public NumberPercentagePair[] numberPercentagePairs;
    private States previousState;
    public int selectedNumber;
    public int selectedIndex;

    private void Start()
    {
        previousState = currentState;
        UpdatePercentages();
        RandomNumber();
    }

    public void Update()
    {
        if (currentState != previousState)
        {
            previousState = currentState;
            UpdatePercentages();
        }
    }

    public void RandomNumber()
    {
        float totalPercentage = 0f;
        foreach (NumberPercentagePair pair in numberPercentagePairs)
        {
            totalPercentage += pair.percentage;
        }

        float randomValue = Random.Range(0f, totalPercentage);

        selectedNumber = -1;
        float accumulatedPercentage = 0f;
        foreach (NumberPercentagePair pair in numberPercentagePairs)
        {
            accumulatedPercentage += pair.percentage;
            if (randomValue < accumulatedPercentage)
            {
                selectedNumber = pair.number;
                break;
            }
        }
        for (int i = 0; i < numberPercentagePairs.Length; i++)
        {
            if (numberPercentagePairs[i].number == selectedNumber)
            {
                selectedIndex = i; 
                break; 
            }
        }
     
    }

    private void UpdatePercentages()
    {
        switch (currentState)
        {
            case States.State0:
                SetPercentage(10, 10, 20, 20, 20, 20, 0, 0);
                break;
            case States.State1:
                SetPercentage(5, 8, 15, 20, 30, 20, 2, 0);
                break;
            case States.State2:
                SetPercentage(1, 2, 15, 20, 30, 25, 5, 2);
                break;
            case States.State3:
                SetPercentage(1, 2, 10, 20, 27, 25, 10, 5);
                break;
            default:
                break;
        }
    }

    private void SetPercentage(params float[] percentages)
    {
        if (percentages.Length != numberPercentagePairs.Length)
        {
            Debug.LogError("Number of percentages doesn't match the number of pairs.");
            return;
        }

        for (int i = 0; i < percentages.Length; i++)
        {
            numberPercentagePairs[i].percentage = percentages[i];
        }
    }
}
