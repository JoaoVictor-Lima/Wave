using Assets.Scripts.UI;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    private Inventory currentInventory;
    [SerializeField] private GameObject ContentItems;
    [SerializeField] private GameObject InventoryItemsPrefab;
    [SerializeField] private ItemContextMenu contextMenu;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Open(Inventory inventory)
    {
        if (currentInventory != null)
            currentInventory.OnInventoryChanged -= RefreshUI;

        currentInventory = inventory;

        currentInventory.OnInventoryChanged += RefreshUI;

        gameObject.SetActive(true);
        RefreshUI();
    }

    public void Close()
    {
        if (currentInventory != null)
            currentInventory.OnInventoryChanged -= RefreshUI;

        currentInventory = null;

        if (contextMenu)
        {
            contextMenu.Close();
        }

        gameObject.SetActive(false);
    }

    public void RefreshUI()
    {
        if (currentInventory == null) return;

        foreach (Transform child in ContentItems.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in currentInventory.Items)
        {
            GameObject inventoryItem = Instantiate(InventoryItemsPrefab, ContentItems.transform);

            var inventoryItemController = inventoryItem.GetComponent<InventoryItemUI>();

            if (inventoryItemController == null)
            {
                Debug.LogError("InventoryItemPrefab does not have an InventoryItemUI component.");
                continue;
            }

            inventoryItemController.Initialize(item);
        }
    }
}
