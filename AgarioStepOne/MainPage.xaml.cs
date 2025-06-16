
using System.Diagnostics;
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
///     This class was used to create the TowardAgarioStepOne functionality. 
///     
/// </summary>

namespace AgarioStepOne
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///    Called when the window is resized.  
        ///    
        ///     Note: this doesn't work because its an 'override' method.. why is it an override method? am i missing the base one?
        ///     
        /// </summary>
        /// <param name="width"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            Debug.WriteLine($"OnSizeAllocated {width} {height}");

            if (!initialized)
            {
                initialized = true;
                InitializeGameLogic();
            }
        }

        public void InitializeGameLogic()
        {
            // assign the worldDrawable to the PlaySurface.Drawable property

            // resize the widget - Window.Width = 500

            // start the timer 
            // timer should have a Tick event that calls 'GameStep'
            // this should 1- tell the worldModel to AdvanceGameStepOne
            // 2- tell the playsurface to redraw itself (invalidate)
            // 3- update the GUI labels to show the current location of the circle and its direction
        }


    }
}