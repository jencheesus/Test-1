using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus;

namespace Test_1.Handlers.Dialogue.Steps
{
    public class ReactionStep : DialogueStepBase
    {

        private readonly Dictionary<DiscordEmoji, ReactionStepData> _options;

        private DiscordEmoji _selectedEmoji;

        public ReactionStep(string content, Dictionary<DiscordEmoji, ReactionStepData> options) : base(content)
        {
            _options = options;
        }

        public override IDialogueStep NextStep => _options[_selectedEmoji].NextStep;

        public Action<DiscordEmoji> OnValidResult { get; set; } = delegate { };

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var cancelEmoji = DiscordEmoji.FromName(client, ":x:");

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Please React To This Embed",
                Description = $"{user.Mention}, {_content}",
                Color = DiscordColor.Lilac,
            };

            embedBuilder.AddField("to Stop The Dialogue", "React with the :x: Emoji To Cacel!");

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

                OnMessageAdded(embed);

                foreach (var emoji in _options.Keys)
                {
                    await embed.CreateReactionAsync(emoji).ConfigureAwait(false);
                }

                await embed.CreateReactionAsync(cancelEmoji).ConfigureAwait(false);

                var ReactionResult = await interactivity.WaitForReactionAsync(x => _options.ContainsKey(x.Emoji) || x.Emoji == cancelEmoji, embed, user).ConfigureAwait(false);

                if (ReactionResult.Result.Emoji == cancelEmoji)
                {
                    return true;
                }

                _selectedEmoji = ReactionResult.Result.Emoji;

                OnValidResult(_selectedEmoji);

                return false;
            }
        }
    }

    public class ReactionStepData
    {
        public string Content { get; set; }
        public IDialogueStep NextStep { get; set; }
    }
}
