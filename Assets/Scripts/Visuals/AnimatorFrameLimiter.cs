using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorFrameLimiter: MonoBehaviour
{
    [Min(1)]
    public int frameRate = 12;
    [SerializeField] private Animator animator;

    protected float framePeriod => 1.0f / frameRate;
    private float frameTimer;

    void Reset()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        animator.enabled = false;
    }


    private void Update()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer > framePeriod)
        {
            frameTimer -= framePeriod;
            animator.Update(framePeriod);
        }
    }
}