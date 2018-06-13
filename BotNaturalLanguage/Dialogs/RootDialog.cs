using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using BotNaturalLanguage.Auxiliares;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace BotNaturalLanguage.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        [NonSerialized]
        Timer t;
        string messagecompleted = string.Empty;
        public RootDialog()
        {
            t = new Timer(new TimerCallback(TimerEvent));
            t.Change(1000, 1000);
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
            ConversationStarter.conversationReference = JsonConvert.SerializeObject(conversationReference);
            ConversationStarter.textReference += $" {message.Text}";
            Contador.Count = 0;
            context.Wait(MessageReceivedAsync);
        }
        public void TimerEvent(object target)
        {
            Contador.Count++;
            if (Contador.Count == 5)
            {
                ConversationStarter.Resume();
            }
        }
    }
}