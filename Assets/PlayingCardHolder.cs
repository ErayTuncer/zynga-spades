using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCardHolder : MonoBehaviour {

    public int CardAmount = 11;
    public float MultiplierX = 1.0f;
    public float MultiplierY = 1.0f;
    public float MultiplierRotate = 1.0f;

    public PlayingCard CardPrefab;

    private List<PlayingCard> cardPlaceList = new List<PlayingCard>();

    private void Awake() {
        DrawCards();
    }

    public void DrawCards() {
        ClearCards();
        for (int i = 0; i < CardAmount; i++) {
            DrawRandomCard(i);
        }
    }

    private void ClearCards() {
        foreach (PlayingCard card in cardPlaceList) {
            Destroy(card.gameObject);
        }
        cardPlaceList.Clear();
    }

    private void DrawRandomCard(int cardIndex) {
        PlayingCard.Suits[] values = (PlayingCard.Suits[])Enum.GetValues(typeof(PlayingCard.Suits));

        PlayingCard card = Instantiate<PlayingCard>(CardPrefab, transform);
        card.GetComponent<RectTransform>().anchoredPosition = GetCardPosition(cardIndex);
        card.GetComponent<RectTransform>().Rotate(new Vector3(0.0f, 0.0f, -1.0f * MultiplierRotate * GetProjectionIndex(cardIndex)));

        card.SetCard(values[UnityEngine.Random.Range(0, values.Length)], UnityEngine.Random.Range(1, 14));

        cardPlaceList.Add(card);
    }

    private Vector2 GetCardPosition(int cardIndex) {
        Vector2 holderPosition = GetComponent<RectTransform>().anchoredPosition;

        int projectionIndex = GetProjectionIndex(cardIndex);
        float cardX = holderPosition.x + (projectionIndex * MultiplierX);
        float cardY = holderPosition.y - (MultiplierY * projectionIndex * projectionIndex);

        return new Vector2(cardX, cardY);
    }

    private int GetProjectionIndex(int cardIndex) {
        int centerIndex = CardAmount / 2;
        return cardIndex - centerIndex;
    }

}