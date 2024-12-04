using UnityEngine;

public class OpenUI : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    public void Open()
    {
        UI.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && UI.activeSelf)
        {
            UI.SetActive(false);
        }
    }
}
