```
Author:     Riley Kraabel and Lincoln Knowles
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  capricornious2
Repo:       https://github.com/uofu-cs3500-spring23/assignment8agario-capricornious4
Date:       7-Apr-2023
Project:    Agario
Copyright:  CS 3500 and Riley Kraabel and Lincoln Knowles - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:
    1. We tried to implement the disconnect, but the buttons did not cooperate accordingly and therefore do not respond well to the client.

    2. While initializing the server, we used Prof. Jim's Communications file but were unable to get the IPaddress field to take in a server
    name other than localhost or an IPAddress. Just a note.

    3. Zoom was not implemented because when we did, it would not update the mass of the client. 
    
# Assignment Specific Topics
    1. World Redrawing vs. Networking Data (which is more of a bottleneck?)
    The world redraw definitely seems like more of a bottle neck compared to the networking data. While the data can be 
    sent directly using the 'Send' method, the World Redrawing requires you to redraw the entire screen (which can be a lot of pixels) 
    every so often, which takes up a lot of processing time. While debugging, we had the most issues with redrawing the screen because
    of how many steps it requires; you have to get the player's color, size, and data in order to draw it, and most of these methods take
    a little while to get all of the steps set up. 

# Consulted Peers:
    1. Class Piazza server
    2. Class Discord server

# References:
    1. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/graphicsview?view=net-maui-7.0
    2. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/graphics/draw?view=net-maui-7.0 
    3. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/graphics/draw?view=net-maui-7.0
    4. https://learn.microsoft.com/en-us/dotnet/api/system.timers.timer?view=net-8.0