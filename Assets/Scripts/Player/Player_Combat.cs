using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Parry Details")]
    [SerializeField] private float parryRecovery = .2f;

    public bool ParryPerformed()
    {
        bool hasPerformedParry = false;

        foreach(var target in GetDetectedColliders())
        {
            IParryable parryable = target.GetComponent<IParryable>();

            if (parryable == null)
                continue;

            if(parryable.CanBeParried)
            {
                parryable.HandleParry();
                hasPerformedParry = true;
            }
        }

        return hasPerformedParry;
    }

    public float GetParryRecoveryDuration() => parryRecovery;
}
