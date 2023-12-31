using UnityEngine;
using UnityEngine.UI;

namespace HealthSystem
{
    public class HealthSystem : MonoBehaviour
    {
        #region Declare Variables
        [SerializeField] protected Slider sliderHpPlayer;
        [SerializeField] public float maxHp;
        public float CurrentHp { get; private set; }
        private bool _isDead;
        private Animator _animator;
        protected SpriteRenderer spriteRenderer;
        private static readonly int TakDamage = Animator.StringToHash("TakeDamage");
        #endregion
    
        private void Awake()
        {
            CurrentHp = maxHp;
        }
        protected virtual void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            TryGetComponent(out _animator);
        }

        private void LateUpdate()
        {
            // Lock the canvas UI rotation.
            Transform canvasTransform = sliderHpPlayer.transform.parent;
            canvasTransform.right = Vector3.right;
        }

        #region Methods
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Take damage and reduce the current hp.
        /// </summary>
        /// <param name="damage">Damage taken.</param>
        /// <param name="attacker">Attacker.</param>
        public virtual void TakeDamage(float damage, GameObject attacker = null)
        {
            CurrentHp -= damage;
            CurrentHp = Mathf.Clamp(CurrentHp, 0, maxHp);
            UpdateUI();
        
            /*if(_animator)
                _animator.SetTrigger(TakDamage);*/
            spriteRenderer.color = new Color(1,0.6f,0.6f,1);
            Invoke(nameof(ResetSpriteColor), 0.2f);
        
            if (CurrentHp > 0 || _isDead) return;
            _isDead = true;
            Dead();
        }

        
        /// <summary>
        /// Heal and increase the current hp.
        /// </summary>
        /// <param name="healPoint">Heal amount.</param>
        public void Heal(float healPoint)
        {
            CurrentHp += healPoint;
            CurrentHp = Mathf.Clamp(CurrentHp, 0, maxHp);
            UpdateUI();
        }
    
        
        /// <summary>
        /// Dead and destroy the object.
        /// </summary>
        protected virtual void Dead()
        {
            Destroy(gameObject);
        }
    
        
        /// <summary>
        /// Reset the current hp to max hp.
        /// </summary>
        public void ResetHealth()
        {
            _isDead = false;
            CurrentHp = maxHp;
            UpdateUI();
        }
    
        
        /// <summary>
        /// Update the UI.
        /// </summary>
        private void UpdateUI()
        {
            sliderHpPlayer.value = CurrentHp / maxHp;
        }

        protected void ResetSpriteColor()
        {
            spriteRenderer.color = Color.white;
        }
        #endregion
    }
}
