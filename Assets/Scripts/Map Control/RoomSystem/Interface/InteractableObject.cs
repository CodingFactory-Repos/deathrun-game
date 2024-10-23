using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    protected bool hasInteracted = false;

    public abstract void Interact(Player player);

    public abstract void RestoreState(GameData gameData);

    public abstract void SaveState(GameData gameData);
}
