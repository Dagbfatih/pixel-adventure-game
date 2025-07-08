using Godot;

public partial class HealthComponent : Node
{
	// Signals
	[Signal] public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);
	[Signal] public delegate void DiedEventHandler();

	// Private fields
	private int _maxHealth = 100;
	private int _currentHealth = 0;

	// Properties
	[Export]
	public int MaxHealth
	{
		get => _maxHealth;
		set
		{
			_maxHealth = Mathf.Max(0, value);

			if (_currentHealth > _maxHealth)
			{
				CurrentHealth = _maxHealth;
			}
			EmitSignal(SignalName.HealthChanged, _currentHealth, _maxHealth);
		}
	}

	[Export]
	public int CurrentHealth
	{
		get => _currentHealth;
		set
		{
			int oldHealth = _currentHealth;

			_currentHealth = Mathf.Clamp(value, 0, MaxHealth);

			if (oldHealth != _currentHealth)
			{
				EmitSignal(SignalName.HealthChanged, _currentHealth, MaxHealth);
			}

			if (_currentHealth <= 0 && oldHealth > 0)
			{
				EmitSignal(SignalName.Died);
			}
		}
	}

	// Methods
	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
	}

	public void TakeDamage(int amount)
	{
		if (amount > 0)
		{
			CurrentHealth -= amount;
			GD.Print($"{GetParent().Name} took {amount} damage. Current Health: {CurrentHealth}");
		}
	}

	public void Heal(int amount)
	{
		if (amount > 0)
		{
			CurrentHealth += amount;
			GD.Print($"{GetParent().Name} healed {amount} health. Current Health: {CurrentHealth}");
		}
	}
}
