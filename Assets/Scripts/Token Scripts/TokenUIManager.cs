using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TokenUIManager : MonoBehaviour
{
    public static TokenUIManager Instance;

    [Header("References")]
    public Transform tokenPanel;
    public GameObject cardPrefab;

    [Header("Card Visuals")]
    public Color cardBackColor = new Color(0.2f, 0.2f, 0.8f);
    public Color cardFrontColor = new Color(1f, 1f, 1f);
    public Color cardUsedColor = new Color(0.3f, 0.3f, 0.3f);
    public Sprite cardBackSprite;

    private List<GameObject> cardObjects = new();
    private List<bool> cardRevealed = new();
    private int pendingCardIndex = -1;
    private bool isAnimating = false;

    void Awake()
    {
        Instance = this;
    }


    private Color GetPlayerColor(PlayerColor color)
    {
        switch (color)
        {
            case PlayerColor.Red: return new Color(0.8f, 0.1f, 0.1f);
            case PlayerColor.Blue: return new Color(0.1f, 0.1f, 0.8f);
            case PlayerColor.Green: return new Color(0.1f, 0.6f, 0.1f);
            case PlayerColor.Yellow: return new Color(0.9f, 0.8f, 0.1f);
            default: return Color.white;
        }
    }

    // Call this at the start of a player's turn to show their cards
    public void ShowTokensForPlayer(PlayerColor color)
    {
        ClearCards();

        List<int> tokens = TokenManager.Instance.GetRemainingTokens(color);

        for (int i = 0; i < tokens.Count; i++)
        {
            int cardIndex = i;
            int cardValue = tokens[i];

            // Create card from prefab
            GameObject card = Instantiate(cardPrefab, tokenPanel);
            cardObjects.Add(card);
            cardRevealed.Add(false);

            // Set root card image to transparent so background shows through
            Image cardImage = card.GetComponent<Image>();
            if (cardImage != null)
                cardImage.color = Color.clear;

            // Set background color to match current player
            Image bgImage = card.transform.Find("CardBackground")?.GetComponent<Image>();
            if (bgImage != null)
                bgImage.color = GetPlayerColor(color);

            // Set logo sprite on Logo child
            Image logoImage = card.transform.Find("Logo")?.GetComponent<Image>();
            if (logoImage != null)
            {
                logoImage.sprite = cardBackSprite;
                logoImage.color = Color.white;
            }

            // Set text empty — number shows after flip
            TMP_Text cardText = card.GetComponentInChildren<TMP_Text>();
            if (cardText != null)
                cardText.text = "";

            // Add click listener
            Button cardButton = card.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.onClick.AddListener(() =>
                {
                    OnCardClicked(cardIndex, cardValue, color);
                });
            }
        }
    }

    private void OnCardClicked(int index, int value, PlayerColor color)
    {
        // Ignore clicks if already animating or card already revealed
        if (isAnimating) return;
        if (cardRevealed[index]) return;
        if (TokenManager.Instance.HasDrawnThisTurn) return;

        // Draw the token
        int drawn = TokenManager.Instance.DrawToken(color, index);
        if (drawn == -1) return;

        // Flip animation
        StartCoroutine(FlipCard(index, drawn));
    }

    private IEnumerator FlipCard(int index, int value)
    {
        isAnimating = true;
        GameObject card = cardObjects[index];
        RectTransform rect = card.GetComponent<RectTransform>();
        TMP_Text cardText = card.GetComponentInChildren<TMP_Text>();
        Image cardImage = card.GetComponent<Image>();

        // Flip first half — shrink horizontally
        float duration = 0.15f;
        float elapsed = 0f;
        Vector3 originalScale = rect.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rect.localScale = new Vector3(
                Mathf.Lerp(1f, 0f, t),
                originalScale.y,
                originalScale.z
            );
            yield return null;
        }

        // Swap to front appearance at midpoint
        if (cardImage != null)
        {
            // Remove sprite and show plain front color
            cardImage.sprite = null;
            cardImage.color = cardFrontColor;
        }

        // Hide logo and background on flip so front looks clean
        Image bgImage = card.transform.Find("CardBackground")?.GetComponent<Image>();
        if (bgImage != null)
            bgImage.gameObject.SetActive(false);

        Image logoImage = card.transform.Find("Logo")?.GetComponent<Image>();
        if (logoImage != null)
            logoImage.gameObject.SetActive(false);

        // Show the number
        if (cardText != null)
            cardText.text = value.ToString();

        cardRevealed[index] = true;

        // Flip second half — grow back
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rect.localScale = new Vector3(
                Mathf.Lerp(0f, 1f, t),
                originalScale.y,
                originalScale.z
            );
            yield return null;
        }

        rect.localScale = originalScale;
        isAnimating = false;

        // Notify TurnManager that a token was drawn
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTokenDrawn(value);
        else
            Debug.Log($"Token drawn: {value} — TurnManager not yet built");
    }

    // Greys out all cards after a move is made
    public void DisableAllCards()
    {
        foreach (GameObject card in cardObjects)
        {
            Button btn = card.GetComponent<Button>();
            if (btn != null)
                btn.interactable = false;

            Image img = card.GetComponent<Image>();
            if (img != null)
                img.color = cardUsedColor;
        }
    }

    public void ClearCards()
    {
        foreach (GameObject card in cardObjects)
            Destroy(card);

        cardObjects.Clear();
        cardRevealed.Clear();
        isAnimating = false;
        pendingCardIndex = -1;
    }
}