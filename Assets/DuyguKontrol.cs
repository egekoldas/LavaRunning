using UnityEngine;

public class DuyguKontrol : MonoBehaviour
{
    public Animator anim;

    public void UzulmeYap() { anim.SetFloat("heyecan", 0f); }
    public void BeklemeYap() { anim.SetFloat("heyecan", 1f); }
    public void SevinmeYap() { anim.SetFloat("heyecan", 2f); }
    public void DansEt() { anim.SetFloat("heyecan", 3f); }
}