using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class KarakterHareket : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;
    public Transform kamera;

    [Header("Hareket Ayarlarý")]
    public float yurumeHizi = 3f;
    public float kosmaHizi = 6f;
    public float ziplamaGucu = 1.5f;
    public float yerCekimi = -9.81f;

    private float donusYumusatma = 0.1f;
    private float donusHiziAnimasyon;
    private Vector3 dikeyHiz;

    private bool duyguOynuyor = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (kamera == null) kamera = Camera.main.transform;
    }

    void Update()
    {
        if (controller.isGrounded && dikeyHiz.y < 0)
        {
            dikeyHiz.y = -2f;
        }

        Vector3 yatayHareket = Vector3.zero;

        if (!duyguOynuyor)
        {
            yatayHareket = HareketHesapla();

            // ÝÞTE GARANTÝLÝ ZIPLAMA KODU
            if (Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                dikeyHiz.y = Mathf.Sqrt(ziplamaGucu * -2f * yerCekimi);
                anim.SetTrigger("Zipla"); // Crossfade yerine Trigger kullanýyoruz!
            }
        }

        dikeyHiz.y += yerCekimi * Time.deltaTime;
        controller.Move((yatayHareket + dikeyHiz) * Time.deltaTime);

        if (controller.isGrounded && !duyguOynuyor)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(DuyguZamanlayici(2f, 3f));
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(DuyguZamanlayici(3f, 5f));
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(DuyguZamanlayici(0f, 3f));
            }
        }
    }

    Vector3 HareketHesapla()
    {
        float yatay = Input.GetAxisRaw("Horizontal");
        float dikey = Input.GetAxisRaw("Vertical");
        Vector3 yon = new Vector3(yatay, 0f, dikey).normalized;

        bool kosuyorMu = Input.GetKey(KeyCode.LeftShift);
        float guncelHiz = kosuyorMu ? kosmaHizi : yurumeHizi;

        Vector3 hareketYonu = Vector3.zero;

        if (yon.magnitude >= 0.1f)
        {
            float hedefAci = Mathf.Atan2(yon.x, yon.z) * Mathf.Rad2Deg + kamera.eulerAngles.y;
            float aci = Mathf.SmoothDampAngle(transform.eulerAngles.y, hedefAci, ref donusHiziAnimasyon, donusYumusatma);
            transform.rotation = Quaternion.Euler(0f, aci, 0f);

            hareketYonu = Quaternion.Euler(0f, hedefAci, 0f) * Vector3.forward;
            hareketYonu = hareketYonu.normalized * guncelHiz;

            if (controller.isGrounded)
            {
                anim.SetFloat("Hiz", kosuyorMu ? 1f : 0.5f);
            }
        }
        else
        {
            if (controller.isGrounded)
            {
                anim.SetFloat("Hiz", 0f);
            }
        }

        return hareketYonu;
    }

    IEnumerator DuyguZamanlayici(float heyecanDegeri, float sure)
    {
        duyguOynuyor = true;
        anim.SetFloat("Hiz", 0f);
        anim.SetFloat("heyecan", heyecanDegeri);
        anim.CrossFade("Duygular", 0.1f);

        yield return new WaitForSeconds(sure);

        anim.SetFloat("heyecan", 1f);
        anim.CrossFade("Hareket", 0.1f);
        duyguOynuyor = false;
    }
}