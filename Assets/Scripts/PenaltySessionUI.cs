using UnityEngine;
using UnityEngine.UI;

public class PenaltySessionUI : MonoBehaviour
{
    public Image[] attempts;
/*
    void Start()
    {
        MarkSuccess(0);    
    }
*/
    public void MarkSuccess(int index)
    {
        if (index >= 0 && index < attempts.Length)
        {
            Debug.Log("MarkSuccess " + index);
            attempts[index].color = Color.green;
        }
    }

    public void MarkFailure(int index)
    {
        if (index >= 0 && index < attempts.Length)
        {
            Debug.Log("MarkFailure " + index);
            attempts[index].color = Color.red;
        }
    }
}
