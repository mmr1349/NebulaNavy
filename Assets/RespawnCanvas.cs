using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnCanvas : MonoBehaviour
{
    [SerializeField] private int timer;
    [SerializeField] private Text timerText;
    // Start is called before the first frame update

    public void StartCountdown(int length)
    {
        timer = length;
        timerText.text = timer.ToString();
        Invoke("TickDown", 1);
    }

    void TickDown()
    {
        timer--;
        timerText.text = timer.ToString();
        if (timer > 0)
        {
            Invoke("TickDown", 1);
        }
    }
}
