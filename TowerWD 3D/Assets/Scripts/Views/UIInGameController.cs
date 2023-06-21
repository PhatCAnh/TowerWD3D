
using UnityEngine;
using TMPro;
using CanasSource;

public class UIInGameController : MonoBehaviour
{
    private InGameController inGame => Singleton<InGameController>.Instance;
    public TextMeshProUGUI txtCoinUI;

    private void Start()
    {
        inGame.changeCoinEvent.AddListener(param => UpdateCoinUI(param));
    }

    private void UpdateCoinUI(int coin)
    {
        txtCoinUI.text = coin.ToString();
    }
}
