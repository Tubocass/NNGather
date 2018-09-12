using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    [SerializeField] string value;
    [SerializeField] Text targetText;

    private void Start()
    {
        targetText.text = value + " " + 1;
    }
    public void ChangedValue(float v)
    {
        targetText.text = value +" "+ v;
    }
	
}
