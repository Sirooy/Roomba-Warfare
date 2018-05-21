using System;
using SDL2;

public class Keyboard
{
    public event Action<string> OnTextChangedEvent;
    public event Action<string> OnSummitEvent;

    private string text { get; set; }

    public Keyboard()
    {
        text = "";
    }

    //Checks the number pressed and adds it to the text
    public void HandleEvents()
    {
        string lastTest = text;

        SDL.SDL_Keycode keyPressed = Hardware.KeyPressed();

        switch (keyPressed)
        {
            case SDL.SDL_Keycode.SDLK_0: text += "0"; break;
            case SDL.SDL_Keycode.SDLK_1: text += "1"; break;
            case SDL.SDL_Keycode.SDLK_2: text += "2"; break;
            case SDL.SDL_Keycode.SDLK_3: text += "3"; break;
            case SDL.SDL_Keycode.SDLK_4: text += "4"; break;
            case SDL.SDL_Keycode.SDLK_5: text += "5"; break;
            case SDL.SDL_Keycode.SDLK_6: text += "6"; break;
            case SDL.SDL_Keycode.SDLK_7: text += "7"; break;
            case SDL.SDL_Keycode.SDLK_8: text += "8"; break;
            case SDL.SDL_Keycode.SDLK_9: text += "9"; break;
            case SDL.SDL_Keycode.SDLK_PERIOD: text += "."; break; 
            case SDL.SDL_Keycode.SDLK_MINUS: text += "-"; break;
            case SDL.SDL_Keycode.SDLK_BACKSPACE:
                if (text.Length > 0)
                    text = text.Remove(text.Length - 1);
                break;
            case SDL.SDL_Keycode.SDLK_RETURN: OnSummitEvent(text); break;
        }
        
        //Remove the last character to make it shorter than 21
        if(text.Length > 21)
            text = text.Remove(text.Length - 1);

        if (text != lastTest)
            OnTextChangedEvent(text);
    }
}
