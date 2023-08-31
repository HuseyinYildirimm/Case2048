using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI scoreTextPrefab;
    [SerializeField] private Transform scoreCanvas; // Reference to the Canvas where scores will be displayed

    public void Update()
    {
        currentScoreText.text = currentScore.ToString("F0");
    }

    public void GetScore(int getScore, Image parentImage)
    {
        Vector3 spawnPosition = parentImage.transform.position;
        TextMeshProUGUI cloneScore = Instantiate(scoreTextPrefab, spawnPosition, Quaternion.identity, scoreCanvas);

        string scoreText = "+" + getScore.ToString("F0");
        cloneScore.text = scoreText;

        currentScore += getScore;

        cloneScore.transform.DOMoveY(cloneScore.transform.position.y + 50f, 1f)
           .SetEase(Ease.OutQuad)
           .OnComplete(() => Destroy(cloneScore.gameObject)); 
    }
}
