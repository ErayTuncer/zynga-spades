using System.Collections.Generic;
using System.Linq;

public class IncrementalStrategy : ISortingStrategy {

    public List<PlayingCard> Sort(List<PlayingCard> cardList) {
        List<PlayingCard> sortedList = new List<PlayingCard>();
        List<PlayingCard> tempList = new List<PlayingCard>();
        cardList
            .OrderBy(card => card.GetSuit())
            .ThenBy(card => card.GetRank())
            .ToList().ForEach(card => {
                if (tempList.Count == 0) {
                    tempList.Add(card);
                } else {
                    if (card.GetSuit().Equals(tempList[tempList.Count - 1].GetSuit())
                        && card.GetRank().Equals(tempList[tempList.Count - 1].GetRank() + 1)) {
                        tempList.Add(card);
                    } else {
                        MoveTempToSortList(tempList, sortedList);
                        tempList.Add(card);
                    }
                }
        });
        MoveTempToSortList(tempList, sortedList);
        return sortedList;
    }

    private void MoveTempToSortList(List<PlayingCard> tempList, List<PlayingCard> sortedList) {
        if (tempList.Count >= 3) {
            sortedList.InsertRange(0, tempList);
        } else {
            sortedList.AddRange(tempList);
        }
        tempList.Clear();
    }

}