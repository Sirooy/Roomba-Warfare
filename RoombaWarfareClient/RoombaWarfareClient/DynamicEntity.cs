
public abstract class DynamicEntity : Entity
{
    public static Image SpriteSheet =
        new Image("resources/images/dynamic_entity_sprite_sheet.png", 64, 96);

    protected float speed;

    public virtual void Update(float deltaTime) { }
}

