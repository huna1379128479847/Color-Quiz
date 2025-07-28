using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class OpenUI : MonoBehaviour
{
    [SerializeField] private GameObject _ui;

    public void Open()
    {
        _ui.SetActive(true);
    }

    // Input Systemのイベントを使用して、Escapeキーが押されたときにUIを閉じる
    private void OnEscape()
    {
        if (_ui.activeSelf) _ui.SetActive(false);
    }
}
