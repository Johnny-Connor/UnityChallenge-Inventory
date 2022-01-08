using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{

    private RectTransform rectTransform;
    private Image selectorImage;
    [SerializeField] Selected selected;
    [SerializeField] private int selectorAtSlot = 0;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        selectorImage = GetComponent<Image>();
    }

    private void Update()
    {
        FixSelectedSpriteImperfection();
        IsHoldingSelectorColor();
    }

    #region CustomFunctions
    public void ClearSelectedImageSprite()
    {
        selected.Clear();
    }

    public void FixSelectedSpriteImperfection()
    {
        if (transform.localPosition.y == 0)
        {
            selected.transform.localScale = new Vector3(selected.transform.localScale.x, 0.93f, selected.transform.localScale.z);
        }
        else
        {
            selected.transform.localScale = new Vector3(selected.transform.localScale.x, 0.925f, selected.transform.localScale.z);
        }
    }

    public void IsHoldingSelectorColor()
    {
        if (selected.IsHolding)
        {
            selectorImage.color = new Vector4(1, 0, 0, 1);
        }
        else
        {
            selectorImage.color = new Vector4(1, 1, 1, 1);
        }
    }
    #endregion CustomFunctions

    #region Properties
    #region Selector.cs Properties
    public Vector2 RectVector2    
    {
        set { rectTransform.anchoredPosition = value; }
    }

    public int SelectorAtSlot
    {
        get { return selectorAtSlot;  }
        set { selectorAtSlot = value; }
    }
    #endregion Selector.cs Properties
    #region Selected.cs Properties
    public Sprite CurrentImageSprite
    {
        get { return selected.CurrentImageSprite; }
        set { selected.CurrentImageSprite = value;  }
    }

    public int HeldSpriteFromSlot
    {
        get { return selected.HeldSpriteFromSlot; }
        set { selected.HeldSpriteFromSlot = value; }
    }

    public int HeldSpriteIDWas
    {
        get { return selected.HeldSpriteIDWas; }
        set { selected.HeldSpriteIDWas = value; }
    }

    public bool IsHolding
    {
        get { return selected.IsHolding; }
        set { selected.IsHolding = value; }
    }
    #endregion Selected.cs Properties
    #endregion Properties

}