using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotNaturalLanguage.Dialogs

{
    [Serializable]
    public class SurveyDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Oiii, esse é o survey dialog. Estou interrompendo seu dialogo. escreva \"done\" para continuar");

            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if ((await result).Text == "done")
            {
                await context.PostAsync("Otimo, voltou para a conversa original!");
                context.Done(String.Empty); //Finish this dialog
            }
            else
            {
                await context.PostAsync("Ainda estou aqui no survey digite \"done\" para continuar");
                context.Wait(MessageReceivedAsync); //Not done yet
            }
        }
    }
}