using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using System.Linq;

namespace Test_1.Commands
{
    public class PollCommands : BaseCommandModule
    {
        [Command("poll")]
        [Description("poll for voting for stuff :D - BIG NOTE AND WARNING! you can only poll emojis and the emojis will be reactions")]
        public async Task poll(CommandContext ctx, TimeSpan duration,  params DiscordEmoji[] emojiOptions )
        {
            var interactivity = ctx.Client.GetInteractivity();
            var options = emojiOptions.Select(x => x.ToString());

            var embed = new DiscordEmbedBuilder
            {
                Title = "Poll",
                Description = string.Join(" ", options),               
            };

            var pollMessage = await ctx.Channel.SendMessageAsync(embed : embed).ConfigureAwait(false);

            foreach(var option in emojiOptions)
            {
                await pollMessage.CreateReactionAsync(option).ConfigureAwait(false);
            }

            var result = await interactivity.CollectReactionsAsync(pollMessage, duration).ConfigureAwait(false);
            var distinctResult = result.Distinct();

            var results = distinctResult.Select(x => $"{x.Emoji}: {x.Total}");

            await ctx.Channel.SendMessageAsync(string.Join("\n", results)).ConfigureAwait(false);

            // NEED TO MAKE IT SO THAT YOU PUT SMTH AND THEN IT CHOOSES OPTIONS
        }
    }
}
