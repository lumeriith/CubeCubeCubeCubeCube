using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGib : MonoBehaviour
{

    private float determinedRandom1;
    private float determinedRandom2;
    private float determinedRandom3;
    private float determinedRandom4;
    private float determinedRandom5;
    private GameObject explode;
    private Rigidbody rb;
    public float score = 10f;
    //귀찮아
    //개발자는 졸리면 코드가 점점 더 스파게티화 됩니다
    private void Start()
    {
        determinedRandom1 = Random.Range(1.5f, 2.1f) * 0.8f;
        determinedRandom2 = Random.Range(1.9f, 2.5f) * 0.8f;
        determinedRandom3 = Random.Range(1.2f, 1.8f) * 0.8f;
        determinedRandom4 = Random.Range(1.0f, 1.6f) * 0.8f;
        determinedRandom5 = Random.Range(400f, 600f);
        explode = transform.Find("Explode").gameObject;
        explode.SetActive(false);
        rb = GetComponent<Rigidbody>();
        rb.velocity = Random.insideUnitSphere.normalized * 10f;
    }

    void Update()
    {
        transform.Rotate(new Vector3(Mathf.Sin(Time.time * determinedRandom1), Mathf.Sin(Time.time), Mathf.Sin(Time.time * determinedRandom2)) * determinedRandom5 * Time.deltaTime * Mathf.Sin(Time.time * determinedRandom3) * Mathf.Sin(Time.time * determinedRandom4));
        rb.velocity = Vector3.MoveTowards(rb.velocity, (Player.instance.transform.position - transform.position).normalized * 45f, Time.deltaTime * 80f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            explode.SetActive(true);
            explode.transform.parent = null;
            Destroy(explode, 3f);
            Destroy(gameObject);
            GameManager.instance.AddScore(score);
        }
    }
}
