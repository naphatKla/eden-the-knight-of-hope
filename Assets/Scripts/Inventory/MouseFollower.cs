using UnityEngine;

namespace Inventory
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera mainCam;

        [SerializeField] private UIInventoryItem item;

        public void Awake()
        {
            canvas = transform.parent.GetComponent<Canvas>();
            mainCam = Camera.main;
            item = GetComponentInChildren<UIInventoryItem>();
        }
    
        public void SetData(Sprite sprite, int quantity)
        {
            item.SetData(sprite, quantity);
        }

        private void Update()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform, 
                Input.mousePosition, 
                canvas.worldCamera, 
                out position);
            transform.position = canvas.transform.TransformPoint(position);
        }
    
        public void Toggle(bool val)
        {
            Debug.Log($"Item toggled {val}"); 
            gameObject.SetActive(val);
        }
    }
}
