using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buffs;

namespace MirandaMod
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig Config;

        float WalkSpeed;

        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            WalkSpeed = this.Config.WalkSpeed;

            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Input.ButtonReleased += this.OnButtonReleased;
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e) 
        {
            Game1.addHUDMessage(new HUDMessage("Good morning I love you", 1));
        }

        private Buff CreateSpeedBuff()
        {
            Texture2D iconTexture = this.Helper.ModContent.Load<Texture2D>("assets/zoom.png");

            Buff SpeedBuff = new Buff(
                id: "MirandaMod.ZoomZoom",
                displayName: "Zoomies",
                description: "Zoom Zoom!",
                iconTexture: iconTexture,
                iconSheetIndex: 0,
                duration: -2,
                effects: new BuffEffects()
                {
                    Speed = { WalkSpeed }
                }
);
            return SpeedBuff;
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // Apply Speed Buff
            if (e.Button == SButton.ControllerB || e.Button == SButton.B)
            {
                Buff SpeedBuff = CreateSpeedBuff();
                Game1.player.applyBuff(SpeedBuff);
            }

            // print button presses to the console window
            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
        }

        private void OnButtonReleased(object? sender, ButtonReleasedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // Apply Speed Buff
            if (e.Button == SButton.ControllerB || e.Button == SButton.B)
            {
                Game1.player.buffs.Remove("MirandaMod.ZoomZoom");
            }
        }
    }
}
