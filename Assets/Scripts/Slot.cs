using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    private string currentName;

    [Header("Components")]
    private Image currentImage;
    private RectTransform rectTransform;

    [Header("Icons Database")]
    [SerializeField] private Sprite emptyIcon;
    [SerializeField] private Sprite[] iDIcons = new Sprite[55];
    [SerializeField] private string[] iDItemName = new string[55];

    [Header("Stats")]
    private bool isOccupied;
    [SerializeField] private int spriteStoredID;

    private void Awake()
    {
        currentImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        IconUpdater();
    }

    #region CustomFunctions
    public void Clear()
    {
        spriteStoredID = -1;
    }

    public void GenerateRandomItem()
    {
        spriteStoredID = Random.Range(0, 54);
    }

    private void IconUpdater()
    {
        if (spriteStoredID >= 0)
        {
            currentImage.sprite = iDIcons[spriteStoredID];
            currentName = iDItemName[spriteStoredID];
            isOccupied = true;
        }
        else
        {
            currentImage.sprite = emptyIcon;
            currentName = null;
            isOccupied = false;
        }
    }
    #endregion CustomFunctions

    #region Properties
    public Sprite CurrentSprite
    {
        get { return currentImage.sprite; }
    }

    public string CurrentName
    {
        get { return currentName; }
    }

    public bool IsOccupied
    {
        get { return isOccupied; }
    }

    public int SpriteStoredID
    {
        get { return spriteStoredID; }
        set { spriteStoredID = value; }
    }

    public Vector2 RectVector2
    {
        get { return rectTransform.anchoredPosition; }
    }
    #endregion Properties

}