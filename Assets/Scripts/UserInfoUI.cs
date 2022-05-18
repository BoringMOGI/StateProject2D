using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoUI : MonoBehaviour
{
    [SerializeField] Text userNameText;
    [SerializeField] Text hpText;
    [SerializeField] Image hpImage;

    public void Setup(string userName)
    {
        userNameText.text = userName;
    }
    public void UpdateHp(int current, float max)
    {
        hpText.text = string.Format("{0}/{1}", current, max);
        hpImage.fillAmount = current / max;
    }
}
