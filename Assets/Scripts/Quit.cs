using UnityEngine;
using System.Collections;

// Quits the player when the user hits escape

public class ExampleClass : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("escape"))  // если нажата клавиша Esc (Escape)
        {
            Application.Quit();    // закрыть приложение
        }
    }
}