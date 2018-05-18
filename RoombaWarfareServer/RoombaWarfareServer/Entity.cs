
public class Entity
{
    public float PosX { get; set; }
    public float PosY { get; set; }

    public override string ToString()
    {
        return PosX.ToString("0.#") + " " + PosY.ToString("0.#");
    }
}
