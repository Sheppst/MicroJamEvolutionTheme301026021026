using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerAction playerAction { get; private set; }
    public PlayerEnvironment playerEnvironment { get; private set; }
    public PlayerInventory playerInventory { get; private set; }
    public PlayerEventProcessing playerEventProcessing { get; private set; }

    public void Initialize()
    {
        playerAction = new();
        playerEnvironment = new();
        playerInventory = new();
        playerEventProcessing = new();

        playerAction.Initialize();
        playerEnvironment.Initialize();
        playerInventory.Initialize();
        playerEventProcessing.Initialize();
    }

    public void PlayerUpdate()
    {
        playerAction.ActionUpdate();
        playerEnvironment.EnvironmentUpdate();
        playerInventory.InventoryUpdate();
        playerEventProcessing.EventProcessingUpdate();
    }
}
