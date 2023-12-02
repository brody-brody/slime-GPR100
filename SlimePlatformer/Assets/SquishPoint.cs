using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishPoint : MonoBehaviour
{
    // this is shared among all squish points
    public static bool isPlayerEntered;

    public enum EnterenceType
    {
        Neither,
        IsEnterenceOnly,
        IsExitOnly,
        IsBoth
    }

    // if not currently on point, 
    [SerializeField] private EnterenceType enterenceType;

    [Header("Modifiers")]
    [SerializeField] private float moveSpeed = 0.6f;

    [Header("Exit Variables")]
    [SerializeField] private Direction exitDirection;

    [Header("SFX")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip enterSound;
    [SerializeField] private AudioClip reachPointSound;
    [SerializeField] private AudioClip exitSound;
    [SerializeField] private AudioClip railSound;

    private SethPlayerTest player;

    private void Start()
    {
        player = FindObjectOfType<SethPlayerTest>();
    }

    public enum Direction
    {
        Left = -1,
        Right = 1,
        Up = 2,
        Down = -2,
        None = 0
    }

    [System.Serializable]
    public class SquishPointData
    {
        public Direction direction;
        public SquishPoint point;
    }

    [SerializeField] private List<SquishPointData> squishPoints = new List<SquishPointData>();

    private bool enteredOnSelf;
    [HideInInspector] public bool readyToMove;

    private bool moving;
    private bool exiting;

    void Update()
    {
        // 1) Try to enter is an enterence
        TryEnter();

        // make move if ready
        MakeMove();

        if (!enteredOnSelf && readyToMove && (enterenceType == EnterenceType.IsBoth || enterenceType == EnterenceType.IsExitOnly) && !exiting && isPlayerEntered)
        {
            exiting = true;
            StartCoroutine(Exit());
        }
    }

    private void TryEnter()
    {
        // If not an enterence, can't enter
        if (enterenceType != EnterenceType.IsBoth && enterenceType != EnterenceType.IsEnterenceOnly) return;
        if (isPlayerEntered) return;
        if (exiting) return;


        if (Vector2.Distance(player.transform.position, transform.position) < 0.1f) { }
        else if (Vector2.Distance(player.transform.position, transform.position) > 1.1f || (int)GetInputDirection() != -(int)exitDirection) return;

        isPlayerEntered = true;
        enteredOnSelf = true;

        player.SuspendAll();

        StartCoroutine(Enter());
    }

    private Direction GetInputDirection()
    {
        // get input
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Direction attemptedDir = Direction.None;
        // read dir
        if (xInput != 0) attemptedDir = (Direction)xInput;
        else if (yInput != 0) attemptedDir = (Direction)(yInput * 2);

        return attemptedDir;
    }

    private void MakeMove()
    {
        if (!readyToMove || moving || exiting) return;

        // invalid if distance is too far. assume its a bug :D
        if (Vector2.Distance(player.transform.position, transform.position) > 0.3f) return;

        // get input
        Direction attemptedDir = GetInputDirection();
        if (attemptedDir == Direction.None) return;

        SquishPointData targetPoint = null;
        foreach(SquishPointData data in squishPoints)
        {
            // found matching point in list to move to
            if(data.direction == attemptedDir) {
                targetPoint = data;
                break;
            }
        }

        if (targetPoint == null) Debug.Log("Could not move " + attemptedDir + "!");
        else
        {
            source.clip = railSound;
            source.loop = true;
            source.Play();

            StartCoroutine(MoveToPoint(targetPoint));
        }
    }

    private IEnumerator Enter()
    {
        source.PlayOneShot(enterSound);
        yield return StartCoroutine(LerpToPosition(player.transform.position, transform.position, 0.3f));

        FindObjectOfType<SimpleCamera>().ChangeSmoothing(false);
        readyToMove = true;

        yield return null;
    }

    private IEnumerator Exit()
    {
        source.PlayOneShot(exitSound);

        readyToMove = false;
        Vector3 moveOffset = Vector3.zero;
        switch (exitDirection)
        {
            case Direction.Left:
                moveOffset = new Vector3(-1, 0, 0);
                break;
            case Direction.Right:
                moveOffset = new Vector3(1, 0, 0);
                break;
            case Direction.Down:
                moveOffset = new Vector3(0, -1, 0);
                break;
            case Direction.Up:
                moveOffset = new Vector3(0, 1, 0);
                break;
            default:
                break;
        }

        yield return StartCoroutine(LerpToPosition(player.transform.position, transform.position + moveOffset, 0.5f));

        FindObjectOfType<SimpleCamera>().ChangeSmoothing(true);

        isPlayerEntered = false;
        readyToMove = false;
        exiting = false;
        player.UnsuspendAll();

        yield return null;
    }

    private IEnumerator LerpToPosition(Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            player.transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.transform.position = end;
    }

    private IEnumerator MoveToPoint(SquishPointData data)
    {
        moving = true;

        float distance = Vector2.Distance(player.transform.position, data.point.transform.position);
        float duration = distance / moveSpeed;

        yield return LerpToPosition(player.transform.position, data.point.transform.position, duration);
        CameraShake.instance.Shake(0.34f, 0.2f);

        source.loop = false;
        source.Stop();
        source.PlayOneShot(reachPointSound);

        yield return new WaitForSeconds(0.1f);

        data.point.readyToMove = true;
        readyToMove = false;
        enteredOnSelf = false;
        moving = false;

        yield return null;
    }
}
