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

// setup GUI
// lock out player controls?
// Game1.freezeControls = true;  to freeze input?
// make host invisible?
// set up config file to handle length of festivals
// add little prompts to let players know whats happening to server guy?
// config file for length of festivals.
// find the actual values of the left click code for clicking on the cancel on sleep dialogue as a method for allowing sleep during the day of late night festivals. ie "cancel the sleep menu">"goto festival"
// Game1.chatBox.activate();
// Game1.chatBox.setText("Hello There");
// need third line to actually send messages what could it be?


namespace test
{


    public class ModEntry : Mod
    {








        private int gameTicks; //stores 1s game ticks for pause code
        private int gameClockTicks; //stores in game clock change
        private int numPlayers = 0; //stores number of players
        private bool IsEnabled = false;  //stores if the the server mod is enabled 
        private readonly Dictionary<string, int> PreviousFriendships = new Dictionary<string, int>();  //stores friendship values
        private bool eggHuntAvailable = false; //is egg festival ready start timer for triggering eggHunt Event
        private int eggHuntCountDown; //to trigger egghunt after set time
        private bool flowerDanceAvailable = false;
        private int flowerDanceCountDown;
        private bool luauSoupAvailable = false;
        private int luauSoupCountDown;
        private bool jellyDanceAvailable = false;
        private int jellyDanceCountDown;
        private bool grangeDisplayAvailable = false;
        private int grangeDisplayCountDown;
        private bool goldenPumpkinAvailable = false;
        private int goldenPumpkinCountDown;
        private bool iceFishingAvailable = false;
        private int iceFishingCountDown;
        private bool winterFeastAvailable = false;
        private int winterFeastCountDown;








        public override void Entry(IModHelper helper)
        {

            helper.ConsoleCommands.Add("server", "Toggles headless server on/off", this.ServerToggle);

            SaveEvents.BeforeSave += this.Shipping_Menu; // Shipping Menu handler
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

            NoClientsPause();

            //left click menu spammer to get through random events happening
            if (IsEnabled == true) // server toggle
            {
                if (Game1.activeClickableMenu != null)
                {
                    this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "receiveLeftClick").Invoke(10, 10, true);
                }
            }

            

            //disable friendship decay
            if (IsEnabled == true) // server toggle
            {
                if (this.PreviousFriendships.Any())
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

            //eggHunt event
            if (eggHuntAvailable == true && Game1.CurrentEvent != null && Game1.CurrentEvent.isFestival)
            {
                eggHuntCountDown += 1;

                if (eggHuntCountDown == 240)  //4 minutes to enjoy fetival before egghunt starts - need to make config file.
                {
                    this.Helper.Reflection.GetMethod(Game1.CurrentEvent, "answerDialogueQuestion", true).Invoke(Game1.getCharacterFromName("Lewis"), "yes"); //trigger eggHunt Scene, 
                }
                if (eggHuntCountDown >= 245) //have to adjust this value with config file as well.
                {
                    if (Game1.activeClickableMenu != null)
                    {
                        this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "receiveLeftClick").Invoke(10, 10, true);
                    }
                }
            }

            //flowerDance event
            if (flowerDanceAvailable == true && Game1.CurrentEvent != null && Game1.CurrentEvent.isFestival)
            {
                flowerDanceCountDown += 1;

                if (flowerDanceCountDown == 240)  
                {
                    this.Helper.Reflection.GetMethod(Game1.CurrentEvent, "answerDialogueQuestion", true).Invoke(Game1.getCharacterFromName("Lewis"), "yes"); //trigger flower dance, 
                }
                if (flowerDanceCountDown >= 245)
                {
                    if (Game1.activeClickableMenu != null)
                    {
                        this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "receiveLeftClick").Invoke(10, 10, true);
                    }
                }
            }

