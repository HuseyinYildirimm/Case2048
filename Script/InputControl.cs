using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;

public class InputControl : MonoBehaviour
{
    public List<Button> highlightedButtons = new List<Button>();
    private Button currentPressedButton = null;
    private bool isPress;
    private bool interaction;

    ChangingState changingState;
    public  Button[] allButtons;

    public Vector3 elasticStrength = new Vector3(0.2f, 0.2f, 0.2f);

    public void Awake()
    {
        changingState = GetComponent<ChangingState>();
    }

    void Start()
    {
        ButtonData();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) isPress = true;

        else if (Input.GetMouseButtonUp(0) && isPress || !IsPointerOverUIObject())
        {
            isPress = false;
            highlightedButtons.Clear();
            interaction = false;
        }

        foreach(Button b in allButtons)
        {
            Image buttonImage = b.GetComponent<Image>();

            if (buttonImage.sprite.name != "Background")
            {
                StartCoroutine(EventTriggerControl(b));
            }
            else
            {
                b.GetComponent<EventTrigger>().enabled = true;
            }

            if (buttonImage.sprite.name == "64") changingState.currentState =ChangingState.States.State1;
            if (buttonImage.sprite.name == "128") changingState.currentState = ChangingState.States.State2;
            if (buttonImage.sprite.name == "256") changingState.currentState = ChangingState.States.State3;
        }
    }

    bool IsPointerOverUIObject()
    {
        Vector2 mousePosition = Input.mousePosition;

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = mousePosition;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void ButtonData()
    {
        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));

            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            pointerEnterEntry.callback.AddListener((data) => { OnButtonPointerEnter(button); });
            trigger.triggers.Add(pointerEnterEntry);

            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((data) => { OnButtonPressed(button); });
            trigger.triggers.Add(pointerDownEntry);

            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((data) => { OnButtonReleased(button); });
            trigger.triggers.Add(pointerUpEntry);
        }
    }

    void OnButtonClicked(Button clickedButton)
    {
        // Týklanan butonu iþle
    }

    void OnButtonPointerEnter(Button enteredButton)
    {
        if (!highlightedButtons.Contains(enteredButton) && isPress && highlightedButtons.Count < 3)
        {
            highlightedButtons.Add(enteredButton);

            //DoTween
            RectTransform buttonRect = enteredButton.gameObject.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(110, 110);
            buttonRect.DOSizeDelta(new Vector2(220, 220), 0.1f).OnComplete(() => buttonRect.DOPunchScale(elasticStrength, 0.1f));
        }
    }

    void OnButtonPressed(Button pressedButton)
    {
        currentPressedButton = pressedButton;
        highlightedButtons.Add(currentPressedButton);
        interaction = true;

        //DoTween
        RectTransform buttonRect =currentPressedButton.gameObject.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(110, 110);
        buttonRect.DOSizeDelta(new Vector2(220, 220), 0.1f).OnComplete(() => buttonRect.DOPunchScale(elasticStrength, 0.1f));
    }

    void OnButtonReleased(Button releasedButton)
    {
        if (currentPressedButton == releasedButton) currentPressedButton = null;
        changingState.RandomNumber();
    }

    IEnumerator EventTriggerControl(Button b)
    {
        yield return new WaitForSeconds(0.2f);
        b.GetComponent<EventTrigger>().enabled = false;
    }
}
