using UnityEngine;
using UnityEngine.UI;

public class Selected : MonoBehaviour
{

    private Image currentImage;
    [SerializeField] private Sprite emptyIcon;
    private int heldSpriteIDWas;
    private int heldSpriteFromSlot = -1;
    private bool isHolding;

    private void Awake()
    {
        currentImage = GetComponent<Image>();
    }

    #region Custom Functions
    public void Clear()
    {
        currentImage.sprite = emptyIcon;
    }
    #endregion Custom Functions

    #region Properties
    public Sprite CurrentImageSprite
    {
        get { return currentImage.sprite; }
        set { currentImage.sprite = value; }
    }

    public bool IsHolding
    {
        get { return isHolding; }
        set { isHolding = value; }
    }

    public int HeldSpriteFromSlot
    {
        get { return heldSpriteFromSlot; }
        set { heldSpriteFromSlot = value; }
    }

    public int HeldSpriteIDWas
    {
        get { return heldSpriteIDWas; }
        set { heldSpriteIDWas = value; }
    }
    #endregion Properties

}