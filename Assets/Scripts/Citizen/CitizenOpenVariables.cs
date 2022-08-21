using Internal;
using UnityEngine;

public class CitizenOpenVariables : MonoBehaviour
{
    public Transform CitizenTransform;
    public Animator CitizenAnimator;
    public Rigidbody CitizenRB;

    [SerializeField] public Transform House;
    [SerializeField] public Transform MeetingPlace;

    [SerializeField] public float CitizenRotationSpeed = 5f;
    [SerializeField] public float CitizenSpeed = 10f;

    [SerializeField] public GameObject[] CitizenWorkAreas;
    private void Awake()
    {
        CitizenTransform = GetComponent<Transform>();
        CitizenAnimator = GetComponent<Animator>();
        CitizenRB = GetComponent<Rigidbody>();
        Locator.Register<CitizenOpenVariables>(this);
    }
}
