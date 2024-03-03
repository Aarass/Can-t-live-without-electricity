using Assets.Scripts.GeneralPurpose;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorShower : SingletonMonoBehaviour<ErrorShower>
{
    [SerializeField] float speed;
    [SerializeField] Color backgroundColor;
    [SerializeField] Color textColor;
    TMP_Text tmp;
    Material myMaterial;
    float alpha;
    private void Start()
    {
        myMaterial = GetComponent<Image>().material;
        tmp = GetComponentInChildren<TMP_Text>();
    }
    private void Update()
    {
        alpha = Mathf.Max(0, alpha - speed * Time.deltaTime);
        Color c;

        c = backgroundColor;
        c.a = Mathf.Min(1f, alpha);
        myMaterial.SetColor("_Color", c);

        c = textColor;
        c.a = Mathf.Min(1f, alpha);
        tmp.material.color = c;
        tmp.color = c;
    }
    public void ShowText(string text)
    {
        if (alpha > 0f && text == tmp.text) return;
        tmp.text = text;
        alpha = 1.5f;
    }
}
