using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ILayoutElement {}

public interface IActionElement: ILayoutElement {
    void AddListener(Action onClick);

    void RemoveListener(Action onClick);

    void Dispose();
}

public abstract class ScreenLayout : MonoBehaviour {
    
    private HashSet<ILayoutElement> _elements = new ();

    protected void CollectLayout() {
        _elements = GetComponentsInChildren<ILayoutElement>().ToHashSet();
    }

    public bool TryGetElement<T>(out T element) where T : ILayoutElement {
        element = _elements.OfType<T>().FirstOrDefault();
        return element != null;
    }

    public T GetElement<T>() where T : ILayoutElement => _elements.OfType<T>().FirstOrDefault();

    public void OnButtonClick<T>(Action onClick) where T : IActionElement {
        if (TryGetElement<T>(out var element)) {
            element.AddListener(onClick);
        }
    }
    
    public void OnButtonDispose<T>(Action onClick) where T : IActionElement {
        if (TryGetElement<T>(out var element)) {
            element.RemoveListener(onClick);
        }
    }

    protected void DisposeAllActions() {
        foreach (var element in _elements.OfType<IActionElement>()) {
            element.Dispose();
        }
    }

    public void Show() {
        CollectLayout();
        gameObject.SetActive(true);
    }

    public void Hide() {
        DisposeAllActions();
        gameObject.SetActive(false);
    }

}