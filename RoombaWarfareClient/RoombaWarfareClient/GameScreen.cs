using System;
using SDL2;

//This screen controls the game loop

public class GameScreen : IScreen
{
    private const byte RED_TEAM_BUTTON = 0;
    private const byte SPECTATOR_TEAM_BUTTON = 1;
    private const byte BLUE_TEAM_BUTTON = 2;

    public ScreenType NextScreen { get; set; }

    private Action CurrentUpdateGameFunction;
    private Action CurrentRenderGameFunction;

    private Map map;
    private LocalPlayer localPlayer;
    private PlayerCollection players;
    private BulletCollection bullets;
    private Button[] buttons;
    private Camera camera;

    private float deltaTime;
    private bool isChangingTeam;

    public GameScreen()
    {
        NextScreen = ScreenType.None;
        map = new Map();
        localPlayer = new LocalPlayer(Game.PlayerSelectedType);
        players = new PlayerCollection();
        camera = new Camera((ushort)Hardware.ScreenWidth, 
            (ushort)Hardware.ScreenHeight);

        buttons = new Button[3];
        buttons[RED_TEAM_BUTTON] = new Button(ButtonType.RedTeamButton);
        buttons[SPECTATOR_TEAM_BUTTON] = new Button(ButtonType.SpectatorTeamButton);
        buttons[BLUE_TEAM_BUTTON] = new Button(ButtonType.BlueTeamButton);
        //Sets the positions of the buttons to change the team
        buttons[RED_TEAM_BUTTON].SetPos(100,
            Hardware.ScreenHeight / 2 - Button.SPRITE_HEIGHT / 2);
        buttons[SPECTATOR_TEAM_BUTTON].SetPos
            (Hardware.ScreenWidth / 2 - Button.SPRITE_WIDTH / 2,
            Hardware.ScreenHeight / 2 - Button.SPRITE_HEIGHT / 2);
        buttons[BLUE_TEAM_BUTTON].SetPos
            (Hardware.ScreenWidth - Button.SPRITE_WIDTH - 100,
            Hardware.ScreenHeight / 2 - Button.SPRITE_HEIGHT / 2);

        isChangingTeam = true;
        CurrentUpdateGameFunction = UpdateGameStateDead;
        CurrentRenderGameFunction = RenderGameDead;
    }

    //Translate all the data send by the server
    public void TranslateData(string[] allCommands)
    {
        foreach(string command in allCommands)
        {
            string[] commandParts = command.Split();

            switch ((ServerMessage)int.Parse(commandParts[0]))
            {
                //Sets the angle of the other players.
                case ServerMessage.SetPlayerAngle:
                    {
                        int id = int.Parse(commandParts[1]);
                        if (id != localPlayer.ID)
                            players.SetAngle(id, commandParts);
                    }
                    break;

                //Sets the position of the players
                case ServerMessage.SetPlayerPosition:
                    {
                        int id = int.Parse(commandParts[1]);
                        if (id != localPlayer.ID)
                            players.SetPosition(id, commandParts);
                        else
                        {
                            localPlayer.SetPos(commandParts);
                            localPlayer.Reconcile();
                        }
                    }
                    break;

                case ServerMessage.NewBullet:
                    //TO DO
                    break;

                case ServerMessage.RemoveBullet:
                    //TO DO
                    break;

                //Respawns a player
                case ServerMessage.Respawn:
                    {
                        int id = int.Parse(commandParts[1]);
                        if (id != localPlayer.ID)
                            players.Respawn(id, commandParts);
                        else
                        {
                            localPlayer.Respawn(commandParts);
                            CurrentUpdateGameFunction = UpdateGameStateAlive;
                            CurrentRenderGameFunction = RenderGameAlive;
                        }
                            
                    }
                    break;

                //Kills the player
                case ServerMessage.KillPlayer:
                    {
                        int id = int.Parse(commandParts[1]);
                        if (id != localPlayer.ID)
                            players.Kill(id);
                        else
                        {
                            localPlayer.Kill();
                            CurrentUpdateGameFunction = UpdateGameStateDead;
                            CurrentRenderGameFunction = RenderGameDead;
                        }
                    }
                    break;

                //Changes the team of a player
                case ServerMessage.SetPlayerTeam:
                    {
                        int id = int.Parse(commandParts[1]);
                        if (id != localPlayer.ID)
                            players.SetTeam(id, commandParts);
                        else
                        {
                            localPlayer.SetTeam(commandParts);
                            if ((PlayerTeam)(int.Parse(commandParts[2]))
                                == PlayerTeam.Spectator)
                                CurrentUpdateGameFunction = UpdateGameStateSpectator;
                        }
                    }
                    break;

                //Removes a player
                case ServerMessage.RemovePlayer:
                    {
                        int id = int.Parse(commandParts[1]);
                        players.Remove(id);
                    }
                    break;

                //Disconnects a player
                case ServerMessage.Disconnect:
                    NextScreen = ScreenType.End;
                    Game.EndMessage = commandParts[1];
                    break;

                //Sets the id of the local player.
                case ServerMessage.SetID:
                    localPlayer.ID = int.Parse(commandParts[1]);
                    break;
            }
        }
    }

    public void ReceiveData()
    {
        if (Game.GameSocket.DataAvailable)
        {
            string data = Game.GameSocket.Receive();
            TranslateData(data.Split(':'));
        }
    }

    public ScreenType Run()
    {
        string mapSeed = Game.GameSocket.Receive();
        map.Create(mapSeed);
        string initialData = Game.GameSocket.Receive();
        Game.GameSocket.Send(Convert.ToString((int)Game.PlayerSelectedType));

        GameLoop();

        return NextScreen;
    }

    public void GameLoop()
    {
        float maxFrameRate = (1f / 60f * 1000f);

        do
        {
            Hardware.ClearScreen();
            ReceiveData();
            CurrentUpdateGameFunction();
            CurrentRenderGameFunction();
            Hardware.UpdateScreen();
        } while (NextScreen == ScreenType.None);
    }

    public void SendData()
    {

    }

    //Updates the game when the player is in spectator team.
    public void UpdateGameStateSpectator()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            //TO DO
        }
    }

    //Updates the game when the player is alive.
    public void UpdateGameStateAlive()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            //TO DO
        }
    }

    //Updates the game when the player is dead.
    public void UpdateGameStateDead()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            //TO DO
        }
    }

    //Render all the things that a player can see either if it is alive or dead
    public void RenderBasics()
    {
        map.Render(camera);
        players.Render(camera);
    }

    //Renders all the necessary things for a dead player
    public void RenderGameDead()
    {
        RenderBasics();
        //TO DO
    }

    //Renders all the necessary things for an alive player
    public void RenderGameAlive()
    {
        RenderBasics();
        //TO DO
    }
}

