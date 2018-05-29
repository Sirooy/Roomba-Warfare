using SDL2;

/*This class handles the spectator control of the camera*/
public class SpectatorCameraController : DynamicEntity
{
    public bool IsActive { get; set; }

    private readonly uint widthLimit;
    private readonly uint heightLimit;

    public SpectatorCameraController()
    {
        speed = 5;
        IsActive = false;
        widthLimit = (uint)(Hardware.ScreenWidth / 8);
        heightLimit = (uint)(Hardware.ScreenHeight / 8);
    }

    //Updates the position based on the mouse position
    public override void Update(float deltaTime)
    {
        SDL.SDL_GetMouseState(out int mouseX, out int mouseY);

        //Checks if the mouse coordinates are in the bounds of the screen
        //and move the entity (The closer the faster it moves)
        if (mouseX > Hardware.ScreenWidth - widthLimit)
            PosX += (speed * deltaTime * 2);
        else if (mouseX > Hardware.ScreenWidth - widthLimit * 2)
            PosX += (speed * deltaTime);
        else if (mouseX < widthLimit)
            PosX -= (speed * deltaTime * 2);
        else if (mouseX < widthLimit * 2)
            PosX -= (speed * deltaTime);

        //Checks that the entity doesnt get out of the middle of the screen
        if (PosX < Hardware.ScreenWidth / 2)
            PosX = Hardware.ScreenWidth / 2;
        else if (PosX > Map.Width - Hardware.ScreenWidth / 2)
            PosX = Map.Width - Hardware.ScreenWidth / 2;

        //Same with the y axis
        if (mouseY > Hardware.ScreenHeight - heightLimit)
            PosY += (speed * deltaTime * 2);
        else if (mouseY > Hardware.ScreenHeight - heightLimit * 2)
            PosY += (speed * deltaTime);
        else if (mouseY < heightLimit)
            PosY -= (speed * deltaTime * 2);
        else if (mouseY < heightLimit * 2)
            PosY -= (speed * deltaTime);

        if (PosY < Hardware.ScreenHeight / 2)
            PosY = Hardware.ScreenHeight / 2;
        else if (PosY > Map.Height - Hardware.ScreenHeight / 2)
            PosY = Map.Height - Hardware.ScreenHeight / 2;
    }
}
