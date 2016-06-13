using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    public static ClickManager instance;
    public Camera MainCamera, DummyCamera;
    public Canvas MainCanvas, CharacterCanvas, LevelCanvas;
    public GameObject CharacterSelection, LevelSelection, LevelButton;
    public bool enableLevel;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SelectCharacter()
    {
        MainCanvas.enabled = false;
        CharacterSelection.SetActive(true);
        CharacterCanvas.worldCamera = MainCamera;

    }

	public void CharacterSelected()
    {
        CharacterCanvas.worldCamera = DummyCamera;
        CharacterSelection.SetActive(false);
        MainCanvas.enabled = true;
        
    }

    public void SelectLevel()
    {
        MainCanvas.enabled = false;
        LevelSelection.SetActive(true);
        LevelCanvas.worldCamera = MainCamera;
    }

    public void LevelSelected()
    {
        LevelCanvas.worldCamera = DummyCamera;
        LevelSelection.SetActive(false);
        MainCanvas.enabled = true;
    }

    void Update()
    {
        if (enableLevel)
            EnableLevelButton();
        else
            DisableLevelButton();
    }

    public void DisableLevelButton()
    {
        if (LevelButton != null)
            LevelButton.SetActive(false);
    }

    public void EnableLevelButton()
    {
        if (LevelButton != null)
            LevelButton.SetActive(true);
    }
}
