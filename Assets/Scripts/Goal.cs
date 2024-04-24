using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // —татическое поле, доступное другому коду
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Goal.goalMet = true;
            Material mat = GetComponent<Renderer>().material;
            mat.shader = Shader.Find("Specular");
            mat.SetColor("_Color", Color.green);
            //Color c = mat.color;
            //c.a = 1;
            //mat.color = c;
        }
    }
}
