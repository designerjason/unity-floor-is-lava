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
            desc.text = "There are lava boots spawned on the level to help you survive.";
            break;

            case "Normal":
            desc.text = "Only <b>one</b> pair of lava boots are spawned. Use them wisely...";
            break;

            case "Luck":
            desc.text = "<b>There are no lava boots to help you</b>. Can you survive to the end..? Only the Gods of fate can decide.";
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
        //difficulty = name;
        difficulty = "Easy";
        SceneManager.LoadScene("Game");
    }
}
