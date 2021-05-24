using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeskManager : MonoBehaviour
{
    public RectTransform manualGuideTransform;
    public RectTransform manualItemTransform;

    public Text manualText;

    public RectTransform[] checkoutKeysTransforms = new RectTransform[3];

    public RectTransform checkoutPopup;
    public Text checkoutText;

    int keyNumber;
    int manualPage;
    
    public TextAsset[] manualTexts;

    // Start is called before the first frame update
    void Start()
    {
        ManualUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        int checkoutKeys = GameEventManager.instance.GetCheckoutKeyCount();

        for (int i = 0; i < 3; i++)
            checkoutKeysTransforms[i].localScale = Vector2.up;
        for (int i = 0; i < checkoutKeys; i++)
            checkoutKeysTransforms[i].localScale = Vector2.one;
    }
    public void OnClickedBook()
    {
        manualGuideTransform.localScale = Vector2.one;
    }
    public void OnClickedManualGuide()
    {
        manualGuideTransform.localScale = Vector2.one;
        manualItemTransform.localScale = Vector2.zero;

    }
    public void OnClickedManualItem()
    {
        manualGuideTransform.localScale = Vector2.zero;
        manualItemTransform.localScale = Vector2.one;
    }
    public void OnClickedClose()
    {
        manualGuideTransform.localScale = Vector2.zero;
        manualItemTransform.localScale = Vector2.zero;
    }

    public void OnClickedCheckoutKey(int keyNumber)
    {
        this.keyNumber = keyNumber;
        KeyValuePair<int, int> checkoutRoom = GameEventManager.instance.GetCheckoutRoom(keyNumber);
        checkoutText.text = checkoutRoom.Key + "0" + checkoutRoom.Value + "호 체크아웃을 완료합니다.";
        checkoutPopup.localScale = Vector2.one;
    }

    public void OnClickedConfirmCheckout()
    {
        GameEventManager.instance.Checkout(keyNumber);
        checkoutPopup.localScale = Vector2.up;
    }

    public void OnClickedManual()
    {
        Debug.Log(manualPage);
        if (manualPage == 0)
        {
            manualPage = 1;
        }
        else
        {
            manualPage = 0;
        }
        ManualUpdate();
    }
    void ManualUpdate()
    {
        manualText.text = manualTexts[manualPage].text;
    }

}
