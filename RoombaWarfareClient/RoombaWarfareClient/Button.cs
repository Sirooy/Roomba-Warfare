using SDL2;

public enum ButtonType : byte
     {  StartButton,ExitButton
     , BlueTeamButton, RedTeamButton
     , SelectAssaultPlayer, SelectCommanderPlayer
     , SelectRusherPlayer, SelectTankPlayer }

public class Button : StaticEntity
{
    public static readonly byte SPRITE_WIDTH = 200;
    public static readonly byte SPRITE_HEIGHT = 100;

    public bool IsClicked { get; set; }

    public Button(ButtonType type)
    {
        IsClicked = false;

        //Assings the texture of the button depending on the type
        switch (type)
        {
            //TO DO (Set the sprite positions)
            case ButtonType.StartButton:
                spriteX = 0;
                spriteY = 0;
                break;

            case ButtonType.ExitButton:
                spriteX = 0;
                spriteY = 0;
                break;

            case ButtonType.SelectAssaultPlayer:
                spriteX = 0;
                spriteY = 0;
                break;

            case ButtonType.SelectCommanderPlayer:
                spriteX = 0;
                spriteY = 0;
                break;

            case ButtonType.SelectRusherPlayer:
                spriteX = 0;
                spriteY = 0;
                break;

            case ButtonType.SelectTankPlayer:
                spriteX = 0;
                spriteY = 0;
                break;
        }
    }

    //Checks if we press the button
    public void HandleEvents(SDL.SDL_Event e)
    {
        IsClicked = false;
        bool mouseInside = false;
        int mouseX, mouseY;

        SDL.SDL_GetMouseState(out mouseX, out mouseY);

        //Checks if the mouse is inside the button when we move 
        //or click with the mouse
        if(e.type == SDL.SDL_EventType.SDL_MOUSEMOTION ||
            e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
        {
            if (mouseX >= PosX && mouseX <= PosX + SPRITE_WIDTH
                && mouseY >= PosY && mouseY <= PosY + SPRITE_HEIGHT)
                mouseInside = true;
        }

        if (mouseInside && e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
            IsClicked = true;
    }

    //Renders the button
    public void Render()
    {
        RenderStatic(SpriteSheet.Texture, SPRITE_WIDTH, SPRITE_HEIGHT);
    }
}
    
