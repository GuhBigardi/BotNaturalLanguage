using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BotFrameworkMultiMessages
{
    [Serializable]
    public class MultiMessageDialog : IDialog<object>
    {
        [NonSerialized]
        Timer timer;
        [NonSerialized]
        private readonly IDialog<object> dialog;
        [NonSerialized]
        private readonly int secondsToResumeMessage;
        /// <summary>
        /// Dialog to use multi-message module
        /// </summary>
        /// <param name="dialog" >The dialog that will be instant and will care the messages build Microsoft.Bot.Builder.Dialogs.Dialog`1 
        /// </param>
        /// <param name="secondsToResumeMessage"> of seconds that dialog will be wait to process the messages received
        /// </param>Quantity 
        public MultiMessageDialog(IDialog<object> dialog, int secondsToResumeMessage)
        {
            timer = new Timer(new TimerCallback(TimerEvent));
            timer.Change(1000, 1000);
            this.dialog = dialog;
            this.secondsToResumeMessage = secondsToResumeMessage;
        }
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var conversationReference = message.ToConversationReference();
            ConversationStarter.ConversationReference = JsonConvert.SerializeObject(conversationReference);
            ConversationStarter.TextReference += $" {message.Text}";
            Contador.Count = 0;
            context.Wait(MessageReceivedAsync);
        }
        private void TimerEvent(object target)
        {
            Contador.Count++;
            if (Contador.Count == secondsToResumeMessage)
                ConversationStarter.Resume(dialog);
        }
    }
}