using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField] private float movmentSpeed;
        [SerializeField] private LayerMask solidLayerMask;
        [SerializeField] private LayerMask interactItem;
        [SerializeField] private LayerMask damageableItem;
        [SerializeField] private float attackRange;

        [SerializeField] private PlayerSpeechBubble speechBubble;
        public PlayerSpeechBubble SpeechBubble => speechBubble;
        public Vector2 LastDirectionVector { get; private set; }
        public Vector3 MovementDirection { get; private set; }
        public IDamageable EnemyTarget { get; private set; }
        public PlayerStats PlayerStats { get; private set; }

        public bool IsAttacking { get; private set; }

        private float MovmentDistance => Time.deltaTime * movmentSpeed;
        private Vector2 boxColiderSize = new Vector2(.4f, .4f);
        private readonly float boxAngle = 1f;
        private List<Interactable> Interactable;

        private bool isWalking;
        private int targetScene;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            PlayerStats = GetComponent<PlayerStats>();

            DontDestroyOnLoad(gameObject);

            Interactable = new List<Interactable>();
        }

        private void Start() => GameInput.Instance.OnInteracted += GameInput_OnInteracted;
      
        private void GameInput_OnInteracted(object sender, System.EventArgs e)
        {
            if (!UIManager.Instance.IsNoneState()) return;

            foreach(Interactable interactable in Interactable)
            {
                interactable.Interact(this);
            }
            Interactable.Clear();
        }

        private void Update()
        {
            HandleMovment();
            HandleInteract();
            HandleAttack();    
        }

        private void HandleMovment()
        {
            Vector2 inputVector = GameInput.Instance.GetMovmentVectorNormalized();

            if (inputVector != Vector2.zero)
                LastDirectionVector = inputVector;

            MovementDirection = new Vector3(inputVector.x, inputVector.y, 0);

            bool canMove = !Physics2D.BoxCast(GetColiderPosition(), boxColiderSize, boxAngle, MovementDirection, MovmentDistance, solidLayerMask);

            if (!canMove)
            {
                Vector3 moveDirX = new Vector3(MovementDirection.x, 0, 0).normalized;
                canMove = MovementDirection.x != 0 && !Physics2D.BoxCast(GetColiderPosition(), boxColiderSize, boxAngle, moveDirX, MovmentDistance, solidLayerMask);
                if (canMove)
                    MovementDirection = moveDirX;
                else
                {
                    Vector3 moveDirY = new Vector3(0, MovementDirection.y, 0).normalized;
                    canMove = MovementDirection.y != 0 && !Physics2D.BoxCast(GetColiderPosition(), boxColiderSize, boxAngle, moveDirY, MovmentDistance, solidLayerMask);

                    if (canMove)
                        MovementDirection = moveDirY;
                }
            }

            if (canMove)
                transform.Translate(MovementDirection * MovmentDistance, Space.World);

            isWalking = (MovementDirection != Vector3.zero);

        }

        private void HandleInteract()
        {
            RaycastHit2D[] raycastHit = Physics2D.BoxCastAll(GetColiderPosition(), boxColiderSize, boxAngle, LastDirectionVector,  MovmentDistance, interactItem);
            Interactable.Clear();
      
            foreach (RaycastHit2D raycastHit2D in raycastHit)
            {
                if (raycastHit2D)
                {
                    if (raycastHit2D.transform.TryGetComponent(out Interactable component))
                    {
                        if (!Interactable.Contains(component))
                            Interactable.Add(component);
                    }                    
                }
                else
                  Interactable.Clear();
            }         
        }

        private void HandleAttack()
        {
            if (InventoryManager.Instance.UsedItem?.itemSO is not SwordSO) return;

            RaycastHit2D raycastHit = Physics2D.BoxCast(GetColiderPosition(), boxColiderSize, boxAngle, LastDirectionVector, attackRange, damageableItem);
            if (raycastHit)
                EnemyTarget = raycastHit.transform.GetComponent<IDamageable>();
            else
                EnemyTarget = null;

            if (InventoryManager.Instance.ItemIsUsed)
                IsAttacking = true;
            else
                IsAttacking = false;
        }

        private Vector3 GetColiderPosition() => transform.position - new Vector3(0, .35f, 0);

        public bool IsWalking() => isWalking;

        public void SetTargetSecne(int target) => targetScene = target;

        public int GetTargetScene() => targetScene;

        public T CheckInteractableType<T>()
        {
            foreach(Interactable interactables in Interactable)
            {
                if (interactables is T obj) 
                    return obj;
            }
            return default;
        }
    }
}
