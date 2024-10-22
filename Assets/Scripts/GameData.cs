using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public bool blacksmithInteracted = false;
    public int blacksmithDialogueIndex = 0;
    public List<string> altarAvailableItems = new List<string>();
    public List<string> altarPlayerReceivedItems = new List<string>();

}
