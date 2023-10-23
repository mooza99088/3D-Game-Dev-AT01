using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    [SerializeField] List<MyButton> mButtons = new List<MyButton>();


    private void Start()
    {
        Player.directionalButtonPushEvent += SetButtonColor;
    }

    private void OnDestroy()
    {
        Player.directionalButtonPushEvent -= SetButtonColor;
    }


    private void SetButtonColor(bool greenOrRed, string buttonID)
    {
        foreach(MyButton button in mButtons)
        {
            if(buttonID == button.buttonID)
            {
                Image img = button.GetComponent<Image>();
                if(greenOrRed == true)
                {
                    img.color = Color.green;
                }
                else
                {
                    img.color = Color.red;
                }

                StartCoroutine(ResetButtonColor(button));
            }
        }
    }


    private IEnumerator ResetButtonColor(MyButton mButton)
    {
        yield return new WaitForSeconds(2f);
        Image img = mButton.GetComponent<Image>();
        img.color = Color.white;
        
    }

    
}
