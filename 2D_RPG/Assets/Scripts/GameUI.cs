using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    // instance
    public static GameUI instance;
    // Start is called before the first frame update

    public void UpdateGoldText (int gold)
    {
        goldText.text = "<b>Gold:</b> " + gold;
    }

    void Awake ()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
