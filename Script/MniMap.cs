using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MniMap : MonoBehaviour, IPointerClickHandler
{

    public Camera miniMapCam;
    public GameObject mainCam;
    public RectTransform miniMapRect;
  
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localClick;
        

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(miniMapRect, eventData.position, eventData.pressEventCamera, out localClick))
            return;

        // Chuyển đổi tọa độ UI sang tọa độ thế giới mini map
        float width = miniMapRect.rect.width;
        float height = miniMapRect.rect.height;

        float x = (localClick.x + width / 2) / width;
        float y = (localClick.y + height / 2) / height;

        // Chuyển sang tọa độ thế giới
        Ray ray = miniMapCam.ViewportPointToRay(new Vector3(x, y, 0));
        RaycastHit hit;
    

       
        if (Physics.Raycast(ray, out  hit,Mathf.Infinity,LayerMask.GetMask("Ground"),QueryTriggerInteraction.Ignore))
        {
            Vector3 camPos = mainCam.transform.position;
            camPos.x = hit.point.x;
            camPos.z = hit.point.z;
           
            mainCam.transform.position = camPos;
        }
    }
  
}
