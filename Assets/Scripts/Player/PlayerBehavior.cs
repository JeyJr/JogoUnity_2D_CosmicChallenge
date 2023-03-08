using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody myRB;

    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float screenLimit = 10;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float smoothVelocity = 0f;

    private bool hitEnemy = false;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hitEnemy)
        {
            hitEnemy = true;
            StartCoroutine(StartEndGame());
            GameEvent.GetInstance().PlaySFXClip(SFXClip.hit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            hitEnemy = false;
        }
    }

    IEnumerator StartEndGame()
    {
        yield return null;
        GameController.GetInstance().SetGameState(GameState.EndGame);
    }

    private void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

        float targetVelocity = input.x * moveSpeed;
        float smoothedVelocity = Mathf.SmoothDamp(myRB.velocity.x, targetVelocity, ref smoothVelocity, smoothTime);



        Vector3 move = new Vector3(smoothedVelocity, 0f, 0f) * Time.deltaTime;
        move.x = Mathf.Clamp(transform.position.x + move.x, -screenLimit, screenLimit) - transform.position.x;
        //para evitar que posição X atual seja adiciona 2X, removemos ela no move

        myRB.MovePosition(transform.position + move);
    }
}
