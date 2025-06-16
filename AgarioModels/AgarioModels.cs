/// <summary>
/// Author:    Riley Kraabel and Lincoln Knowles
/// Date:      7-Apr-2023
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
///     This class contains the models for the game logic for the Agario game. 
///     There are methods to interact with the Server, and Protocol messages are the
///     standard for doing so. There are storage lists to hold items in the game and 
///     these are updated upon specific scenarios that may remove/add new items. 
///     
/// </summary>
using System.Text.Json.Serialization;

namespace AgarioModels
{
    /// <summary>
    ///     This class is responsible for storing the current state of the game, including the status and location
    ///     of every object in the game. 
    /// </summary>
    public class World
    {
        private readonly int height;
        private readonly int width;

        private Dictionary<long, Player> playerList;
        private Dictionary<long, Food> foodList;

        /// <summary>
        ///     Constructor method for a World item. 
        /// </summary>
        /// 
        /// <param name="name">     string type - the name of the player to connect to the world </param>
        /// <param name="IP">       string type - the IP address to connect the player to </param>
        public World()
        {
            height = 5000;
            width = 5000;

            playerList = new Dictionary<long, Player>();
            foodList = new Dictionary<long, Food>();
        }

        /// <summary>
        ///     Getter/setter method to return the player's ID (long). 
        /// </summary>
        public long playerID { get; set; }

        /// <summary>
        ///     Getter method to return the integer of the world's height display. 
        /// </summary>
        /// 
        /// <returns>   int type - holds the size of the world's height </returns>
        public int GetHeight() { return height; }

        /// <summary>
        ///     Getter method to return the integer of the world's width display. 
        /// </summary>
        /// 
        /// <returns>   int type - holds the size of the world's width </returns>
        public int GetWidth() { return width; }

        /// <summary>
        ///     Getter method to allow access to the World's players.
        /// </summary>
        /// 
        /// <returns>   Dictionary of Players - the list of Players in the world. </returns>
        public Dictionary<long, Player> GetPlayerList() { return playerList; }

        /// <summary>
        ///     Getter method to allow access to the World's food. 
        /// </summary>
        /// 
        /// <returns>   Dictionary of Food - the list of Food in the world. </returns>
        public Dictionary<long, Food> GetFoodList() { return foodList; }
    }

    /// <summary>
    ///     This class is a 'helper' class storing all information associated with a given game object (food, player, etc).
    /// </summary>
    public class GameObject
    {
        /// <summary>
        ///     JSON property for the ARGB field. Getter/setter method.
        /// </summary>
        [JsonPropertyName("ARGBColor")]
        public int ARGBColor { get; set; }

        /// <summary>
        ///     JSON property for the x coordinate field. Getter/setter method.
        /// </summary>
        [JsonPropertyName("X")]
        public float X { get; set; }

        /// <summary>
        ///     JSON property for the y coordinate field. Getter/setter method.
        /// </summary>
        [JsonPropertyName("Y")]
        public float Y { get; set; }

        /// <summary>
        ///     JSON property for the mass field. Getter/setter method.
        /// </summary>
        [JsonPropertyName("Mass")]
        public float Mass { get; set; }

        /// <summary>
        ///     JSON property for the id field. Getter/setter method.
        /// </summary>
        [JsonPropertyName("ID")]
        public long ID { get; set; }

        public int Radius { get; set; }

        public GameObject(float x, float y, int argbColor, long id, float mass)
        {
            X = x;
            Y = y;
            ARGBColor = argbColor;
            ID = id;
            Mass = mass;

            Radius = (int)Math.Sqrt(Mass / (Math.PI));
        }
    }

    /// <summary>
    ///     This class adds the 'name' field and subclasses the GameObject class. 
    /// </summary>
    public class Player : GameObject
    {
        /// <summary>
        ///     Constructor for initializing a 'Player' object for the game.
        /// </summary>
        /// 
        /// <param name="argbColor">    argb type - the color of the player item </param>
        /// <param name="x">            float type - the x coordinate of the player item </param>
        /// <param name="y">            float type - the y coordinate of the player item </param>
        /// <param name="mass">         float type - the mass of the player item </param>
        public Player(string name, float x, float y, int argbColor, long id, float mass)
            : base(x, y, argbColor, id, mass)
        {
            Name = name;
        }

        /// <summary>
        ///     Getter/Setter method for the player's 'name' field.
        /// </summary>
        public String Name { get; set; }
    }

    /// <summary>
    ///     This class subclasses the GameObject class (not much else).
    /// </summary>
    public class Food : GameObject
    {
        /// <summary>
        ///     Constructor for initializing a 'Food' object for the game.
        /// </summary>
        /// 
        /// <param name="argbColor">    arbg type - the color of the food item </param>
        /// <param name="x">            float type - the x coordinate of the food item </param>
        /// <param name="y">            float type - the y coordinate of the food item </param>
        /// <param name="mass">         float type - the mass of the food item </param>
        public Food(float x, float y, int argbColor, long id, float mass) : base(x, y, argbColor, id, mass) { }
    }

}