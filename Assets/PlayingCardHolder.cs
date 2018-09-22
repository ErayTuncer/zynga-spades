using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCardHolder : MonoBehaviour {

    public int EmptySlotAmount = 11;
    public float cardSpacing;
    public float rotationMultiplier = 5.0f;
    public ParabolicFormula formula;

    private List<GameObject> cardList = new List<GameObject>();

    public void AddCard(PlayingCard newCard) {
        if (EmptySlotAmount < 0) {
            return;
        }

        newCard.transform.SetParent(transform);
        newCard.transform.rotation = new Quaternion();

        HolderElement holderElement = newCard.gameObject.AddComponent<HolderElement>();
        holderElement.OnDragBegin.AddListener(() => holderElement.GetComponent<RectTransform>().rotation = new Quaternion());
        holderElement.OnDragging.AddListener(() => CheckSwap(holderElement.gameObject));
        holderElement.OnDragEnd.AddListener(() => RepositionCards());
        cardList.Add(newCard.gameObject);
        RepositionCards();
    }

    private void CheckSwap(GameObject draggedElement) {
        int draggedElementIndex = cardList.IndexOf(draggedElement.gameObject);

        // Check Left Swap
        if (draggedElementIndex > 0) {
            GameObject leftElement = cardList[draggedElementIndex - 1];
            if (leftElement.transform.position.x > draggedElement.transform.position.x) {
                SwapElements(leftElement, draggedElement);
                return;
            }
        }

        // Check Right Swap
        if (draggedElementIndex < cardList.Count - 1) {
            GameObject rightElement = cardList[draggedElementIndex + 1];
            if (rightElement.transform.position.x < draggedElement.transform.position.x) {
                SwapElements(rightElement, draggedElement);
                return;
            }
        }
    }

    public void OnClickIncremental() {
        SortCards(new IncrementalStrategy());
    }

    public void OnClickSameRank() {
        SortCards(new SameRankStrategy());
    }

    public void SortCards(ISortingStrategy sortStrategy) { 
        List<PlayingCard> playingCardList = new List<PlayingCard>();
        cardList.ForEach(element => playingCardList.Add(element.GetComponent<PlayingCard>()));

        List<PlayingCard> sortedList = sortStrategy.Sort(playingCardList);
        cardList.Clear();
        sortedList.ForEach(element => cardList.Add(element.gameObject));
        RepositionCards();


        // TODO: remove below
        Debug.LogError("ORDERED:");
        sortedList.ForEach(element => Debug.LogError("Suit: " + element.GetSuit() + "   Rank: " + element.GetRank()));
    }

    private void SwapElements(GameObject element1, GameObject element2) {
        int indexElement1 = cardList.IndexOf(element1);
        int indexElement2 = cardList.IndexOf(element2);
        cardList[indexElement1] = element2;
        cardList[indexElement2] = element1;

        cardList[indexElement1].transform.SetSiblingIndex(indexElement1);
        cardList[indexElement2].transform.SetSiblingIndex(indexElement2);

        if (!element1.GetComponent<HolderElement>().IsDragged()) {
            StartCoroutine(MoveElement(element1, new Vector2(GetXByIndex(indexElement2), formula.GetY(GetXByIndex(indexElement2)))));
        }
        if (!element2.GetComponent<HolderElement>().IsDragged()) {
            StartCoroutine(MoveElement(element2, new Vector2(GetXByIndex(indexElement1), formula.GetY(GetXByIndex(indexElement1)))));
        }
    }

    private IEnumerator MoveElement(GameObject element, Vector2 destination) {
        float delta = 0.0f;
        Vector2 initialPosition = element.GetComponent<RectTransform>().anchoredPosition;
        while (delta < 1.0f) {
            yield return new WaitForSecondsRealtime(0.016f); // 60 frames per second
            Vector2 nextPosition = Vector2.Lerp(initialPosition, destination, delta);
            element.GetComponent<RectTransform>().anchoredPosition = nextPosition;
            element.GetComponent<RectTransform>().rotation = Quaternion.Euler(0.0f, 0.0f, nextPosition.x * rotationMultiplier);
            delta += 0.1f;
        }
        element.GetComponent<RectTransform>().anchoredPosition = destination;
        element.GetComponent<RectTransform>().rotation = Quaternion.Euler(0.0f, 0.0f, destination.x * rotationMultiplier);
    }

    private void OnValidate() {
        RepositionCards();
    }

    public void RepositionCards() {
        for (int i = 0; i < cardList.Count; i++) {
            if (!cardList[i].GetComponent<HolderElement>().IsDragged()) {
                cardList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(GetXByIndex(i), formula.GetY(GetXByIndex(i)));
                cardList[i].GetComponent<RectTransform>().rotation = Quaternion.Euler(0.0f, 0.0f, GetXByIndex(i) * rotationMultiplier);
                cardList[i].transform.SetSiblingIndex(i);
            }
        }
    }

    private float GetXByIndex(int index) {
        float mostLeftX = ((cardList.Count - 1) / 2.0f) * -cardSpacing;
        return mostLeftX + (index * cardSpacing);
    }

}