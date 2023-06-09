using UnityEngine;

public class EnemyUFOCatcher : MonoBehaviour
{
    public GameObject[] cows;
    public float cowPullSpeed = 5f;
    public float cowCatchRadius = 2f;
    public float cowCatchHeight = 5f;
    public float enemyCatchDistance = 20f;
    public float enemyCatchSpeed = 10f;
    public GameObject cone;

    private GameObject targetCow;
    private Vector3 cowCatchOffset;
    private bool isCatching = false;

    void Start()
    {
        targetCow = null;
        cowCatchOffset = Vector3.up * cowCatchHeight;
        cone.SetActive(false);
    }

    void Update()
    {
        if (!isCatching)
        {
            // Find a random cow within range
            Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * cowCatchRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Cow"))
                {
                    if (targetCow == null)
                    {
                        targetCow = collider.gameObject;
                    }
                    else if (Vector3.Distance(transform.position, collider.transform.position) < Vector3.Distance(transform.position, targetCow.transform.position))
                    {
                        targetCow = collider.gameObject;
                    }
                }
            }

            if (targetCow != null)
            {
                // Move enemy to catch position
                Vector3 cowPosition = targetCow.transform.position;
                Vector3 enemyCatchPosition = new Vector3(cowPosition.x, cowPosition.y + cowCatchHeight, cowPosition.z);

                transform.position = Vector3.MoveTowards(transform.position, enemyCatchPosition, enemyCatchSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, enemyCatchPosition) < 2f)
                {
                    isCatching = true;
                }
            }
        }
        else
        {
            // Pull cow into enemy
            cone.SetActive(!cone.activeSelf);
            Vector3 cowPosition = targetCow.transform.position + cowCatchOffset;
            targetCow.transform.position = Vector3.MoveTowards(targetCow.transform.position, cowPosition, cowPullSpeed * Time.deltaTime);

            // If cow touches enemy, destroy cow and fly away
            if (Vector3.Distance(targetCow.transform.position, transform.position) < 6f)
            {
                Destroy(targetCow);
                transform.Translate(transform.up * 50f * Time.deltaTime);
                isCatching = false;
                cone.SetActive(false);
            }
        }
    }
}
