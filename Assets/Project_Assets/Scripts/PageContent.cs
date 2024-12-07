using UnityEngine;

[CreateAssetMenu(fileName = "PageContent", menuName = "Page Content")]
public class PageContent : ScriptableObject
{
    public string title;
    [TextArea] public string content;
}
