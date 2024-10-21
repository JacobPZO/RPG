using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviourPun
{
    [HideInInspector]
    public int id;
    [Header("Info")]
    public float moveSpeed;
    public int gold;
    public int curHp;
    public int maxHp;
    public bool dead;
    [Header("Attack")]
    public int damage;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;
    [Header("Components")]
    public Rigidbody2D rig;
    public Player photonPlayer;
    public SpriteRenderer sr;
    public Animator weaponAnim;
    // local player
    public static PlayerController me;

    void Move()
    {
        // get the horizontal and vertical inputs
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        // apply that to our velocity
        rig.velocity = new Vector2(x, y) * moveSpeed;
    }

    // melee attacks towards the mouse
    void Attack()
    {
        lastAttackTime = Time.time;
        // calculate the direction
        Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        // shoot a raycast in the direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir, dir, attackRange);
        // did we hit an enemy?
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
        {
            // get the enemy and damage them
        }
        // play attack animation
        weaponAnim.SetTrigger("Attack");
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        curHp -= damage;
        // update the health bar
        if (curHp <= 0)
            Die();
        else
        {
            StartCoroutine(DamageFlash());
            IEnumerator DamageFlash()
            {
                sr.color = Color.red;
                yield return new WaitForSeconds(0.05f);
                sr.color = Color.white;
            }
        }
    }

    void Die()
    {
        dead = true;
        rig.isKinematic = true;
        transform.position = new Vector3(0, 99, 0);
        Vector3 spawnPos = GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)].position;
        StartCoroutine(Spawn(spawnPos, GameManager.instance.respawnTime));
    }

    [PunRPC]
    void Heal(int amountToHeal)
    {
        curHp = Mathf.Clamp(curHp + amountToHeal, 0, maxHp);
        // update the health bar
    }
    
    [PunRPC]
    void GiveGold(int goldToGive)
    {
        gold += goldToGive;
        // update the ui
    }

    IEnumerator Spawn(Vector3 spawnPos, float timeToSpawn)
    {
        yield return new WaitForSeconds(timeToSpawn);
        dead = false;
        transform.position = spawnPos;
        curHp = maxHp;
        rig.isKinematic = false;
        // update the health bar
    }

    [PunRPC]
    public void Initialize(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;
        GameManager.instance.players[id - 1] = this;
        // initialize the health bar
        if (player.IsLocal)
            me = this;
        else
            rig.isKinematic = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;
        Move();
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime > attackRate)
            Attack();
        float mouseX = (Screen.width / 2) - Input.mousePosition.x;
        if (mouseX < 0)
            weaponAnim.transform.parent.localScale = new Vector3(-1, 1, 1);
        else
            weaponAnim.transform.parent.localScale = new Vector3(1, 1, 1);

    }
}