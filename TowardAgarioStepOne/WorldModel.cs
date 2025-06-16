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
///     This class was used to create the TowardAgarioStepOne functionality. 
///     
/// </summary>

namespace TowardAgarioStepOne
{
    /// <summary>
    ///     This class contains the 'global state' of the program - in this case, only the x,y, and radius of the circle along with the
    ///     direction (vector2) that the circle in moving in and the width/height of the window.
    /// </summary>
    internal class WorldModel
    {
        private readonly System.Numerics.Vector2 xY = new System.Numerics.Vector2(100, 100);
        private readonly System.Numerics.Vector2 direction = new System.Numerics.Vector2(50, 25);

        /// <summary>
        ///     This constructor holds the object data.
        /// </summary>
        public WorldModel(System.Numerics.Vector2 location, System.Numerics.Vector2 move)
        {
            xY = location;
            direction = move;
        }

        /// <summary>
        ///     This method adds the direction vector to the circle. If the circle moves off the rectangle to the left or right, 
        ///     change the direction by multiplying the X direction by -1. If the circle moves off the rectangle to the top or bottom, 
        ///     multiply the Y direction by -1. 
        /// </summary>
        public void AdvanceGameOneStep()
        {

        }
    }

}