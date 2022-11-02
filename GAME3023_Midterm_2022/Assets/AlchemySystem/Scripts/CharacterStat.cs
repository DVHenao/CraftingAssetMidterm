using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CharacterStat : MonoBehaviour
{
    public int stat;
    public TMP_Text TextBox;
    
    // Start is called before the first frame update
    void Start()
    {
        TextBox.GetComponentInChildren<TMP_Text>().text =  stat.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyStat(int modifier)
    {
        stat += modifier;
        TextBox.GetComponentInChildren<TMP_Text>().text = stat.ToString();
    }
}
