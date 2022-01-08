using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryControls : MonoBehaviour
{

    [Header("Audio Clips")]
    [SerializeField] private AudioClip discard;
    [SerializeField] private AudioClip holdRelease;
    [SerializeField] private AudioClip move;
    [SerializeField] private AudioClip randomize;

    [Header("Inventory")]
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] private Slot[] slotsArray = new Slot[18];
    private int columnSize = 3, rowSize = 6;

    [Header("Selector")]
    [SerializeField] private Selector selector;
    [SerializeField] private int selectorInitialSlot = 0;

    private AudioSource audioSource;
    private int resID = 2;
    private bool resLoaded;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ItemTextDisplay();
    }

    private void Start()
    {
        selector.RectVector2 = slotsArray[selectorInitialSlot].RectVector2;
        InventoryStart();
    }

    #region CustomFunctions
    public void ChangeRes(InputAction.CallbackContext context)
    {
        if (context.performed && resLoaded)
        {
            resID++;
            resLoaded = false;
            audioSource.PlayOneShot(move);
        }
        if (resLoaded == false)
        {
            switch (resID)
            {
                case 1:
                    Screen.SetResolution(1280, 720, true);
                    resLoaded = true;
                    break;
                case 2:
                    Screen.SetResolution(1920, 1080, true);
                    resLoaded = true;
                    break;
                case 3:
                    Screen.SetResolution(3860, 2160, true);
                    resLoaded = true;
                    resID = 0;
                    break;
            }
        }
    }

    private void ClearAllAndRandomize5()
    {
        for (int i = 0; i < slotsArray.Length; i++)
        {
            slotsArray[i].Clear();
        }
        InventoryStart();
        audioSource.PlayOneShot(randomize);
    }

    private void Discard()
    {
        selector.HeldSpriteFromSlot = -1;
        selector.IsHolding = false;
        selector.ClearSelectedImageSprite();
        audioSource.PlayOneShot(discard);
    }

    public void HoldOrRelease(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Pressed A on a slot with an item (sprite).
            if (slotsArray[selector.SelectorAtSlot].IsOccupied == true)
            {
                // Was holding something.
                if (selector.HeldSpriteFromSlot >= 0)
                {
                    /* Using "aux" to hold the sprite ID from the second selected sprite, which is about to get the
                     * sprite ID value from the first selected sprite. */
                    int aux = slotsArray[selector.SelectorAtSlot].SpriteStoredID;
                    // Second selected sprite gets the sprite of the first selected sprite.
                    slotsArray[selector.SelectorAtSlot].SpriteStoredID = selector.HeldSpriteIDWas;
                    // Firstly selected sprite gets the sprite of the second selected sprite.
                    slotsArray[selector.HeldSpriteFromSlot].SpriteStoredID = aux;
                    // Cleaning "Selected" GameObject.
                    selector.ClearSelectedImageSprite();
                    selector.HeldSpriteFromSlot = -1;
                    selector.IsHolding = false;
                }
                // Wasn't holding anything.
                else
                {
                    // "Selected" GameObject's sprite receives the sprite that was in the selected slot.
                    selector.CurrentImageSprite = slotsArray[selector.SelectorAtSlot].CurrentSprite;
                    // Saving the selected slot number and icon ID in case we have to switch its icon with another slot's.
                    selector.HeldSpriteFromSlot = selector.SelectorAtSlot;
                    selector.HeldSpriteIDWas = slotsArray[selector.SelectorAtSlot].SpriteStoredID;
                    selector.IsHolding = true;
                    /* Clearing the selected slot's sprite (gives the impression that the item was "picked",
                     * since its sprite was transfered to the "Selected" Gameobject's). */
                    slotsArray[selector.SelectorAtSlot].Clear();
                }
                audioSource.clip = holdRelease;
                audioSource.Play(0);
            }
            // Pressed A on an empty slot (no sprite).
            else
            {
                // Was holding something.
                if (selector.HeldSpriteFromSlot >= 0)
                {
                    // Empty sprite slot receives the first selected sprite.
                    slotsArray[selector.SelectorAtSlot].SpriteStoredID = selector.HeldSpriteIDWas;
                    // Cleaning "Selected" GameObject.
                    selector.ClearSelectedImageSprite();
                    selector.HeldSpriteFromSlot = -1;
                    selector.IsHolding = false;
                    audioSource.clip = holdRelease;
                    audioSource.Play(0);
                }
                // Wasn't holding anything.
                else
                {
                    // Nothing happens.
                }
            }
        }
    }

    private void ItemTextDisplay()
    {
        itemNameText.text = slotsArray[selector.SelectorAtSlot].CurrentName;
    }

    public void DiscardOrRandomize(InputAction.CallbackContext context)
    {
        if (context.performed && selector.IsHolding == false)
        {
            ClearAllAndRandomize5();
        }
        else if (context.performed && selector.IsHolding)
        {
            Discard();
        }
    }

    private void InventoryStart()
    {
        int[] initialSlots = new int[5];
        int[] possiblePositions = new int[slotsArray.Length];
        // Filling the "possiblePositions" list so we can shuffle it and give its new first values to the "initialSlots" elements.
        for (int i = 0; i < slotsArray.Length; i++)
        {
            possiblePositions[i] = i;
        }
        ShuffleArray(possiblePositions);
        for (int i = 0; i < initialSlots.Length; i++)
        {
            initialSlots[i] = possiblePositions[i];
        }
        for (int i = 0; i < initialSlots.Length; i++)
        {
            slotsArray[initialSlots[i]].GenerateRandomItem();
        }
    }

    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        if (context.performed && inputVector == new Vector2(0,1))
        {
            if (selector.SelectorAtSlot <= rowSize - 1)
            {
                selector.SelectorAtSlot += rowSize * (columnSize - 1);
            }
            else
            {
                selector.SelectorAtSlot -= rowSize;
            }
            audioSource.clip = move;
            audioSource.Play(0);
        }
        if (context.performed && inputVector == new Vector2(0, -1))
        {
            if (selector.SelectorAtSlot >= rowSize * (columnSize - 1))
            {
                selector.SelectorAtSlot -= rowSize * (columnSize - 1);
            }
            else
            {
                selector.SelectorAtSlot += rowSize;
            }
            audioSource.clip = move;
            audioSource.Play(0);
        }
        if (context.performed && inputVector == new Vector2(-1, 0))
        {
            if (selector.SelectorAtSlot % rowSize == 0)
            {
                selector.SelectorAtSlot += rowSize - 1;
            }
            else
            {
                selector.SelectorAtSlot--;
            }
            audioSource.clip = move;
            audioSource.Play(0);
        }
        if (context.performed && inputVector == new Vector2(1, 0))
        {
            if ((selector.SelectorAtSlot + 1) % rowSize == 0)
            {
                selector.SelectorAtSlot -= rowSize - 1;
            }
            else
            {
                selector.SelectorAtSlot++;
            }
            audioSource.clip = move;
            audioSource.Play(0);
        }
        selector.RectVector2 = slotsArray[selector.SelectorAtSlot].RectVector2;
    }

    private int[] ShuffleArray(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            int rnd = Random.Range(0, arr.Length);
            int temp = arr[rnd];
            arr[rnd] = arr[i];
            arr[i] = temp;
        }
        return arr;
    }
    #endregion CustomFunctions

}