using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 2f;
    [SerializeField] private LayerMask _interactableMask;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;
    private CharacterStats playerStats;

    private void Awake()
    {
        playerStats = this.GetComponent<CharacterStats>();
    }

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders,
            _interactableMask);

        if (_numFound > 0)
        {
            IInteractable interactable = null;

            for (int i = 0; i < _numFound; ++i)
            {
                interactable = _colliders[i].GetComponent<IInteractable>();
                bool hasEnoughMoney = playerStats.GetCurrency() >= interactable.GetPrice();

                if (hasEnoughMoney) break;

                interactable = null;
            }

            if (interactable != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                interactable.Interact(this);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
