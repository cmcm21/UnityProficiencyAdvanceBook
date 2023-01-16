using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public delegate void ButtonClickedDelegate(int option);

public class Ch1_UIManager : MonoBehaviour
{
    public event ButtonClickedDelegate OnButtonClicked;

    private VisualElement container;
    private void Start()
    {
        VisualElement visualElement = GetComponent<UIDocument>().rootVisualElement;

        visualElement.Q<Button>("LoadScene1").clicked += () => OnButtonClicked?.Invoke(1);
        visualElement.Q<Button>("LoadScene2").clicked += () => OnButtonClicked?.Invoke(2);
        visualElement.Q<Button>("LoadScene3").clicked += () => OnButtonClicked?.Invoke(3);
        
        container = visualElement.Q<VisualElement>("Container");
    }

    public void Hide()
    {
        container.visible = false;
    }
}
