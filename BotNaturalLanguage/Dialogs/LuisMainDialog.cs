using BotNaturalLanguage.Auxiliares;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace BotNaturalLanguage.Dialogs
{
    [Serializable]
    public class LuisMainDialog : BaseLuisDialog<object>
    {

        string qnaSubscriptionKey = ConfigurationManager.AppSettings["QnaSubscriptionKey"];
        string qnaKnowledgebaseId = ConfigurationManager.AppSettings["QnaKnowledgebaseId"];
        [NonSerialized]
        IMessageActivity activity;
        public override Task StartAsync(IDialogContext context)
        {
            activity = (IMessageActivity)context.Activity;
            return base.StartAsync(context);
        }

        [LuisIntent("Cumprimento")]
        public async Task Cumprimento(IDialogContext context, LuisResult result)
        {
            ConversationStarter.textReference = string.Empty;
            await context.PostAsync($"Texto enviado: {result.Query}");
            var qnaService = new QnAMakerService(new QnAMakerAttribute(qnaSubscriptionKey, qnaKnowledgebaseId, "Buguei aqui, pera!  ¯＼(º_o)/¯"));
            var qnaMaker = new QnAMakerDialog(qnaService);
            await qnaMaker.MessageReceivedAsync(context, Awaitable.FromItem(activity));
        }
        private async Task AfterQnaDialog(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var messageHandled = await result;
            context.Wait(MessageReceived);
        }
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            ConversationStarter.textReference = string.Empty;
            await context.PostAsync($"Eita, não consegui entender a frase {result.Query}");
        }
    }
}