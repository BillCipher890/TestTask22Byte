using System.Collections;
using UnityEngine;

public class GameOverChange : MonoBehaviour
{
    [SerializeField]
    private float _scaleTimeSeconds;
    [SerializeField]
    private GameObject _gameplayUI;
    [SerializeField]
    private GameObject _failUI;
    [SerializeField]
    private GameObject _winUI;

    private void Start()
    {
        GlobalEventManager.OnDeath += Fail;
        GlobalEventManager.OnWin += Win;
        GlobalEventManager.OnRetry += ReSubscribeFailEvent;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnDeath -= Fail;
        GlobalEventManager.OnWin -= Win;
        GlobalEventManager.OnRetry -= ReSubscribeFailEvent;
    }

    public void ReSubscribeFailEvent()
    {
        GlobalEventManager.OnDeath += Fail;
    }

    private void Fail()
    {
        _gameplayUI.SetActive(false);
        _failUI.SetActive(true);
        _failUI.transform.localScale = Vector3.zero;
        GlobalEventManager.OnDeath -= Fail;
        StartCoroutine(UIScale(_failUI));
    }

    private void Win(int money)
    {
        GlobalData.CurrentMoney += money;
        _gameplayUI.SetActive(false);
        _winUI.GetComponent<WinUIActivateChanger>().Activate();
        _winUI.transform.localScale = Vector3.zero;
        StartCoroutine(UIScale(_winUI));
    }

    IEnumerator UIScale(GameObject ui)
    {
        var fixedTickCount = 50 * _scaleTimeSeconds;
        for (int i = 0; i < fixedTickCount; i++)
        {
            ui.transform.localScale += new Vector3(1.0f / fixedTickCount, 1.0f / fixedTickCount, 1.0f / fixedTickCount);
            yield return new WaitForFixedUpdate();
        }
    }
}
