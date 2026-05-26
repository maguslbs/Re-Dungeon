using System.Collections;
using UnityEditor;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SkinnedMeshRenderer smr; //3d

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .2f;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject hitVfx;


    private void Awake()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>(); //3d
        originalMaterial = smr.material;
    }

    public void CreateOnHitVFX(Transform target)
    {
        GameObject vfx = Instantiate(hitVfx, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;
    }

    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);

        onDamageVfxCoroutine = StartCoroutine(OnDamageVFXCo());
    }

    private IEnumerator OnDamageVFXCo()
    {
        if (smr == null) yield break;

        smr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVfxDuration);

        smr.material = originalMaterial;
    }
}
