using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Normal Attack")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private Animator anim;
    private PlayerScript playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Special Attack")]
    [SerializeField] private float specialCooldown;
    [SerializeField] private float fatigueDuration;
    private bool isFatigued = false;

    [Header("Special Attack Settings")]
    [SerializeField] private int specialFireballCount;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerScript>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        if (Input.GetMouseButtonDown(1) && cooldownTimer > specialCooldown && !isFatigued)
        {
            StartCoroutine(SpecialAttackRoutine());
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;
        ExecuteSingleShot();
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private System.Collections.IEnumerator SpecialAttackRoutine()
    {
        isFatigued = true;
        cooldownTimer = 0;

        for (int i = 0; i < specialFireballCount; i++)
        {
            ExecuteSingleShot();
            yield return new WaitForSeconds(0.1f);
        }

        GetComponent<SpriteRenderer>().color = Color.gray;
        playerMovement.ApplyFatigue(0.3f);

        yield return new WaitForSeconds(fatigueDuration);

        playerMovement.ApplyFatigue(1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        isFatigued = false;
    }
    private void ExecuteSingleShot()
    {
        int index = FindFireball();
        fireballs[index].transform.position = firePoint.position;
        fireballs[index].SetActive(true);
        fireballs[index].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
}