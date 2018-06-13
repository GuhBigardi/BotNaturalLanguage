using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BotNaturalLanguage.Dialogs
{
    [Serializable]
    public class FutebolDialog : QnAMakerDialog 
    {
        public FutebolDialog() : base(GetNewService()){}
        private static IQnAService[] GetNewService()
        {
            var subscriptionKey = ConfigurationManager.AppSettings.Get("QnaSubscriptionKey");
            var knowledgebaseid = ConfigurationManager.AppSettings.Get("QnaKnowledgebaseId");
            var defaultMessage = "Buguei aqui, pera!  ¯＼(º_o)/¯";
            var qnaModel = new QnAMakerAttribute(subscriptionKey, knowledgebaseid, defaultMessage);
            return new IQnAService[] { new QnAMakerService(qnaModel) };
        }

    }
}