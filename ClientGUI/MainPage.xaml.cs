/// <summary>
/// Authors:   Riley Kraabel and Lincoln Knowles
/// Date:      22-Mar-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Riley Kraabel and Lincoln Knowles - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Riley Kraabel and Lincoln Knowles, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///     This class contains methods for the functionality of the ClientGUI. 
///     There are methods to create the GUI and a Drawable screen. The GUI updates according
///     to the Timer object, when eaten food/players are replaced and the player moves.
///     
///     The specific game statistics are displayed in the top bar of the Client GUI, and can
///     be accessed at any time throughout the game by the player. The more important and 
///     notable stats are listed on the physical GUI. 
///     
/// </summary>
using AgarioModels;
using Communications;
using Logger;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ClientGUI
{
    /// <summary>
    ///     This class contains methods that allow for functionality of the ClientGUI.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private bool initialized = false;
        private WorldDrawable world;
        private World worldObject;

        private Networking networky;
        private FileLogger fileLoggy;

        private Point? mousePosition;

        /// <summary>
        ///     Constructor for initializing a new MainPage of the ClientGUI.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            fileLoggy = new FileLogger("loggyy");
            networky = new Networking(fileLoggy, onConnect, onDisconnect, onMessage, '\n');

            worldObject = new World();
        }

        /// <summary>
        ///     Method for handling the functionality of a connection. 
        /// </summary>
        /// 
        /// <param name="channel">  Networking object - the item connecting </param>
        private void onConnect(Networking channel)
        {
            try
            {
                networky.AwaitMessagesAsync(true);
                networky.Send(String.Format(Protocols.CMD_Start_Game, NameEntry.Text));

                fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " connection success.", null);
            }

            catch (Exception exception)
            {
                fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " connection failure." + exception);
            }
        }

        /// <summary>
        ///     Method for handling the functionality of a disconnection.
        /// </summary>
        /// 
        /// <param name="channel">  Networking object - the item disconnecting </param>
        private async void onDisconnect(Networking channel)
        {
            try
            {
                initialized = false;
                PlaySurface.IsVisible = false;

                GameOver.IsVisible = true;
                PlayAgain.IsVisible = true;
                EndGame.IsVisible = true;

                lock (worldObject.GetPlayerList())
                {
                    worldObject.GetPlayerList().Remove(worldObject.GetPlayerList()[worldObject.playerID].ID);
                }

                networky.AwaitMessagesAsync(false);

                fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " disconnection success.", null);
            }

            catch (Exception exception)
            {
                fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " disconnection failed.", exception);
            }
        }

        /// <summary>
        ///     Method for handling the functionality of messages being sent/received.
        /// </summary>
        /// 
        /// <param name="channel">  Networking type - the item sending/receiving the messages </param>
        /// <param name="message">  string type - the message being sent/received </param>
        private void onMessage(Networking channel, string message)
        {
            if (message.StartsWith(Protocols.CMD_Player_Object))
            {
                long.TryParse(message[Protocols.CMD_Player_Object.Length..], out long playerID);
                worldObject.playerID = playerID;

                fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " command player object.", null);
            }

            else if (message.StartsWith(Protocols.CMD_Eaten_Food))
            {
                try
                {
                    List<int> foodToRemove = JsonSerializer.Deserialize<List<int>>(message[Protocols.CMD_Eaten_Food.Length..]);

                    lock (worldObject.GetFoodList())
                    {
                        foreach (int foodItem in foodToRemove)
                        {
                            if (worldObject.GetFoodList().ContainsKey(foodItem))
                                worldObject.GetFoodList().Remove(foodItem);
                        }
                    }

                    fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " command eaten food." + null);
                }

                catch (Exception exception)
                {
                    fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " command eaten food failure." + exception);
                }
            }

            else if (message.StartsWith(Protocols.CMD_Dead_Players))
            {
                try
                {
                    List<int> playerIDsToRemove = JsonSerializer.Deserialize<List<int>>(message[Protocols.CMD_Dead_Players.Length..]);

                    lock (worldObject.GetPlayerList())
                    {
                        foreach (long playerID in playerIDsToRemove)
                        {
                            if (worldObject.GetPlayerList().ContainsKey(playerID))
                            {
                                worldObject.GetPlayerList().Remove(playerID);
                                onDisconnect(GetNetworking());
                            }
                        }
                    }
                }

                catch (Exception exception) { }
            }

            else if (message.StartsWith(Protocols.CMD_Food))
            {
                try
                {
                    List<Food> tempFood = JsonSerializer.Deserialize<List<Food>>(message[Protocols.CMD_Food.Length..]);

                    lock (worldObject.GetFoodList())
                    {
                        foreach (Food foodItem in tempFood)
                        {
                            worldObject.GetFoodList().Add(foodItem.ID, foodItem);
                        }
                    }

                    fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " command food." + null);
                }

                catch (Exception exception)
                {
                    fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " command food failure." + exception);
                }
            }

            else if (message.StartsWith(Protocols.CMD_Update_Players))
            {
                try
                {
                    List<Player> tempPlayers = JsonSerializer.Deserialize<List<Player>>(message[Protocols.CMD_Update_Players.Length..]);

                    lock (worldObject.GetPlayerList())
                    {
                        foreach (Player player in tempPlayers)
                        {
                            if (worldObject.GetPlayerList().ContainsKey(player.ID))
                                worldObject.GetPlayerList()[player.ID] = player;

                            else
                                worldObject.GetPlayerList().Add(player.ID, player);
                        }
                    }

                    fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " command update players." + null);
                }

                catch (Exception exception)
                {
                    fileLoggy.Log(LogLevel.Information, "client " + channel.ID + " command update players failed." + exception);
                }
            }

            else if (message.StartsWith(Protocols.CMD_HeartBeat))
            {
                if (mousePosition != null)
                    this.Move((int)mousePosition.Value.X, (int)mousePosition.Value.Y);

                if (worldObject.GetPlayerList().ContainsKey(worldObject.playerID) && worldObject.GetPlayerList()[worldObject.playerID].ID != 0)
                    mass.Text = "Current Mass: " + worldObject.GetPlayerList()[worldObject.playerID].Mass;

                // Update the TextBoxes in the GUI
                playerCount.Text = "Current Player Count: " + worldObject.GetPlayerList().Count;
                foodCount.Text = "Current Food Count: " + worldObject.GetFoodList().Count;

                Leaderboard.Text = UpdateLeaderboard();

                Dispatcher.Dispatch(() => PlaySurface.Invalidate());
            }
        }

        /// <summary>
        ///     This method handles the functionality for the 'leaderboard' object. 
        /// </summary>
        /// 
        /// <returns>   string type - the text holding the leaderboard items. </returns>
        public string UpdateLeaderboard()
        {
            List<string> highestMasses = new List<string>();
            int maxMass = 0;

            foreach (Player playerItem in worldObject.GetPlayerList().Values)
            {
                if (playerItem.Mass > maxMass)
                    maxMass = (int)playerItem.Mass;
                highestMasses.Add(playerItem.Name);
            }

            return "Leaderboard: \n1. " + highestMasses[0] + "\n2. " + highestMasses[1] + "\n3. " + highestMasses[2] + "\n4. " + highestMasses[3] +
                "\n5. " + highestMasses[4];
        }

        /// <summary>
        ///     Method for functionality when a player object wants to move. Sends a protocol command to the server.
        /// </summary>
        /// 
        /// <param name="px">   float type - the x coordinate of where to move to </param>
        /// <param name="py">   float type - the y coordinate of where to move to </param>
        public void Move(int moveX, int moveY)
        {
            try
            {
                int topLeftX = (int)(worldObject.GetPlayerList()[worldObject.playerID].X - 500);
                int topLeftY = (int)(worldObject.GetPlayerList()[worldObject.playerID].Y - 250);

                networky.Send(String.Format(Protocols.CMD_Move, moveX + topLeftX, moveY + topLeftY));

                fileLoggy.Log(LogLevel.Information, "client " + networky.ID + " command move." + null);
            }

            catch (Exception exception)
            {
                fileLoggy.Log(LogLevel.Information, "client " + networky.ID + " command move failed." + exception);
            }
        }

        /// <summary>
        ///     Getter method to return the current Networking item. Used for Networking communications.
        /// </summary>
        /// 
        /// <returns>   Networking type - the current item's Networking object. </returns>
        public Networking GetNetworking() { return networky; }

        /// <summary>
        ///     Helper method for determining if an entry's text is valid or not. Used inside of the connectionPage. 
        /// </summary>
        /// 
        /// <param name="entry">    Entry type - the Entry field being changed </param>
        /// <returns>               bool type - true if the Entry's text field is not null/"". false if it is. </returns>
        private bool ValidText(Entry entry)
        {
            if (entry.Text == null || entry.Text == "")
                return false;
            else
                return true;
        }

        /// <summary>
        ///     This method is used to start the gameLogic for the world. It creates new objects
        ///     (like a World and WorldDrawable). 
        /// </summary>
        private void InitializeGameLogic()
        {
            try
            {
                networky.Connect(NameOrAddressEntry.Text, 11000);

                PlaySurface.Drawable = new WorldDrawable(worldObject);

                Split.Focus();
                fileLoggy.Log(LogLevel.Information, "Successfully connected the client to the server." + null);
            }

            catch (Exception exception)
            {
                fileLoggy.Log(LogLevel.Information, "There was an error connecting to the client." + exception);
            }

            initialized = true;
        }

        /// <summary>
        ///     This method constantly receives the coordinate point location of the mouse inside of the game screen. 
        /// </summary>
        /// 
        /// <param name="Sender">   the item that's pointer is changing locations. </param>
        /// <param name="evt"></param>
        private void PointerChanged(object Sender, PointerEventArgs evt)
        {
            mousePosition = evt.GetPosition(PlaySurface);

            try
            {
                if (worldObject.GetPlayerList()[worldObject.playerID].ID != 0 && worldObject.GetPlayerList().ContainsKey(worldObject.playerID) == false)
                    this.onDisconnect(GetNetworking());

                else if (worldObject.GetPlayerList().Count > 0 && worldObject.GetPlayerList()[worldObject.playerID].ID != 0 && worldObject.GetPlayerList().ContainsKey(worldObject.playerID))
                {
                    Dispatcher.DispatchAsync(() => coordinates.Text = "Current Coordinates: " +
                       worldObject.GetPlayerList()[worldObject.playerID].X + ", "
                     + worldObject.GetPlayerList()[worldObject.playerID].Y);
                }

                else
                    this.onDisconnect(GetNetworking());
            }

            catch (Exception exception)
            {
                ErrorField.Text += "Oops, there was an error: " + exception;
            }
        }

        /// <summary>
        ///     Handles the functionality of when the 'connect to server' button is pressed. 
        ///     Changes the 'screen' to the gameScreen. 
        /// </summary>
        /// 
        /// <param name="sender"> the item who pressed the button </param>
        /// <param name="e"></param>
        public void OnConnectClicked(object sender, EventArgs e)
        {
            if (ValidText(NameEntry) && ValidText(NameOrAddressEntry) && !initialized)
            {
                initialized = true;

                ConnectionScreen.IsVisible = false;
                GameScreen.IsVisible = true;
                PlaySurface.IsVisible = true;

                InitializeGameLogic();
            }

            else
                ErrorField.Text = "Error: Make sure you entered a server/address name and a player name! Try Again.";
        }

        /// <summary>
        ///     This method handles the functionality for when the 'press space to split' button is pressed or the space bar is pressed. 
        /// </summary>
        /// 
        /// <param name="sender">   the item that sent the request to split. </param>
        /// <param name="e"></param>
        public void OnSplit(object sender, EventArgs e)
        {
            if (Split.Text == " " && worldObject.GetPlayerList().ContainsKey(worldObject.playerID) && worldObject.GetPlayerList()[worldObject.playerID].ID != 0)
            {
                GetNetworking().Send(String.Format(Protocols.CMD_Split,
                    (int)(mousePosition.Value.X + worldObject.GetPlayerList()[worldObject.playerID].X - 500),
                    (int)(mousePosition.Value.Y + worldObject.GetPlayerList()[worldObject.playerID].Y - 250)));
            }

            else
                gameErrors.Text = "Error: You must have at least 1000 mass to split! Eat more food.";

            Split.Text = "";
        }

        /// <summary>
        ///     This method handles the functionality for when the 'play again' button is clicked.
        /// </summary>
        /// 
        /// <param name="sender"> the item that clicked the play again button. </param>
        /// <param name="e"></param>
        public void OnPlayClicked(object sender, EventArgs e)
        {
            GameOver.IsVisible = false;
            PlayAgain.IsVisible = false;
            EndGame.IsVisible = false;

            PlaySurface.IsVisible = true;

            initialized = true;
            InitializeGameLogic();
        }

        /// <summary>
        ///     This method handles the functionality for when the 'end game' button is clicked.
        /// </summary>
        /// 
        /// <param name="sender"> the item that clicked the 'end game' button. </param>
        /// <param name="e"></param>
        public void OnEndClicked(object sender, EventArgs e)
        {
            GameOver.IsVisible = false;
            PlayAgain.IsVisible = false;
            EndGame.IsVisible = false;

            GameScreen.IsVisible = false;
            ConnectionScreen.IsVisible = true;

            NameEntry.Text = "";
            NameOrAddressEntry.Text = "";
        }
    }

}