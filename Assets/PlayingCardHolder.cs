using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCardHolder : MonoBehaviour {

    public int elementAmount = 11;

    public ParabolicFormula formula;
    public PlayingCard playingCardPrefab;
    public float cardSpacing;

    private List<HolderElement> cardList = new List<HolderElement>();

    private void Awake() {
        StartCoroutine(DrawElements());
    }

    private IEnumerator DrawElements() {
        for (int i = 0; i < elementAmount; i++) {
            yield return new WaitForSeconds(0.1f);
            IncreaseElement();
        }
    }

    public void IncreaseElement() {
        PlayingCard.Suits[] values = (PlayingCard.Suits[])System.Enum.GetValues(typeof(PlayingCard.Suits));
        PlayingCard card = Instantiate(playingCardPrefab, transform);
        card.SetCard(values[Random.Range(0, values.Length)], Random.Range(1, 14));

        HolderElement holderElement = card.gameObject.AddComponent<HolderElement>();
        holderElement.OnDragEnd.AddListener(() => { RepositionCards(); });
        holderElement.OnDragging.AddListener(() => { CheckSwap(holderElement); });
        cardList.Add(holderElement);
        RepositionCards();
    }

    private void CheckSwap(HolderElement draggedElement) {
        int draggedElementIndex = cardList.IndexOf(draggedElement);

        // Check Left Swap
        if (draggedElementIndex > 0) {
            HolderElement leftElement = cardList[draggedElementIndex - 1];
            if (leftElement.transform.position.x > draggedElement.transform.position.x) {
                SwapElements(leftElement, draggedElement);
                return;
            }
        }

        // Check Right Swap
        if (draggedElementIndex < cardList.Count - 1) {
            HolderElement rightElement = cardList[draggedElementIndex + 1];
            if (rightElement.transform.position.x < draggedElement.transform.position.x) {
                SwapElements(rightElement, draggedElement);
                return;
            }
        }
    }

    private void SwapElements(HolderElement element1, HolderElement element2) {
        int indexElement1 = cardList.IndexOf(element1);
        int indexElement2 = cardList.IndexOf(element2);
        cardList[indexElement1] = element2;
        cardList[indexElement2] = element1;
        cardList[indexElement1].transform.SetSiblingIndex(indexElement1);
        cardList[indexElement2].transform.SetSiblingIndex(indexElement2);

        if (!element1.IsDragged()) {
            StartCoroutine(MoveElement(element1, new Vector2(GetXByIndex(indexElement2), formula.GetY(GetXByIndex(indexElement2)))));
        }
        if (!element2.IsDragged()) {
            StartCoroutine(MoveElement(element2, new Vector2(GetXByIndex(indexElement1), formula.GetY(GetXByIndex(indexElement1)))));
        }
    }

    private IEnumerator MoveElement(HolderElement element, Vector2 destination) {
        float delta = 0.0f;
        Vector2 initialPosition = element.GetComponent<RectTransform>().anchoredPosition;
        while (delta < 1.0f) {
            yield return new WaitForSecondsRealtime(0.016f); // 60 frames per second
            element.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, destination, delta);
            delta += 0.1f;
        }
        element.GetComponent<RectTransform>().anchoredPosition = destination;
    }

    public void RepositionCards() {
        for (int i = 0; i < cardList.Count; i++) {
            if (!cardList[i].IsDragged()) {
                cardList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(GetXByIndex(i), formula.GetY(GetXByIndex(i)));
                cardList[i].transform.SetSiblingIndex(i);
            }
        }
    }

    private float GetXByIndex(int index) {
        float mostLeftX = ((cardList.Count - 1) / 2.0f) * -cardSpacing;
        return mostLeftX + (index * cardSpacing);
    }

}