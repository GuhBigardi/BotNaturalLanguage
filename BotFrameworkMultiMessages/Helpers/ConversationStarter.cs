using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BotFrameworkMultiMessages
{
    public class ConversationStarter
    {
        public static string ConversationReference { get; set; }
        public static string TextReference { get; set; }

        /// <summary>
        /// This will interrupt the conversation and send the user to Dialog, then adding this stack to run and once it's finished, we will be back to the original conversation
        /// </summary>
        /// <param name="dialog" >The dialog that will be instant and will care the messages build Microsoft.Bot.Builder.Dialogs.Dialog`1 
        /// </param>
        /// <returns></returns>
        public static async Task Resume(IDialog<object> dialog)
        {
            var message = JsonConvert.DeserializeObject<ConversationReference>(ConversationReference).GetPostToBotMessage();
            message.Text = TextReference;
            var client = new ConnectorClient(new Uri(message.ServiceUrl));

            using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
            {
                var botData = scope.Resolve<IBotData>();
                await botData.LoadAsync(CancellationToken.None);

                var task = scope.Resolve<IDialogTask>();

                await task.Forward(dialog, null, message, CancellationToken.None);

                await task.PollAsync(CancellationToken.None);
                TextReference = string.Empty;

                await botData.FlushAsync(CancellationToken.None);
            }
        }
    }
}