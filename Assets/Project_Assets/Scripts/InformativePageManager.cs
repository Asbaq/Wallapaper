using UnityEngine;
using UnityEngine.UI;

public class InformativePageManager : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentText;
    [SerializeField] private PageContent pageContent;

    private void Start()
    {
        LoadPage();
    }

    private void LoadPage()
    {
        titleText.text = pageContent.title;
        contentText.text = pageContent.content;
    }
}
