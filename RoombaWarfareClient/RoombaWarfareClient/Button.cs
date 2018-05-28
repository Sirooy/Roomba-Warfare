using SDL2;

public enum ButtonType : byte
     {  StartButton, ExitButton , SetAddressButton
     , BlueTeamButton, RedTeamButton , SpectatorTeamButton
     , SelectAssaultPlayer, SelectCommanderPlayer
     , SelectRusherPlayer, SelectTankPlayer }

public enum ButtonState : byte
{
    Normal,
    MouseOver,
    Clicked
}

public class Button : StaticEntity
{
    public static readonly byte SPRITE_WIDTH = 200;
    public static readonly byte SPRITE_HEIGHT = 100;

    public bool IsClicked { get; set; }

    private ButtonState currentState;
    private ushort[] spritesX;
    private ushort[] spritesY;

    public Button(ButtonType type)
    {
        IsClicked = false;
        spritesX = new ushort[3];
        spritesY = new ushort[3];
        currentState = 0;

        //Assings the texture of the button depending on the type
        switch (type)
        {
            //TO DO (Set the sprite positions)
            case ButtonType.StartButton:
                spritesX[(byte)ButtonState.Normal] = 600;
                spritesY[(byte)ButtonState.Normal] = 128;
                spritesX[(byte)ButtonState.MouseOver] = 600;
                spritesY[(byte)ButtonState.MouseOver] = 228;
                spritesX[(byte)ButtonState.Clicked] = 600;
                spritesY[(byte)ButtonState.Clicked] = 328;
                break;

            case ButtonType.SetAddressButton:
                spritesX[(byte)ButtonState.Normal] = 800;
                spritesY[(byte)ButtonState.Normal] = 128;
                spritesX[(byte)ButtonState.MouseOver] = 800;
                spritesY[(byte)ButtonState.MouseOver] = 228;
                spritesX[(byte)ButtonState.Clicked] = 600;
                spritesY[(byte)ButtonState.Clicked] = 328;
                break;

            case ButtonType.ExitButton:
                spritesX[(byte)ButtonState.Normal] = 800;
                spritesY[(byte)ButtonState.Normal] = 428;
                spritesX[(byte)ButtonState.MouseOver] = 800;
                spritesY[(byte)ButtonState.MouseOver] = 528;
                spritesX[(byte)ButtonState.Clicked] = 600;
                spritesY[(byte)ButtonState.Clicked] = 328;
                break;

            case ButtonType.SelectAssaultPlayer:
                spritesX[(byte)ButtonState.Normal] = 0;
                spritesY[(byte)ButtonState.Normal] = 428;
                spritesX[(byte)ButtonState.MouseOver] = 0;
                spritesY[(byte)ButtonState.MouseOver] = 528;
                spritesX[(byte)ButtonState.Clicked] = 0;
                spritesY[(byte)ButtonState.Clicked] = 328;
                break;

            case ButtonType.SelectCommanderPlayer:
                spritesX[(byte)ButtonState.Normal] = 0;
                spritesY[(byte)ButtonState.Normal] = 128;
                spritesX[(byte)ButtonState.MouseOver] = 0;
                spritesY[(byte)ButtonState.MouseOver] = 228;
                spritesX[(byte)ButtonState.Clicked] = 0;
                spritesY[(byte)ButtonState.Clicked] = 328;
                break;

            case ButtonType.SelectRusherPlayer:
                spritesX[(byte)ButtonState.Normal] = 0;
                spritesY[(byte)ButtonState.Normal] = 128;
                spritesX[(byte)ButtonState.MouseOver] = 0;
                spritesY[(byte)ButtonState.MouseOver] = 228;
                spritesX[(byte)ButtonState.Clicked] = 0;
                spritesY[(byte)ButtonState.Clicked] = 328;
                break;

            case ButtonType.SelectTankPlayer:
                spritesX[(byte)ButtonState.Normal] = 0;
                spritesY[(byte)ButtonState.Normal] = 128;
                spritesX[(byte)ButtonState.MouseOver] = 0;
                spritesY[(byte)ButtonState.MouseOver] = 228;
                spritesX[(byte)ButtonState.Clicked] = 0;
                spritesY[(byte)ButtonState.Clicked] = 328;
                break;

            case ButtonType.SpectatorTeamButton:
                spritesX[(byte)ButtonState.Normal] = 0;
                spritesY[(byte)ButtonState.Normal] = 128;
                spritesX[(byte)ButtonState.MouseOver] = 0;
                spritesY[(byte)ButtonState.MouseOver] = 228;
                spritesX[(byte)ButtonState.Clicked] = 0;
                spritesY[(byte)ButtonState.Clicked] = 328;
                break;

            case ButtonType.BlueTeamButton:
                spritesX[(byte)ButtonState.Normal] = 0;
                spritesY[(byte)ButtonState.Normal] = 128;
                spritesX[(byte)ButtonState.MouseOver] = 0;
                spritesY[(byte)ButtonState.MouseOver] = 228;
                spritesX[(byte)ButtonState.Clicked] = 0;
                spritesY[(byte)ButtonState.Clicked] = 328;
                break;

            case ButtonType.RedTeamButton:
                spritesX[(byte)ButtonState.Normal] = 0;
                spritesY[(byte)ButtonState.Normal] = 128;
                spritesX[(byte)ButtonState.MouseOver] = 0;
                spritesY[(byte)ButtonState.MouseOver] = 228;
                spritesX[(byte)ButtonState.Clicked] = 0;
                spritesY[(byte)ButtonState.Clicked] = 328;
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
            e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN ||
            e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
        {
            if (mouseX >= PosX && mouseX <= PosX + SPRITE_WIDTH
                && mouseY >= PosY && mouseY <= PosY + SPRITE_HEIGHT)
                mouseInside = true;
        }

        if (!mouseInside)
        {
            currentState = ButtonState.Normal;
        }
        else
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_MOUSEMOTION:
                    if(currentState != ButtonState.Clicked)
                        currentState = ButtonState.MouseOver;
                    break;

                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    currentState = ButtonState.Clicked;
                    break;

                case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                    currentState = ButtonState.Normal;
                    IsClicked = true;
                    break;
            }
        }
    }

    //Renders the button
    public void Render()
    {
        spriteX = spritesX[(byte)currentState];
        spriteY = spritesY[(byte)currentState];
        RenderStatic(SpriteSheet.Texture, SPRITE_WIDTH, SPRITE_HEIGHT);
    }
}
    
