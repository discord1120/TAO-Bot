using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
namespace TAO_BOT
{
    class Program
    {
        static DiscordSocketClient client;
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }
        private async Task MainAsync()
        {
            client = new DiscordSocketClient();
            client.MessageReceived += Message;
            client.Log += (LogMessage msg) =>
            {
                Console.WriteLine(msg.ToString());
                return Task.CompletedTask;
            };
            await client.LoginAsync(TokenType.Bot, "NjIyODAwNTI3NzAwNTI1MDc0.XX7TrA.2DLHl5nyfZocBx1wO1krPPZamLY");
            await client.StartAsync();
            await Task.Delay(-1);
        }
        static ulong noch = 0;
        static ulong nowch = 0;
        static int count = 0;
        private async Task Message(IMessage msg)
        {
            if (msg is IUserMessage message)
            {
                var dt = DateTime.Now;
                if (message.Content == "::atk" && message.Author.Id == 622800527700525074)
                {
                    Console.WriteLine($"{dt.ToString("H:mm:ss")} Botが攻撃しました");
                    nowch = message.Channel.Id;
                }
                if (message.Content == ".atk")
                {
                    await message.Channel.SendMessageAsync("::atk");
                }
                if (message.Content == ".st")
                {
                    await message.Channel.SendMessageAsync("::st");
                    Console.WriteLine($"{dt.ToString("H:mm:ss")} Botがステータスを表示しました");
                }
                if (message.Author.Id != 526620171658330112)
                    return;
                if (message.Content == "`攻撃失敗。ゆっくりコマンドを打ってね。`")
                {
                    Console.WriteLine($"{dt.ToString("H:mm:ss")} Botが攻撃に失敗しました");
                    if (count == 0 || count == 1)
                    {
                        count += 1;
                        await Task.Delay(new Random().Next(1000));
                        await message.Channel.SendMessageAsync("::atk");
                    }
                    else
                    {
                        count = 0;
                        return;
                    }
                }
                if (message.Content.Contains("エラー"))
                {
                    await message.Channel.SendMessageAsync("::atk");
                }
                if (message.Channel.Id == 623005176428625940)
                {
                    var ch = client.GetGuild(622947842494955522).GetTextChannel(nowch);
                    Console.WriteLine($"{dt.ToString("H:mm:ss")} TAOが再起動しました");
                    await ch.SendMessageAsync("::atk");
                }
                if (message.Embeds.FirstOrDefault() != null)
                {
                    var embed = message.Embeds.FirstOrDefault();
                    if (embed.Title != null)
                    {
                        if (embed.Title == "戦闘結果")
                            return;
                        if (noch != message.Channel.Id && embed.Title.Contains("待ち構えている"))
                        {
                            await message.Channel.SendMessageAsync("::atk");
                        }
                    }
                    if (embed.Description != null)
                    {
                        if (embed.Description.Contains("ゲームにログインしてね"))
                        {
                            await message.Channel.SendMessageAsync("::login");
                        }
                        if (embed.Description.Contains("ログボをゲットしました！！！"))
                        {
                            await message.Channel.SendMessageAsync("::atk");
                        }
                        if (embed.Description.Contains("仲間になりたそうに"))
                        {
                            await Task.Delay(1000);
                            await message.AddReactionAsync(new Emoji("👍"));
                            Console.WriteLine($"{dt.ToString("H:mm:ss")} Botが{embed.Description.Substring(0, embed.Description.IndexOf("が"))}を仲間にしました");
                        }
                        if (embed.Description.Contains("何処かで戦闘中"))
                        {
                            await message.Channel.SendMessageAsync("::atk");
                        }
                    }
                }
                if (!message.Content.Contains("HP"))
                {
                    count = 0;
                    return;
                }
                if (message.Content.Contains("T A OのHP:0"))
                {
                    await message.Channel.SendMessageAsync("::re");
                    Console.WriteLine($"{dt.ToString("H:mm:ss")} Botがリセットしました");
                    await (message.Channel as IGuildChannel).DeleteAsync();
                    var ch = await (message.Channel as IGuildChannel).Guild.CreateTextChannelAsync("tao");
                    await ch.SendMessageAsync("::atk");
                    noch = message.Channel.Id;
                    nowch = ch.Id;
                }
                else
                {
                    await message.Channel.SendMessageAsync("::atk");
                }
            }
        }
    }
}
