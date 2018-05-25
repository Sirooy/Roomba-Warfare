using System;
using System.Threading;
using SDL2;

//This screen controls the game loop

public class GameScreen : IScreen
{
    private static Image deathBackground = new Image
        (@"resources\images\backgrounds\death_bck_test.png", 640, 480);

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
    private Button[] teamButtons;
    private Camera camera;

    private float deltaTime;
    private bool isChangingTeam;
    private string messageBuffer;

    //REMOVE LATER
    private Bullet redBullet;
    private Bullet blueBullet;

    public GameScreen()
    {
        Game.GameSocket.OnDisconnectionEvent += Disconnect;

        NextScreen = ScreenType.None;
        map = new Map();
        localPlayer = new LocalPlayer(Game.PlayerSelectedType);
        bullets = new BulletCollection();
        players = new PlayerCollection();
        camera = new Camera((ushort)Hardware.ScreenWidth, 
            (ushort)Hardware.ScreenHeight);

        teamButtons = new Button[3];
        teamButtons[RED_TEAM_BUTTON] = new Button(ButtonType.RedTeamButton);
        teamButtons[SPECTATOR_TEAM_BUTTON] = new Button(ButtonType.SpectatorTeamButton);
        teamButtons[BLUE_TEAM_BUTTON] = new Button(ButtonType.BlueTeamButton);
        //Sets the positions of the buttons to change the team
        teamButtons[RED_TEAM_BUTTON].SetPos(100,
            Hardware.ScreenHeight / 2 - Button.SPRITE_HEIGHT / 2);
        teamButtons[SPECTATOR_TEAM_BUTTON].SetPos
            (Hardware.ScreenWidth / 2 - Button.SPRITE_WIDTH / 2,
            Hardware.ScreenHeight / 2 - Button.SPRITE_HEIGHT / 2);
        teamButtons[BLUE_TEAM_BUTTON].SetPos
            (Hardware.ScreenWidth - Button.SPRITE_WIDTH - 100,
            Hardware.ScreenHeight / 2 - Button.SPRITE_HEIGHT / 2);

        isChangingTeam = true;
        messageBuffer = "";
        CurrentUpdateGameFunction = UpdateGameStateDead;
        CurrentRenderGameFunction = RenderGameDead;
        deltaTime = 1;

        //REMOVE LATER
        redBullet = new Bullet(PlayerTeam.Red);
        redBullet.SetPos(0, 0);
        blueBullet = new Bullet(PlayerTeam.Blue);
        blueBullet.SetPos(0, 32);
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
                        }
                    }
                    break;
                
                //Creates a new player
                case ServerMessage.NewPlayer:
                    {
                        int id = int.Parse(commandParts[1]);
                        if (id != localPlayer.ID)
                            players.Add(id, commandParts);
                        break;
                    }

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
        //Get all the data sent by the server and send the player type
        string initialData = Game.GameSocket.Receive();
        TranslateData(initialData.Split(':'));

        if (NextScreen != ScreenType.End)
        {
            string mapSeed = Game.GameSocket.Receive();
            map.Create(mapSeed);

            Game.GameSocket.Send(Convert.ToString((int)Game.PlayerSelectedType));
            GameLoop();
        }

        return NextScreen;
    }

    public void GameLoop()
    {
        float maxFrameRate = (1f / 60f) * 1000f;

        do
        {
            uint time = SDL.SDL_GetTicks();

            ReceiveData();
            Hardware.ClearScreen();
            CurrentUpdateGameFunction();
            CurrentRenderGameFunction();
            Hardware.UpdateScreen();
            SendData();

            //Cap the frame rate to 60 fps
            uint frameTime = (SDL.SDL_GetTicks() - time);
            if(frameTime < maxFrameRate)
            {
                Thread.Sleep((int)(maxFrameRate - frameTime));
            }
            //Get the total time of the frame
            deltaTime = (SDL.SDL_GetTicks() - time) / 10;
        } while (NextScreen == ScreenType.None);
    }

    public void SendData()
    {
        messageBuffer += localPlayer.GetMessage();

        if(messageBuffer != "")
        {
            //Remove the last :
            messageBuffer = messageBuffer.Remove(messageBuffer.Length - 1);
            Console.WriteLine("Message sent: " + messageBuffer); //Remove later
            Game.GameSocket.Send(messageBuffer);
            messageBuffer = "";
        }
    }

    //Updates the game when the player is alive.
    public void UpdateGameStateAlive()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            localPlayer.HandleEvents(e);
        }
        //Update the players and bullet positions
        localPlayer.SetAngle(camera);
        localPlayer.Update(deltaTime);
        localPlayer.CheckCollisions(map.Hitboxes);
        camera.SetPos(localPlayer, Player.SPRITE_WIDTH, Player.SPRITE_HEIGHT);
        players.Update(deltaTime);
        bullets.Update(deltaTime);
    }

    //Updates the game when the player is dead or in spectate mode.
    public void UpdateGameStateDead()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            if (Hardware.IsKeyPressed(SDL.SDL_Keycode.SDLK_m, e))
                isChangingTeam = !isChangingTeam;

            if (isChangingTeam)
            {
                foreach(Button button in teamButtons)
                {
                    button.HandleEvents(e);
                }
            }
        }

        //When the player press a team button sends to the server a change
        //team request or nothing if it tries to change to its current team
        if (isChangingTeam)
        {
            if (teamButtons[SPECTATOR_TEAM_BUTTON].IsClicked)
            {
                if(localPlayer.Team != PlayerTeam.Spectator)
                {
                    messageBuffer += (int)ClientMessage.ChangeTeam + " " +
                        localPlayer.ID + " " + (int)PlayerTeam.Spectator + ":";
                }
                isChangingTeam = false;
            }
            else if (teamButtons[RED_TEAM_BUTTON].IsClicked)
            {
                if (localPlayer.Team != PlayerTeam.Red)
                {
                    messageBuffer += (int)ClientMessage.ChangeTeam + " " +
                        localPlayer.ID + " " + (int)PlayerTeam.Red + ":";
                }
                isChangingTeam = false;
            }
            else if (teamButtons[BLUE_TEAM_BUTTON].IsClicked)
            {
                if (localPlayer.Team != PlayerTeam.Blue)
                {
                    messageBuffer += (int)ClientMessage.ChangeTeam + " " +
                        localPlayer.ID + " " + (int)PlayerTeam.Blue + ":";
                }
                isChangingTeam = false;
            }
        }

        players.Update(deltaTime);
        bullets.Update(deltaTime);
    }

    //Render all the things that a player can see either if it is alive or dead
    public void RenderBasics()
    {
        map.Render(camera);
        players.Render(camera);
        bullets.Render(camera);

        //REMOVE LATER
        blueBullet.Render(camera);
        redBullet.Render(camera);
    }

    //Renders all the necessary things for a dead player
    public void RenderGameDead()
    {
        RenderBasics();

        if (isChangingTeam)
        {
            foreach(Button button in teamButtons)
            {
                button.Render();
            }
        }

        Hardware.RenderBackground(deathBackground);
    }

    //Renders all the necessary things for an alive player
    public void RenderGameAlive()
    {
        RenderBasics();
        localPlayer.Render(camera);
    }

    //Disconnects the player from the server when an error occurs
    public void Disconnect()
    {
        Game.EndMessage = "Connection lost.";
        Game.GameSocket.Disconnect();
        NextScreen = ScreenType.End;
    }
}

