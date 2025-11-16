using Godot;

// Bu sınıf, HurtState'e geçiş yapılırken ihtiyaç duyulan veriyi (KnockbackVektörü) taşır.
public partial class HurtStateParams : StateParams
{
    public int Damage { get; }
    public Vector2 KnockbackVector { get; set; }

    public HurtStateParams(Vector2 knockbackVector, int damage)
    {
        KnockbackVector = knockbackVector;
        Damage = damage;
    }
}

public partial class HurtState : State
{
    [Export(PropertyHint.Range, "0.1, 1.0, 0.05")]
    public float StunTime = 0.3f; // Süre (saniye) boyunca karakterin sersemlemesi
    [Export]
    public float Gravity = 1800f;
    [Export]
    public float KnockbackDecay = 0.9f; // Knockback hızının yavaşlama faktörü

    private float _timeInState = 0f;

    public HurtState()
    {
        this.Key = "Hurt";
    }

    /// <summary>
    /// Tip güvenli parametre alan Enter metodu. HurtStateParams'tan knockback verisini alır ve uygular.
    /// </summary>
    public override void Enter(StateParams parameters = null)
    {
        GD.Print("HURTTTTTT", Actor.Velocity);

        _timeInState = 0f;

        // Gelen parametrenin HurtStateParams olduğundan emin ol
        if (Actor is CharacterBody2D body && parameters is HurtStateParams hurtParams)
        {
            Vector2 knockbackVector = hurtParams.KnockbackVector;

            // Knockback'i uygula
            body.Velocity = knockbackVector;

            // Karakteri darbe yönünden uzağa doğru çevir
            Sprite.FlipH = knockbackVector.X < 0;

            // Can azaltma burada
            if (Actor.HasNode("HealthComponent") && Actor is IHealth)
            {
                var health = Actor.GetNode<HealthComponent>("HealthComponent");
                health.TakeDamage(hurtParams.Damage);
            }
        }
        
        Sprite?.Play("hurt");
    }

    public override void StatePhysicsProcess(double delta)
    {
        if (Actor is not CharacterBody2D body) return;

        _timeInState += (float)delta;

        Vector2 velocity = body.Velocity;

        // Yer çekimi uygula
        velocity.Y += Gravity * (float)delta;

        // Sersemleme süresi dolmadan yatay hızı yavaşlat (Hızla yavaşça durur)
        if (_timeInState < StunTime)
        {
            velocity.X *= KnockbackDecay;
        }
        else
        {
            // Sersemleme süresi doldu.
            GD.Print("HURT STATE OVER, GOING TO IDLE OR FALL", body.IsOnFloor());
            // Eğer zemindeyse Idle'a geçiş yap.
            if (body.IsOnFloor())
            {
                velocity.X = 0; // Durması için hızı sıfırla
                FSM.ChangeState("Idle");
            }
            else
            {
                // Havadaysa, yatay hızı sıfırla ve Fall State'e geçiş yap, yerçekimi devreye girsin.
                velocity.X = 0;
                FSM.ChangeState("Fall");
            }
        }

        body.Velocity = velocity;
        body.MoveAndSlide();
    }

    public override void Exit()
    {
        // Vurulma anı bittikten sonra i-frame (geçici dokunulmazlık) gibi ek temizlik mantığı buraya eklenebilir.
    }
}
