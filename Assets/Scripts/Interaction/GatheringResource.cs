using System.Collections;
using Inventory;
using PlayerBehavior;
using UnityEngine;

namespace Interaction
{
    public class GatheringResource : InteractableObject
    {
        public GatheringResourceSO resourceData;
        private PolygonCollider2D _collider2D;
        private bool _isDestroying;
        private AudioSource _audioSource;
        
        public void SetData(GatheringResourceSO data)
        {
            resourceData = data;
            countdownTime = resourceData.gatheringTime;
            GetComponent<SpriteRenderer>().sprite = resourceData.sprite[Random.Range(0, resourceData.sprite.Length)];
        }
        protected override void Start()
        {
            base.Start();
            _collider2D = gameObject.AddComponent<PolygonCollider2D>();
            prompt = $"<b>[ E ] {resourceData.name}";
            interactionTextUI.text = prompt;
            SpriteRenderer = GetComponent<SpriteRenderer>();
            if (resourceData)
                SetData(resourceData);
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = true;
        }

        protected override void InteractHandler()
        {
            if (_isDestroying) return;
            base.InteractHandler();
        }

        public override void OnTarget(bool isTarget)
        {
            if (_isDestroying)
            {
                foreach (var interactionIndicator in interactionIndicators)
                        interactionIndicator.SetActive(false);
                return;
            }
            foreach (var interactionIndicator in interactionIndicators)
                interactionIndicator.SetActive(isTarget);
        
            if (!isTarget) return;
            InteractHandler();
        }

        protected override void InteractAction()
        {
            SoundManager.Instance.RandomPlaySound(resourceData.destroySounds);
            
            if (resourceData.point.y > 0)
                GameManager.Instance.AddPoint((int)Random.Range(resourceData.point.x, resourceData.point.y+1));
            
            if (resourceData.itemDrop1.Count > 0)
            {
                GatheringItemDrop gatheringItemDrop = ProjectExtensions.RandomPickOne(resourceData.itemDrop1).obj;
                int quantity = (int)Random.Range(gatheringItemDrop.quantityDrop.x, gatheringItemDrop.quantityDrop.y+1);
                PlayerInventoryController.Instance.InventoryData.AddItem(gatheringItemDrop.item, quantity);
            }
            
            if (resourceData.itemDrop2.Count > 0)
            {
                GatheringItemDrop gatheringItemDrop = ProjectExtensions.RandomPickOne(resourceData.itemDrop2).obj;
                int quantity = (int)Random.Range(gatheringItemDrop.quantityDrop.x, gatheringItemDrop.quantityDrop.y+1);
                PlayerInventoryController.Instance.InventoryData.AddItem(gatheringItemDrop.item, quantity);
            }
            
            if (resourceData.itemDrop3.Count > 0)
            {
                GatheringItemDrop gatheringItemDrop = ProjectExtensions.RandomPickOne(resourceData.itemDrop3).obj;
                int quantity = (int)Random.Range(gatheringItemDrop.quantityDrop.x, gatheringItemDrop.quantityDrop.y+1);
                PlayerInventoryController.Instance.InventoryData.AddItem(gatheringItemDrop.item, quantity);
            }
            
            if (resourceData.itemDrop4.Count > 0)
            {
                GatheringItemDrop gatheringItemDrop = ProjectExtensions.RandomPickOne(resourceData.itemDrop4).obj;
                int quantity = (int)Random.Range(gatheringItemDrop.quantityDrop.x, gatheringItemDrop.quantityDrop.y+1);
                PlayerInventoryController.Instance.InventoryData.AddItem(gatheringItemDrop.item, quantity);
            }
            
            if (resourceData.itemDrop5.Count > 0)
            {
                GatheringItemDrop gatheringItemDrop = ProjectExtensions.RandomPickOne(resourceData.itemDrop5).obj;
                int quantity = (int)Random.Range(gatheringItemDrop.quantityDrop.x, gatheringItemDrop.quantityDrop.y+1);
                PlayerInventoryController.Instance.InventoryData.AddItem(gatheringItemDrop.item, quantity);
            }

            StartCoroutine(FadeAnimWhenDestroy());
        }

        protected override IEnumerator CountDownAndInteract(float time)
        {
            if (resourceData.gatheringSounds.Length > 0)
            {
                AudioClip audioClip = resourceData.gatheringSounds[Random.Range(0, resourceData.gatheringSounds.Length)];
                _audioSource.clip = audioClip;
                _audioSource.Play();
            }
            
            float progressionTimeLeft = time;
            progressBar.gameObject.SetActive(true);
        
            while (progressionTimeLeft > 0)
            {
                progressBar.value = progressionTimeLeft / time;
                progressionTimeLeft -= Time.deltaTime;
                yield return null;

                PlayerInteractSystem.Instance.isStopMove = time - progressionTimeLeft <= 0.3f;
                if (progressionTimeLeft < 0.15f) continue;
                if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) continue;
                if (PlayerInteractSystem.Instance.isStopMove) continue;
                progressBar.gameObject.SetActive(false);
                interactCoroutine = null;
                PlayerInteractSystem.Instance.isStopMove = false;
                _audioSource.Stop();
                yield break;
            }
        
            InteractAction();
            progressBar.gameObject.SetActive(false);
            interactCoroutine = null;
            progressBar.gameObject.SetActive(false);
            PlayerInteractSystem.Instance.isStopMove = false;
        }

        IEnumerator FadeAnimWhenDestroy()
        {
            PlayerInteractSystem.Instance.isStopMove = false;
            progressBar.gameObject.SetActive(false);
            _isDestroying = true;
            _collider2D.isTrigger = true;
            float timeCount = 0;
            while (timeCount < 0.5f)
            {
                Color color = SpriteRenderer.color;
                color.a = 1 - timeCount / 0.5f;
                SpriteRenderer.color = color;
                timeCount += Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
