using Internal;
using UnityEngine;

public class CitizenOpenVariables : MonoBehaviour
{
    public Transform CitizenTransform;
    public Animator CitizenAnimator;
    public Rigidbody CitizenRB;

    [SerializeField] public GameObject[] CitizenWaypoints;
    private void Awake()
    {
        CitizenTransform = GetComponent<Transform>();
        CitizenAnimator = GetComponent<Animator>();
        CitizenRB = GetComponent<Rigidbody>();
        Locator.Register<CitizenOpenVariables>(this);
    }
}
