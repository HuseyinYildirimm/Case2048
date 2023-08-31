using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ChangingState;
using DG.Tweening;

public class ButtonInfo : MonoBehaviour
{
    private ScoreManager scoreManager;
    private ChangingState changingState;
    public LayerMask layerMask;
    public Sprite newSprite;
    private bool changingSprite;
    public List<Image> matchingSprites = new List<Image>();

    [SerializeField] private Vector3 elasticStrength = new Vector3(0.2f, 0.2f, 0.2f);

    public void Start()
    {
        changingState = GameObject.Find("GameManager").GetComponent<ChangingState>();
        scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
    }

    public void Update()
    {
        CheckNeighbors();
    }

    private void CheckNeighbors()
    {
        Vector2 buttonPosition = transform.position;
        Image buttonSpriteRenderer = GetComponent<Image>();
        string buttonSpriteName = buttonSpriteRenderer.sprite.name;

        if (buttonSpriteName != "Background")
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(buttonPosition, 50, layerMask); 
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject == gameObject)
                    continue;

                Image neighborSpriteRenderer = collider.GetComponent<Image>();
                string neighborSpriteName = neighborSpriteRenderer.sprite.name;

                if (neighborSpriteName == buttonSpriteName && matchingSprites.Count <= 2)
                {
                    if (!matchingSprites.Contains(neighborSpriteRenderer) && neighborSpriteRenderer != this)
                    {
                        matchingSprites.Add(neighborSpriteRenderer);
                    }
                }
                else if(neighborSpriteName !=buttonSpriteName)
                {
                    matchingSprites.Remove(neighborSpriteRenderer);
                }
                if (matchingSprites.Count == 2 && !changingSprite )
                {
                    StartCoroutine(ClearList(buttonSpriteRenderer));
                    neighborSpriteRenderer.gameObject.GetComponent<ButtonInfo>().matchingSprites.Clear();
                }
            }
        }
        else matchingSprites.Clear();
    }
    IEnumerator ClearList(Image mainSprite)
    {
        changingSprite = true;
        yield return new WaitForSeconds(0.2f);

        List<Image> processedSprites = new List<Image>();

        foreach (Image spriteRenderer in matchingSprites)
        {
            if (!processedSprites.Contains(spriteRenderer))
            {
                spriteRenderer.sprite = newSprite;
                processedSprites.Add(spriteRenderer);
            }
        }

        yield return new WaitForSeconds(0.2f);

        foreach (NumberPercentagePair pair in changingState.numberPercentagePairs)
        {
            if (mainSprite.sprite.name == pair.numberImage.name)
            {
                int index = Array.IndexOf(changingState.numberPercentagePairs, pair);
                mainSprite.sprite = changingState.numberPercentagePairs[index + 1].numberImage;

                scoreManager.GetScore(changingState.numberPercentagePairs[index + 1].number *2, mainSprite);

                //DoTween
                RectTransform buttonRect = mainSprite.gameObject.GetComponent<RectTransform>();
                buttonRect.sizeDelta = new Vector2(110, 110);
                buttonRect.DOSizeDelta(new Vector2(220, 220), 0.1f).OnComplete(() => buttonRect.DOPunchScale(elasticStrength, 0.1f));

                break;
            }
        }
        matchingSprites.Clear();
        changingSprite = false;
    }
}
