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
using Microsoft.EntityFrameworkCore.Design;


namespace Test_1.Handlers.Dialogue.Steps
{
    class TextStep : DialogueStepBase
    {
        private  IDialogueStep _nextStep;

        private readonly int? _minLength;
        private readonly int? _maxLength;

        public TextStep(
            string content,
            IDialogueStep nextStep,
            int? minLength = null,
            int? maxlegth = null) : base(content)
        {
            _nextStep = nextStep;
            _minLength = minLength;
            _maxLength = maxlegth;
        }

        public Action<string> OnValidResult { get; set; } = delegate { };

        public override IDialogueStep NextStep => _nextStep;

        public void SetNextStep(IDialogueStep nextstep)
        {
            _nextStep = nextstep;
        }

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Please Respond Below",
                Description = $"{user.Mention}, {_content}",
            };

            embedBuilder.AddField("To Stop The dialogue", "Use the ?cancel command");

            if(_minLength.HasValue)
            {
                embedBuilder.AddField("min length:", $"{_minLength.Value} characters");
            }
            if (_maxLength.HasValue)
            {
                embedBuilder.AddField("min length:", $"{_maxLength.Value} characters");
            }

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

                OnMessageAdded(embed);

                var messageResult = await interactivity.WaitForMessageAsync(
                    x => x.ChannelId == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                OnMessageAdded(messageResult.Result);

                if (messageResult.Result.Content.Equals("?cancel", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (_minLength.HasValue)
                {
                    if (messageResult.Result.Content.Length < _minLength.Value)
                    {
                        await TryAgain(channel, $"your input is {_minLength.Value - messageResult.Result.Content.Length} characters too short").ConfigureAwait(false);
                        continue;
                    }
                }
                if (_maxLength.HasValue)
                {
                    if (messageResult.Result.Content.Length < _maxLength.Value)
                    {
                        await TryAgain(channel, $"your input is {_maxLength.Value - messageResult.Result.Content.Length} characters too long").ConfigureAwait(false);
                        continue;
                    }
                }

                OnValidResult(messageResult.Result.Content);
                return false;
            }
        }

    }
}
