   using System.Collections.Generic;
   using UnityEngine;

   public class ScrollableList : MonoBehaviour
   {
       public GameObject listItemPrefab;
       public int initialItemCount = 10;
       public float itemSpacing = 1.5f;
       public Camera vrCamera;

       private List<GameObject> items = new List<GameObject>();
       private float scrollSpeed = 5f;
       private float currentScroll = 0f;
       private float maxScroll;

       void Start()
       {
           PopulateInitialItems();
           CalculateMaxScroll();
       }

       void Update()
       {
           HandleInput();
       }

       void PopulateInitialItems()
       {
           for (int i = 0; i < initialItemCount; i++)
           {
               AddItem();
           }
       }

       void AddItem()
       {
           GameObject newItem = Instantiate(listItemPrefab, transform);
           newItem.transform.localPosition = new Vector3(0, -items.Count * itemSpacing, 0);
           // Здесь можно добавить код для установки содержимого ячейки (изображение, текст)
           items.Add(newItem);
       }

       void CalculateMaxScroll()
       {
           // Предположим, что мы знаем общее количество элементов
           maxScroll = items.Count * itemSpacing;
       }

       void HandleInput()
       {
           float scrollInput = Input.GetAxis("Mouse ScrollWheel");
           currentScroll += scrollInput * scrollSpeed;

           currentScroll = Mathf.Clamp(currentScroll, 0, maxScroll);

           transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + scrollInput * scrollSpeed, transform.localPosition.z);

           // Логика добавления новых ячеек при прокрутке
           if (currentScroll > (items.Count - 5) * itemSpacing)
           {
               AddItem();
               CalculateMaxScroll();
           }
       }
   }
