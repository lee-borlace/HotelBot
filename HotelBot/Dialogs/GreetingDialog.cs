using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace HotelBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        /// <summary>
        /// Called when dialog is first triggered.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi I'm John Bot");

            // Bot always needs somewhere to go next.
            context.Wait(MessageReceivedAsync);
        }

        /// <summary>
        /// Called for subsequent messages in the dialog.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var userName = string.Empty;


            var needToGetName = false;


            // Try to get name from property bag.
            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("NeedToGetName", out needToGetName);

            if (needToGetName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("NeedToGetName", false);
            }

            // If we don't have it already, ask for it.
            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("What is your name please?");

                // Record in state that we need to get the name. The next iteration will pick this up and run with it.
                context.UserData.SetValue<bool>("NeedToGetName", true);
            }
            else
            {
                await context.PostAsync($"Hi {userName}, how can I help you today?");
            }

            // Bot always needs somewhere to go next.
            context.Wait(MessageReceivedAsync);

        }
    } 
}