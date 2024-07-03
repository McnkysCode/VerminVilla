using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons;


    public void SetInteractibleCards(bool isInteractible)
    {
        foreach (Button button in buttons)
        {

            button.interactable = isInteractible;
        }
    }
}
