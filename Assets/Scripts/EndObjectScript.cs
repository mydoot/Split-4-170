using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class EndObjectScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textObject;
    

    void Start()
    {
        // End Object needs to be active to become a subscriber to the event before it sets itself to be inactive
        GameManager.triggerSignUI += changeText;

        if (transform.gameObject.activeSelf)
            transform.gameObject.SetActive(false);
    }

    public void changeText(string Text)
    {
        textObject.text = Text;
    }
}
