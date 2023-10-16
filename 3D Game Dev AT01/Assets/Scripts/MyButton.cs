using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyButton : MonoBehaviour
{
    public string buttonID;

    private bool redOrGreen = false;

    private Image buttonSprite;


    private void Awake()
    {
        if(!TryGetComponent<Image>(out buttonSprite))
        {
            Debug.Log("this script needs to be attached to something with an image component like a button");
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        //MoveCube.directionalButtonPushEvent += SetButtonColor;
        buttonSprite.color = Color.white;
    }



    private void SetButtonColor(bool colorSet, string bid)
    {
        if (bid == buttonID)
        {


            if (colorSet == true)
            {
                buttonSprite.color = Color.green;
            }
            else
            {
                buttonSprite.color = Color.red;
            }

            Invoke("ResetTimer", 2f);
        }
    }



    private void ResetTimer()
    {
        buttonSprite.color = Color.white;
    }

    private void OnDestroy()
    {
        //MoveCube.directionalButtonPushEvent -= SetButtonColor;
    }

}
