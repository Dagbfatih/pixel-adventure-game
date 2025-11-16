using Godot;

public partial class HealthComponent : Node
{
    // Signals
    [Signal] public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);
    [Signal] public delegate void DiedEventHandler();

    // Private fields
    private int _maxHearts = 3;
    private int _currentHearts = 0;

    // Properties
    [Export]
    public int MaxHearts
    {
        get => _maxHearts;
        set
        {
            _maxHearts = Mathf.Max(1, value); // At least 1 heart
            if (_currentHearts > _maxHearts)
            {
                CurrentHearts = _maxHearts;
            }
            EmitSignal(SignalName.HealthChanged, _currentHearts, _maxHearts);
        }
    }

    [Export]
    public int CurrentHearts
    {
        get => _currentHearts;
        set
        {
            int oldHearts = _currentHearts;
            _currentHearts = Mathf.Clamp(value, 0, MaxHearts);

            if (oldHearts != _currentHearts)
            {
                EmitSignal(SignalName.HealthChanged, _currentHearts, MaxHearts);
            }

            if (_currentHearts <= 0 && oldHearts > 0)
            {
                EmitSignal(SignalName.Died);
            }
        }
    }

    // Methods
    public override void _Ready()
    {
        CurrentHearts = MaxHearts;
    }

    public void TakeDamage(int amount = 1)
    {
        if (amount > 0)
        {
            CurrentHearts -= amount;
            GD.Print($"{GetParent().Name} lost {amount} heart(s). Current Hearts: {CurrentHearts}");
        }
    }

    public void Heal(int amount = 1)
    {
        if (amount > 0)
        {
            CurrentHearts += amount;
            GD.Print($"{GetParent().Name} healed {amount} heart(s). Current Hearts: {CurrentHearts}");
        }
    }
}