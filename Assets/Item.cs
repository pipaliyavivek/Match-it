using Sirenix.OdinInspector;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string Name = "Default"; 
    public bool isinHand = false;

    //public Collider SpawnArea;

    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Collider myCol;

    private float m_checkTime;
    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!myCol) myCol = GetComponent<Collider>();
    }
    [Button]
    void Make()
    {
        if (gameObject.layer == 10) return;
        Name = gameObject.name;
        gameObject.layer = 10;
        gameObject.tag = "Item";
        gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<MeshCollider>().convex = true;
    }
    void OnSpawned()
    {        
        transform.localScale = Vector3.one;
        myCol.enabled = true;
        isinHand = false;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void Update()
    {
        if (isinHand)
        {
            if (Input.GetMouseButton(0))
            {
                rb.MovePosition(Vector3.MoveTowards(transform.position,GetWorldPositionOnPlane(),Time.deltaTime*50));
                transform.Rotate(0, 50 * Time.deltaTime, 50 * Time.deltaTime);
            }
        }
    }
    public void Pick()
    {
        if (Door.Instance.Slot1 == this) Door.Instance.Slot1 = null;
        Placed(false);
        isinHand = true;
        rb.useGravity = false;
    }
    public void Release()
    {
        isinHand = false;
        rb.useGravity = true;
    }
    public void Placed(bool status)
    {
       // myCol.enabled = !status;
        rb.isKinematic = status;
    }
    private void OnDisable()
    {
        isinHand = false;
    }
    public Vector3 GetWorldPositionOnPlane()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane zx = new Plane(Vector3.up, new Vector3(0, 2f, 0));
        float distance;
        zx.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
    private void FixedUpdate()
    {
        if (m_checkTime > 2)
        {
            if (Vector3.Distance(transform.position, Vector3.zero) > 100)
            {
                rb.velocity = Vector3.zero;
                var Spawn_point = RandomPointInBounds(GameManager.Instance.SpawnArea.bounds);
                transform.position = Spawn_point;
            }
            m_checkTime = 0;
        }
        else
        {
          m_checkTime +=Time.deltaTime;
        }
        
    }
}
