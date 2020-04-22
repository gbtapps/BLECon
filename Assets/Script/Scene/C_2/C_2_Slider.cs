using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class C_2_Slider : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
        var button = transform.parent.parent.Find("Btn_Next").GetComponent<Button>();
        button.interactable = true;
    }
}
