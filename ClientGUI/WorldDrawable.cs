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
///     This class contains methods that draw the world inside of the ClientGUI. 
///    
/// </summary>
using AgarioModels;

namespace ClientGUI
{
    /// <summary>
    ///     This class contains all drawings for the 'World' view.
    /// </summary>
	internal class WorldDrawable : IDrawable
    {
        private World modelWorld;
        private Player currentPlayer;

        /// <summary>
        ///     Constructor to initialize the Drawable World view.
        /// </summary>
        /// 
        /// <param name="world"> World type - the World stats that we are going to draw. </param>
        public WorldDrawable(World world)
        {
            modelWorld = world;
        }

        /// <summary>
        ///     This method handles drawing all of the items onto the graphics view. 
        /// </summary>
        /// 
        /// <param name="canvas">       ICanvas type - the canvas being drawn on. </param>
        /// <param name="dirtyRect">    RectF type - TODO... fill this in. </param>
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (modelWorld.GetPlayerList().ContainsKey(modelWorld.playerID) && modelWorld.GetPlayerList()[modelWorld.playerID].ID != 0)
            {
                currentPlayer = modelWorld.GetPlayerList()[modelWorld.playerID];

                lock (modelWorld.GetPlayerList())
                {
                    int RightBound = (int)currentPlayer.X + 1000 / 2;
                    int LeftBound = (int)currentPlayer.X - 1000 / 2;
                    int UpperBound = (int)currentPlayer.Y - 500 / 2;
                    int LowerBound = (int)currentPlayer.Y + 500 / 2;

                    float topLeftX = (currentPlayer.X - 1000 / 2);
                    float topLeftY = (currentPlayer.Y - 500 / 2);

                    canvas.StrokeColor = Colors.Black;
                    canvas.StrokeSize = 1;
                    canvas.DrawRectangle(0, 0, 1000, 500);

                    try
                    {
                        foreach (Food foodObj in modelWorld.GetFoodList().Values)
                        {
                            canvas.FillColor = Color.FromInt(foodObj.ARGBColor);
                            canvas.FillCircle(foodObj.X - topLeftX, foodObj.Y - topLeftY, foodObj.Radius);
                        }

                        foreach (Player playerObj in modelWorld.GetPlayerList().Values)
                        {
                            canvas.FillColor = Color.FromInt(playerObj.ARGBColor);
                            canvas.FillCircle(playerObj.X - topLeftX, playerObj.Y - topLeftY, playerObj.Radius);

                            canvas.StrokeColor = Colors.Black;
                            canvas.StrokeSize = 1;
                            canvas.DrawCircle(playerObj.X - topLeftX, playerObj.Y - topLeftY, playerObj.Radius);

                            canvas.FontColor = Colors.Black;
                            canvas.DrawString(playerObj.Name, playerObj.X - topLeftX, playerObj.Y - topLeftY, HorizontalAlignment.Center);
                        }
                    }

                    catch (Exception ex) { }
                }

            }
        }
    }

}