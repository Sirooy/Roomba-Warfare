
public abstract class DynamicEntity : Entity
{
    public static Image SpriteSheet =
        new Image(@"resources\images\sprite_sheet_text.png", 64, 96);

    protected float speed;

    public virtual void Update(float deltaTime) { }
}

