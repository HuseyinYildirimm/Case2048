using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ChangingState;

public class NumberInteraction : MonoBehaviour
{
    [SerializeField] private Image previewNumber;
   
    private float singleNumber;
    private float doubleNumber;
    private float quadrupleNumber;
    private ChangingState changingState;
    private InputControl inputControl;

    public void Start()
    {
        changingState = GetComponent<ChangingState>();
        inputControl = GetComponent<InputControl>();

        EventTrigger eventTrigger = previewNumber.GetComponent<EventTrigger>();
        Component.Destroy(eventTrigger);
    }
    public void Update()
    {
        foreach (NumberPercentagePair pair in changingState.numberPercentagePairs)
        {
            if (changingState.selectedNumber == pair.number)
            {
                previewNumber.sprite = pair.numberImage;
                singleNumber = pair.number;
                doubleNumber = singleNumber / 2;
                quadrupleNumber = doubleNumber / 2;
            }
        }

        if(inputControl.highlightedButtons.Count == 1)
        {
            inputControl.highlightedButtons[0].image.sprite = previewNumber.sprite;
        }
        else if(inputControl.highlightedButtons.Count == 2 && singleNumber != 2)
        {
            inputControl.highlightedButtons[0].image.sprite = changingState.numberPercentagePairs[changingState.selectedIndex - 1].numberImage;
            inputControl.highlightedButtons[1].image.sprite = changingState.numberPercentagePairs[changingState.selectedIndex - 1].numberImage;

        }
        else if (inputControl.highlightedButtons.Count == 3 && singleNumber !=2 && singleNumber !=4)
        {
            inputControl.highlightedButtons[0].image.sprite = changingState.numberPercentagePairs[changingState.selectedIndex - 1].numberImage;
            inputControl.highlightedButtons[1].image.sprite = changingState.numberPercentagePairs[changingState.selectedIndex - 2].numberImage;
            inputControl.highlightedButtons[2].image.sprite = changingState.numberPercentagePairs[changingState.selectedIndex - 2].numberImage;
        }
    }


}
