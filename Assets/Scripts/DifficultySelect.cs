using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DifficultySelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public TextMeshProUGUI desc;
    public static string difficulty;


    public void OnPointerEnter (PointerEventData eventData)
    {
        switch (gameObject.name)
        {
            case "Easy":
            desc.text = "There are multiple lava potions spawned on the level to help you survive. And they last twice as long!";
            break;

            case "Normal":
            desc.text = "Only <b>one</b> lava potion is spawned. Use it wisely...";
            break;

            case "Luck":
            desc.text = "<b>There are no lava potions to help you</b>. Can you survive to the end..? Only the Gods of fate can decide.";
            break;

            default:
            return;
        }
    }


    public void OnPointerExit (PointerEventData eventData)
    {
        desc.text = "";
    }

    public void ButtonClicked()
    {
    	string name = EventSystem.current.currentSelectedGameObject.name;
        difficulty = name;
        SceneManager.LoadScene("Game");
    }
}
