using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] Trigger ObsticleTrigger;


    [SerializeField] Vector3 startPos;
    [SerializeField] Quaternion startRotation;
    [SerializeField] Vector3 startScale;

    private bool alive = true;
    private Moves movesScript;

    // Start is called before the first frame update
    void Start()
    {


        GameManager.OnNewTurn += OnNewTurn;
        GameManager.OnStageReset += ResetPlayer;

        movesScript = transform.GetComponent<Moves>();

        startPos = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;

        alive = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            if (ObsticleTrigger.IsTriggered() && movesScript.IsPlayingMoves())
            {
                SetAsDead();
            }
        } 
    }

    private void OnNewTurn()
    {
        Debug.Log("Player: New Turn");
        movesScript.SetActiveCard(GameManager.Instance.GetActivePlayerCard());
        movesScript.SetPlay(true);
    }

    void SetAsDead()
    {

        Debug.Log("Player: Im dead :(");
        alive = false;
        movesScript.SetPlay(false);
    }

    private void ResetPlayer()
    {
        transform.position = startPos;
        transform.rotation = startRotation;
        transform.localScale = startScale;

        alive = true;
        movesScript.SetPlay(false);
        movesScript.ResetMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Goal"))
        {
            GameManager.Instance.PlayerReachedGoal();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            SetAsDead();
        }
    }
}
