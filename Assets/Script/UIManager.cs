using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTXT;

    void Start()
    {
        showScore(0);
    }

    public void showScore(int score)
    {
        scoreTXT.text = score.ToString();
    }
}
