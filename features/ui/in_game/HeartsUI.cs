using Godot;
using System.Collections.Generic;

public partial class HeartsUI : Control
{
    [Export] public Texture2D HeartFull;
    [Export] public Texture2D HeartEmpty;
    [Export] public HealthComponent HealthComponent; // drag your HealthComponent here
    [Export] public Vector2 HeartSize = new(16, 16); // change in Inspector

    private HBoxContainer _container;
    private List<TextureRect> _heartIcons = new();

    public override void _Ready()
    {
        _container = GetNode<HBoxContainer>("HBoxContainer");

        if (HealthComponent != null)
        {
            HealthComponent.HealthChanged += OnHealthChanged;
            GenerateHearts(HealthComponent.MaxHearts);
        }
    }

    private void GenerateHearts(int maxHearts)
    {
        // clear old icons
        foreach (Node child in _container.GetChildren())
            child.QueueFree();
        _heartIcons.Clear();

        // create new icons and set their size
        for (int i = 0; i < maxHearts; i++)
        {
            var heart = new TextureRect
            {
                Texture = HeartFull, // AtlasTexture veya Texture2D olabilir
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                CustomMinimumSize = HeartSize
            };
            _container.AddChild(heart);
            _heartIcons.Add(heart);
        }

        // update immediately to current health
        OnHealthChanged(HealthComponent?.CurrentHearts ?? maxHearts, maxHearts);
    }

    private void OnHealthChanged(int current, int max)
    {
        // if max changed, rebuild
        if (_heartIcons.Count != max)
            GenerateHearts(max);

        // update textures
        for (int i = 0; i < _heartIcons.Count; i++)
            _heartIcons[i].Texture = (i < current) ? HeartFull : HeartEmpty;
    }
}
