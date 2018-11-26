#region  References
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;
#endregion

namespace ReactiveAdaptiveCardsBot
{
    public class ReactiveAdaptiveCardsBot:IBot
    {
        private readonly ReactiveAdaptiveCardsBotAccessors _accessors;

        private readonly DialogSet _dialogs;

         public ReactiveAdaptiveCardsBot(ReactiveAdaptiveCardsBotAccessors accessors)
        {
                _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));

                _dialogs = new DialogSet(_accessors.ConversationDialogState);
                
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken= default(CancellationToken))
        {
            DialogContext dc = null;
            string adaptiveCard = string.Empty;

            switch(turnContext.Activity.Type)
            {
                case ActivityTypes.ConversationUpdate:
                    foreach(var member in turnContext.Activity.MembersAdded)
                    {
                        if(member.Id != turnContext.Activity.Recipient.Id)
                        {
                            adaptiveCard = File.ReadAllText(@".\wwwroot\ColorSelector.json");
                            var reply = turnContext.Activity.CreateReply();
                            reply.Attachments = new List<Attachment>()
                            {
                                new Attachment()
                                {
                                    ContentType = "application/vnd.microsoft.card.adaptive",
                                    Content = JsonConvert.DeserializeObject(adaptiveCard)
                                }
                            };
                            
                            //adaptiveCardPath = string.Format(Constants.AdaptiveCardPath, Constants.AdaptiveCards.WelcomeMessage.ToString());
                            await turnContext.SendActivityAsync(reply, cancellationToken:cancellationToken);
                            
                        }
                    }
                    break;

                case ActivityTypes.Message:
                    
                    var token = JToken.Parse(turnContext.Activity.ChannelData.ToString());
                    string selectedcolor = string.Empty;
                    if(System.Convert.ToBoolean(token["postback"].Value<string>()))
                    {
                        JToken commandToken = JToken.Parse(turnContext.Activity.Value.ToString());
                        string command = commandToken["action"].Value<string>();

                        if(command.ToLowerInvariant() == "colorselector")
                        {
                            selectedcolor = commandToken["choiceset"].Value<string>();
                        }
                        
                    }

                    await turnContext.SendActivityAsync($"You Selected {selectedcolor}", cancellationToken: cancellationToken);

                    break;

            }

            

            
        }
    }
}