using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    [Header("Hedef ve Dönüþ Ayarlarý")]
    [Tooltip("First Person Controller objesini buraya sürükleyin")]
    public Transform hedefOyuncu;
    public float donusHizi = 5f; // Gözün sana dönme hýzý

    void Update()
    {
        // Eðer hedef (oyuncu) atanmamýþsa kod hata vermesin diye kontrol ediyoruz
        if (hedefOyuncu == null)
            return;

        // 1. Gözden senin karakterine doðru olan yön vektörünü buluyoruz
        Vector3 bakisYonu = hedefOyuncu.position - transform.position;

        // Bazen karakter tam gözün içine girerse (mesafe 0 olursa) hata vermesin diye ufak bir kontrol:
        if (bakisYonu != Vector3.zero)
        {
            // 2. HOCANIN ÝSTEDÝÐÝ KISIM: Quaternion.LookRotation ile hedefe bakýþ açýsýný hesaplýyoruz.
            Quaternion hedefDonus = Quaternion.LookRotation(bakisYonu);

            // 3. Gözün sana aniden robot gibi deðil, ürkütücü ve yavaþ bir þekilde dönmesi için Slerp kullanýyoruz.
            transform.rotation = Quaternion.Slerp(transform.rotation, hedefDonus, donusHizi * Time.deltaTime);
        }
    }
}