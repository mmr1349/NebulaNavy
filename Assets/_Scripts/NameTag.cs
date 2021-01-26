using UnityEngine;
using UnityEngine.UI;

public class NameTag : MonoBehaviour
{
    [SerializeField] private Text nameTagText;
    private Camera mainCam = null;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        nameTagText = GetComponentInChildren<Text>();
    }

    private void LateUpdate() {
        if (mainCam) {
            transform.LookAt(mainCam.transform.position);
        }
    }

    public void SetTextAndColor(string text, Color color) {
        nameTagText.text = text;
        nameTagText.color = color;
    }
}