            //luauSoup event
            if (luauSoupAvailable == true && Game1.CurrentEvent != null && Game1.CurrentEvent.isFestival)
            {
                luauSoupCountDown += 1;

                if (luauSoupCountDown == 240)  
                {
                    this.Helper.Reflection.GetMethod(Game1.CurrentEvent, "answerDialogueQuestion", true).Invoke(Game1.getCharacterFromName("Lewis"), "yes"); //trigger flower dance, 
                }
                if (luauSoupCountDown >= 245) 
                {
                    if (Game1.activeClickableMenu != null)
                    {
                        this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "receiveLeftClick").Invoke(10, 10, true);
                    }
                }
            }

            //Dance of the Moonlight Jellies event
            if (jellyDanceAvailable == true && Game1.CurrentEvent != null && Game1.CurrentEvent.isFestival)
            {
                jellyDanceCountDown += 1;

                if (jellyDanceCountDown == 240)  
                {
                    this.Helper.Reflection.GetMethod(Game1.CurrentEvent, "answerDialogueQuestion", true).Invoke(Game1.getCharacterFromName("Lewis"), "yes"); //trigger flower dance, 
                }
                if (jellyDanceCountDown >= 245) 
                {
                    if (Game1.activeClickableMenu != null)
                    {
                        this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "receiveLeftClick").Invoke(10, 10, true);
                    }
                }
            }

            //Grange Display event
            if (grangeDisplayAvailable == true && Game1.CurrentEvent != null && Game1.CurrentEvent.isFestival)
            {
                grangeDisplayCountDown += 1;

                if (grangeDisplayCountDown == 240)  
                {
                    this.Helper.Reflection.GetMethod(Game1.CurrentEvent, "answerDialogueQuestion", true).Invoke(Game1.getCharacterFromName("Lewis"), "yes"); //trigger flower dance, 
                }
                if (grangeDisplayCountDown == 245) 
                {
                    Game1.player.team.SetLocalReady("festivalEnd", true);
                    Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalEnd", true, (ConfirmationDialog.behavior)(who =>
                    {
                        Game1.exitActiveMenu();
                        Game1.warpFarmer("Farmhouse", 9, 9, false);
                        Game1.timeOfDay = 2200;
                    }), (ConfirmationDialog.behavior)null);
                }
            }

            //golden pumpkin maze event
            if (goldenPumpkinAvailable == true && Game1.CurrentEvent != null && Game1.CurrentEvent.isFestival)
            {
                goldenPumpkinCountDown += 1;

                if (goldenPumpkinCountDown == 10) 
                {
                    Game1.player.team.SetLocalReady("festivalEnd", true);
                    Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalEnd", true, (ConfirmationDialog.behavior)(who =>
                    {
                        Game1.exitActiveMenu();
                        Game1.warpFarmer("Farmhouse", 9, 9, false);
                        Game1.timeOfDay = 2200;
                    }), (ConfirmationDialog.behavior)null);
                }
            }

            //ice fishing event
            if (iceFishingAvailable == true && Game1.CurrentEvent != null && Game1.CurrentEvent.isFestival)
            {
                iceFishingCountDown += 1;

                if (iceFishingCountDown == 240)  //4 minutes to enjoy fetival before egghunt starts - need to make config file.
                {
                    this.Helper.Reflection.GetMethod(Game1.CurrentEvent, "answerDialogueQuestion", true).Invoke(Game1.getCharacterFromName("Lewis"), "yes"); //trigger eggHunt Scene, 
                }
                if (iceFishingCountDown >= 245)
                { 
                    if (Game1.activeClickableMenu != null)
                    {
                        this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "receiveLeftClick").Invoke(10, 10, true);
                    }
                }
            }

            //Feast of the Winter event
            if (winterFeastAvailable == true && Game1.CurrentEvent != null && Game1.CurrentEvent.isFestival)
            {
                winterFeastCountDown += 1;

                if (winterFeastCountDown == 10)
                {
                    Game1.player.team.SetLocalReady("festivalEnd", true);
                    Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalEnd", true, (ConfirmationDialog.behavior)(who =>
                    {
                        Game1.exitActiveMenu();
                        Game1.warpFarmer("Farmhouse", 9, 9, false);
                        Game1.timeOfDay = 2200;
                    }), (ConfirmationDialog.behavior)null);
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



                if (currentDate == eggFestival && numPlayers >= 1) 
                {
                    EggFestival();
                }

                else if (currentDate == flowerDance && numPlayers >= 1) 
                {
                    FlowerDance();
                }

                else if (currentDate == luau && numPlayers >= 1) 
                {
                    Luau();
                }

                else if (currentDate == danceOfJellies && numPlayers >= 1) 
                {
                    DanceOfTheMoonlightJellies();
                }

                else if (currentDate == stardewValleyFair && numPlayers >= 1) 
                {
                    StardewValleyFair();
                }

                else if (currentDate == spiritsEve && numPlayers >= 1) 
                {
                    SpiritsEve();
                }

                else if (currentDate == festivalOfIce && numPlayers >= 1) 
                {
                    FestivalOfIce();
                }

                else if (currentDate == feastOfWinterStar && numPlayers >= 1) 
                {
                    FeastOfWinterStar();
                }

                else if (numPlayers >= 1)  
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
                    else if (currentTime >= 1410)
                    {
                        GoToBed();
                        eggHuntAvailable = false;
                        eggHuntCountDown = 0;
                    }
                }

                void FlowerDance()
                {
                    if (currentTime >= 900 && currentTime <= 1400)
                    {

                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Forest", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);

                        flowerDanceAvailable = true;

                    }
                    else if (currentTime >= 1410)
                    {
                        GoToBed();
                        flowerDanceAvailable = false;
                        flowerDanceCountDown = 0;
                    }
                }

                void Luau()
                {
                    if (currentTime >= 900 && currentTime <= 1400)
                    {

                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Beach", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);

                        luauSoupAvailable = true;

                    }
                    else if (currentTime >= 1410)
                    {
                        GoToBed();
                        luauSoupAvailable = false;
                        luauSoupCountDown = 0;
                    }
                }

                void DanceOfTheMoonlightJellies()
                {

                    /*if (currentTime < 2200)  //triggers weird bug if you try to go to sleep then jump to a festival. Maybe try to fix in future?
                    {
                        GoToBed();
                    }*/

                    if (currentTime >= 2200 && currentTime <= 2400)
                    {

                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Beach", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);

                        jellyDanceAvailable = true;

                    }
                    else if (currentTime >= 2410)
                    {
                        GoToBed();
                        jellyDanceAvailable = false;
                        jellyDanceCountDown = 0;
                    }
                }

                void StardewValleyFair()
                {
                    if (currentTime >= 900 && currentTime <= 1500)
                    {
                        
                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Town", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);

                        grangeDisplayAvailable = true;

                    }
                    else if (currentTime >= 1510)
                    {
                        GoToBed();
                        Game1.displayHUD = true;
                        grangeDisplayAvailable = false;
                        grangeDisplayCountDown = 0;
                    }
                }

                void SpiritsEve()
                {
                    /*if (currentTime < 2200)  //triggers weird bug if you try to go to sleep then jump to a festival. Maybe try to fix in future?
                        {
                            GoToBed();
                        }*/

                    if (currentTime >= 2200 && currentTime <= 2350)
                    {

                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Town", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);

                        goldenPumpkinAvailable = true;

                    }
                    else if (currentTime >= 2400)
                    {
                        GoToBed();
                        Game1.displayHUD = true;
                        goldenPumpkinAvailable = false;
                        goldenPumpkinCountDown = 0;
                    }
                }

                void FestivalOfIce()
                {
                    if (currentTime >= 900 && currentTime <= 1400)
                    {

                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Forest", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);

                        iceFishingAvailable = true;

                    }
                    else if (currentTime >= 1410)
                    {
                        GoToBed();
                        iceFishingAvailable = false;
                        iceFishingCountDown = 0;
                    }
                }

                void FeastOfWinterStar()
                {
                    if (currentTime >= 900 && currentTime <= 1400)
                    {

                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Town", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);

                        winterFeastAvailable = true;

                    }
                    else if (currentTime >= 1410)
                    {
                        GoToBed();
                        winterFeastAvailable = false;
                        winterFeastCountDown = 0;
                    }
                }

            }


        }

        private void GoToBed()
        {
            Game1.displayHUD = true;
            Game1.warpFarmer("Farmhouse", 9, 9, false);
            this.Helper.Reflection.GetMethod(Game1.currentLocation, "startSleep").Invoke();

        }



        // shipping menu"OK" click through code
        private void Shipping_Menu(object sender, EventArgs e)
        {
            if (IsEnabled == false) // server toggle
                return;
            if (Game1.activeClickableMenu != null)
            {
                this.Monitor.Log("This is the Shipping Menu");
                this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "okClicked").Invoke();
            }
        }

    }
}

