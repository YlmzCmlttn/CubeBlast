using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GoalPanel : MonoBehaviour
{
    [SerializeField]private Image thisImage;
    public Sprite sprite;
    [SerializeField] private TextMeshProUGUI thisText;
    public string text;
    [SerializeField] Image checkedImage;
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    private void Setup()
    {
        thisImage.sprite = sprite;
        thisText.text = text;
        checkedImage.gameObject.SetActive(false);
    }

    public void Completed() {
        checkedImage.gameObject.SetActive(true);
        thisText.gameObject.SetActive(false);
    }

    public void UpdateGoal(string text)
    {
        thisText.text = text;
    }
}
