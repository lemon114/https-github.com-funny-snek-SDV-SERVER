
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Network;

// TODOs and NOtes 
//gametime is given in raw military time eg: 600-1800
// feed messages to in game warnings
// lock out player controls?
// Game1.freezeControls = true;  to freeze input?
// make host invisible?
// set up config file to handle length of festivals
//deal with outliers like Grampa's Ghost or other random events that might break the server




namespace test
{


    public class ModEntry : Mod
    {

       






        private int gameTicks; //stores 1s game ticks for pause code
        private int gameClockTicks; //stores in game clock change
        private int numPlayers = 0; //stores number of players
        private bool IsEnabled = false;  //stores if the the server mod is enabled 
        private readonly Dictionary<string, int> PreviousFriendships = new Dictionary<string, int>();
        private bool eggHuntAvailable = false; //is egg festival ready start timer for triggering eggHunt Event
        private int eggHuntCountDown; //to trigger egghunt after set time
        //private bool justSaved = true; //store if we just saved for sleep timing, not used saved for future jic







        public override void Entry(IModHelper helper)
        {

            helper.ConsoleCommands.Add("server", "Toggles headless server on/off", this.ServerToggle);

            SaveEvents.BeforeSave += this.Shipping_Menu; // Shipping Menu handler
            // SaveEvents.AfterSave += this.AfterSave; // not used for now
            GameEvents.OneSecondTick += this.GameEvents_OneSecondTick; //game tick event handler
            TimeEvents.TimeOfDayChanged += this.TimeEvents_TimeOfDayChanged; // Time of day change handler



        }

        private void ServerToggle(string command, string[] args)          // toggles server on/off
        {
            if (IsEnabled == false)
            {
                IsEnabled = true;
                this.Monitor.Log("The server is running!", LogLevel.Info);

            }
            else if (IsEnabled == true)
            {
                IsEnabled = false;
                this.Monitor.Log("The server is turned off!", LogLevel.Info);
            }
        }




        
        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            

            if (IsEnabled == false) // server toggle
            {
                Game1.paused = false;
                return;
            }

            //NoClientsPause();  /// REMEMBER TO TURN BACK ON AFTER TESTING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    
            if (IsEnabled == true) // server toggle
            {
                if (this.PreviousFriendships.Any())  //disable friendship decay
                {
                    foreach (string key in Game1.player.friendshipData.Keys)
                    {
                        Friendship friendship = Game1.player.friendshipData[key];
                        if (this.PreviousFriendships.TryGetValue(key, out int oldPoints) && oldPoints > friendship.Points)
                            friendship.Points = oldPoints;
                    }
                }

                this.PreviousFriendships.Clear();
                foreach (var pair in Game1.player.friendshipData.FieldDict)
                    this.PreviousFriendships[pair.Key] = pair.Value.Value.Points;

            }
            
            if (eggHuntAvailable == true)
            {
                eggHuntCountDown += 1;
                
                if (eggHuntCountDown >= 240)  //set to 240 when done testing!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                { 
                    this.Helper.Reflection.GetMethod(Game1.CurrentEvent, "answerDialogueQuestion", true).Invoke(Game1.getCharacterFromName("Lewis"), "yes"); //start eggHunt
                    eggHuntAvailable = false;
                }
              
            }


        }




        //Pause game if no clients Code
        private void NoClientsPause()
        {
            gameTicks += 1;

            if (gameTicks >= 3)
            {
                this.numPlayers = Game1.otherFarmers.Count;

                if (numPlayers >= 1)
                {
                    Game1.paused = false;
                }
                else if (numPlayers <= 0)
                {
                    Game1.paused = true;
                    this.Monitor.Log($"The number of connected players is: {numPlayers} and the game is Paused", LogLevel.Debug);
                }

                gameTicks = 0;
            }
        }

        /*private void AfterSave(object sender, EventArgs e)   //not used, saved for future reference in case I need it
        {
            justSaved = true;
        }*/




        // auto-sleep and Holiday code
        private void TimeEvents_TimeOfDayChanged(object sender, EventArgs e)
        {
            if (IsEnabled == false) // server toggle
                return;

                
            gameClockTicks += 1;


            if (gameClockTicks >= 2)
            {
                var currentTime = Game1.timeOfDay;
                var currentDate = SDate.Now();
                var eggFestival = new SDate(13, "spring");
                var flowerDance = new SDate(24, "spring");
                    var luau = new SDate(11, "summer");
                    var danceOfJellies = new SDate(28, "summer");
                    var stardewValleyFair = new SDate(16, "fall");
                    var spiritsEve = new SDate(27, "fall");
                    var festivalOfIce = new SDate(8, "winter");
                    var feastOfWinterStar = new SDate(25, "winter");



                    if (currentDate == eggFestival && numPlayers >= 0) /// REMEMBER TO CHANGE BACK TO ONE AFTER TESTING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {
                        EggFestival();
                    }

                    else if (currentDate == flowerDance && numPlayers >= 0) /// REMEMBER TO CHANGE BACK TO ONE AFTER TESTING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {
                        FlowerDance();
                    }
                    
                    else if (numPlayers >= 0)  /// REMEMBER TO CHANGE BACK TO ONE AFTER TESTING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {
                        GoToBed();
                    }
                    gameClockTicks = 0;




                    void EggFestival()
                    {
                        if (currentTime >= 900 && currentTime <= 1400)
                        {
                            //teleports to egg festival
                            Game1.player.team.SetLocalReady("festivalStart", true);
                            Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                            {
                                Game1.exitActiveMenu();
                                Game1.warpFarmer("Town", 1, 20, 1);
                            }), (ConfirmationDialog.behavior)null);

                            eggHuntAvailable = true;

                        }
                        else if (currentTime >= 2210)
                        {
                            GoToBed();
                        }
                    }

                    void FlowerDance()
                    {
                        if (currentTime >= 910 && currentTime <= 1400)
                        {

                            Game1.player.team.SetLocalReady("festivalStart", true);
                            Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                            {
                                Game1.exitActiveMenu();
                                Game1.warpFarmer("Forest", 1, 20, 1);
                            }), (ConfirmationDialog.behavior)null);
                            // TODO trigger the dance

                        }
                        else if (currentTime >= 2210)
                        {
                            GoToBed();
                        }
                    }


            }   
                
  
        }

        private void GoToBed()
        {
            Game1.warpFarmer("Farmhouse", 9, 9, false);
            this.Helper.Reflection.GetMethod(Game1.currentLocation, "startSleep").Invoke();

        }   
        


        // shipping menu"OK" click through code
        private void Shipping_Menu(object sender, EventArgs e)
        {
            if (IsEnabled == false) // server toggle
                return;

            this.Monitor.Log("This is the Shipping Menu");
            this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "okClicked").Invoke();
        }

    }
}
