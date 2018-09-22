using UnityEngine;

[RequireComponent(typeof(PlayingCardUI))]
public class PlayingCard : MonoBehaviour {

    public enum Suits {
        CLUBS,
        DIAMONDS,
        HEARTS,
        SPADES,
    }

    private Suits suit;
    private int rank;
    private PlayingCardUI cardUI;

    private void Awake() {
        cardUI = GetComponent<PlayingCardUI>();

        SetCard(Suits.HEARTS, 1); // TODO: remove!!!
    }

    public void SetCard(Suits suit, int rank) {
        if (rank < 1 || rank > 13) {
            throw new System.ArgumentException("Rank cannot be lower than 1 or greater than 13. Rank provided: " + rank);
        }

        this.suit = suit;
        this.rank = rank;

        ResetUI();
    }

    public void ResetUI() {
        cardUI.SetUI(suit, rank);
    }

    public Suits GetSuit() {
        return suit;
    }

    public int GetRank() {
        return rank;
    }

}