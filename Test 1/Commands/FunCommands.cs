using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Test_1.Handlers.Dialogue;
using Test_1.Handlers.Dialogue.Steps;
using Test_1.Attributes;

namespace Test_1.Commands
{
    public class FunCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("returns pong")]
        [RequireRoles(RoleCheckMode.Any, "Admin", "Embryo Mod")]
        // require attributes - still not working aaaaaaaaa
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong!").ConfigureAwait(false); //just sends pong back
        }

        [Command("add")]
        [Description("adds two numbers together")]
        public async Task Add(CommandContext ctx,[Description("first number")] int NumOne,[Description("second number")]int NumTwo)
        {
            await ctx.Channel.SendMessageAsync((NumOne + NumTwo).ToString()).ConfigureAwait(false); // adds two numbers then displays it in the channel
        }

        [Command("respond")]
        [Description("bot waits for a message and then responds with it")]
        public async Task ResponsdMessage(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content);

            //sends a message back in the channel after you put smth after repond (need to check)
        }

        [Command("respondreaction")]
        [Description("bot waits for a emoji reaction and then responds with it")]
        public async Task ResponsdReaction(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForReactionAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Emoji);


        }

        [Command("echo")]
        [Description("Hennifer will invite you to her dms, and repeat what you say in the channel where the ocmmand was used :D")]
        public async Task Dialogue(CommandContext ctx)
        {
            string input = string.Empty;

            var inputStep = new TextStep("Enter Something interesting", null, minLength : 10);
            var funnyStep = new TextStep("you think you are funny pfff", null, maxlegth : 100);

            inputStep.OnValidResult += (result) =>
            {
                input = result;
                if (result == "something interesting")
                {
                    inputStep.SetNextStep(funnyStep);
                }
            };

            var UserChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                UserChannel,
                ctx.User,
                inputStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded)
            {
                return;
            }

            await ctx.Channel.SendMessageAsync(input).ConfigureAwait(false);
           
        }

        [Command("emojidialogue")]
        [Description("Ill think of how to explain this later lol")]
        public async Task EmojiDialohue(CommandContext ctx)
        {
            var YesStep = new TextStep("You chose Yes!", null);
            var NoStep = new TextStep("You Chose No!", null);


            var emojiStep = new ReactionStep("Yes Or No?", new Dictionary<DiscordEmoji, ReactionStepData>
            {
                {DiscordEmoji.FromName(ctx.Client, ":thumbsup:"), new ReactionStepData {Content = "this means yes", NextStep = null} },
                {DiscordEmoji.FromName(ctx.Client, ":thumbsdown:"), new ReactionStepData {Content = "this means no", NextStep = null } }
            }) ;
            var userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client, 
                userChannel, 
                ctx.User,
                emojiStep
                );

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded)
            {
                return;
            }

        }
    }
}
