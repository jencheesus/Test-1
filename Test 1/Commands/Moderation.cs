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
    public class Moderation :BaseCommandModule
    {

        [Command("ban")]
        [Description("You can people, EMBRYO MOD AND HIGHER ROLE!")]
        [RequireRoles(RoleCheckMode.Any, "Embryo Mod", "Admin")]
        public async Task Ban(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("lol you thought i would be smart enough to do that bwahahaha - no <3");
            await ctx.Channel.SendMessageAsync("Banned JenniferLouisCheesus#0706");
            await ctx.Channel.SendMessageAsync("pfff - jk");
        }

        [Command("unban")]
        [Description("You can people, EMBRYO MOD AND HIGHER ROLE!")]
        [RequireRoles(RoleCheckMode.Any, "Embryo Mod", "Admin")]
        public async Task unBan(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("why would you unban her smh my guacaomole falvoured shren");
            await ctx.Channel.SendMessageAsync("but yeah - most of the commands are broken till i gorw a brain - Jen <3");
        }

        [Command("kick")]
        [Description("you can kick people- well when it works, and if you have a mod role <3")]
        [RequireRoles(RoleCheckMode.Any, "mod", "Embryo Mod", "Admin")]
        public async Task kick(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("yeet - good bye, again another command that wont work :D");
        }
    }
}
