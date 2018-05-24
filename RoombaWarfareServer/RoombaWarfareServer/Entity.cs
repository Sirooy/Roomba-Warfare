
public class Entity
{
    public float PosX { get; set; }
    public float PosY { get; set; }

    public void SetPos(float posX, float posY)
    {
        PosX = posX;
        PosY = posY;
    }

    public override string ToString()
    {
        return PosX.ToString("0.#") + " " + PosY.ToString("0.#");
    }
}
