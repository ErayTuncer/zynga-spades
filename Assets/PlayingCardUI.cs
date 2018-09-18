using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayingCardUI : MonoBehaviour {

    public Text Rank;
    public Text CornerSuit;
    public Text CenterSuit;

    public void SetUI(PlayingCard.Suits suit, int rank) {
        SetSuitText(CornerSuit, suit);
        SetSuitText(CenterSuit, suit);
        SetRankText(suit, rank);
    }

    private void SetSuitText(Text textUI, PlayingCard.Suits suit) {
        textUI.color = GetSuitColor(suit);
        switch (suit) {
            case PlayingCard.Suits.CLUBS:
                textUI.text = "♣";
                break;
            case PlayingCard.Suits.DIAMONDS:
                textUI.text = "♦";
                break;
            case PlayingCard.Suits.HEARTS:
                textUI.text = "♥";
                break;
            case PlayingCard.Suits.SPADES:
                textUI.text = "♠";
                break;
            default:
                break;
        }
    }

    private void SetRankText(PlayingCard.Suits suit, int rank) {
        if (rank >= 2 && rank <= 10) {
            Rank.text = Convert.ToString(rank);
        } else if (rank == 1) {
            Rank.text = "A"; 
        } else if (rank == 11) {
            Rank.text = "J";
        } else if (rank == 12) {
            Rank.text = "Q";
        } else if (rank == 13) {
            Rank.text = "K";
        }

        Rank.color = GetSuitColor(suit);
    }

    private Color GetSuitColor(PlayingCard.Suits suit) {
        if (suit == PlayingCard.Suits.CLUBS || suit == PlayingCard.Suits.SPADES) {
            return Color.black;
        } else {
            return Color.red;
        }
    }

}