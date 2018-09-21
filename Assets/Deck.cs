using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CardData {
    public PlayingCard.Suits Suit;
    public int Rank;
}

public class Deck : MonoBehaviour {

    public PlayingCardHolder holder;
    public PlayingCard playingCardPrefab;

    private List<CardData> cardList = new List<CardData>();

    private void Awake() {
        PlayingCard.Suits[] suits = (PlayingCard.Suits[])System.Enum.GetValues(typeof(PlayingCard.Suits));

        foreach (PlayingCard.Suits suit in suits) {
            for (int rank = 1; rank <= 13; rank++) {
                CardData data = new CardData();
                data.Suit = suit;
                data.Rank = rank;
                cardList.Add(data);
            }
        }
    }

    public void DrawCard() {
        if (holder.EmptySlotAmount > 0) {
            holder.EmptySlotAmount--;

            CardData randomData = cardList[UnityEngine.Random.Range(0, cardList.Count)];
            cardList.Remove(randomData);

            PlayingCard card = Instantiate(playingCardPrefab, transform.parent);
            card.SetCard(randomData.Suit, randomData.Rank);
            card.transform.position = transform.position;

            StartCoroutine(AnimateDraw(card, () => {
                holder.AddCard(card);
                DrawCard();
            }));
        }
    }

    private IEnumerator AnimateDraw(PlayingCard card, Action onComplete) {
        float delta = 0.0f;
        Vector3 initialPosition = card.transform.position;
        while (delta < 1.0f) {
            yield return new WaitForSecondsRealtime(0.016f); // 60 frames per second
            card.transform.position = Vector3.Lerp(initialPosition, holder.transform.position, delta);
            card.transform.Rotate(new Vector3(0.0f, 0.0f, 40.0f));
            delta += 0.05f;
        }
        card.transform.position = holder.transform.position;
        onComplete();
    }

}