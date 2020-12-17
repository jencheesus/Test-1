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
    class IntStep : DialogueStepBase
    {
        private readonly IDialogueStep _nextStep;
        private readonly int? _minValue;
        private readonly int? _maxValue;

        public IntStep(
            string content,
            IDialogueStep nextStep,
            int? minValue = null,
            int? maxValue = null) : base(content)
        {
            _nextStep = nextStep;
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public Action<int> OnValidResult { get; set; } = delegate { };

        public override IDialogueStep NextStep => _nextStep;

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Please Respond Below",
                Description = $"{user.Mention}, {_content}",
            };

            embedBuilder.AddField("To Stop The dialogue", "Use the ?cancel command");

            if(_minValue.HasValue)
            {
                embedBuilder.AddField("min Value:", $"{_minValue.Value} characters");
            }
            if (_maxValue.HasValue)
            {
                embedBuilder.AddField("max Value:", $"{_maxValue.Value} characters");
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

                if (!int.TryParse(messageResult.Result.Content, out int inputValue))
                {
                    await TryAgain(channel, $"your input is not an integer").ConfigureAwait(false);
                }


                if (_minValue.HasValue)
                {
                    if (inputValue < _minValue.Value)
                    {
                        await TryAgain(channel, $"your value  {_minValue.Value - messageResult.Result.Content.Length} is smaller than main value").ConfigureAwait(false);
                        continue;
                    }
                }
                if (_maxValue.HasValue)
                {
                    if (inputValue < _maxValue.Value)
                    {
                        await TryAgain(channel, $"your input value {_maxValue.Value - messageResult.Result.Content.Length} is bigger than the max value").ConfigureAwait(false);
                        continue;
                    }
                }

                OnValidResult(inputValue);
                return false;
            }
        }

    }
}
