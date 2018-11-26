#region References
using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
#endregion

namespace ReactiveAdaptiveCardsBot
{
    public class ReactiveAdaptiveCardsBotAccessors
    {
        public ReactiveAdaptiveCardsBotAccessors(ConversationState conversationState, UserState userState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(ConversationState));
            UserState = userState ?? throw new ArgumentNullException(nameof(UserState));
        }

        public static readonly string DialogStateName = $"{nameof(ReactiveAdaptiveCardsBotAccessors)}.DialogState";

        public static readonly string CommandStateName = $"{nameof(ReactiveAdaptiveCardsBotAccessors)}.CommandState";

        
        public IStatePropertyAccessor<DialogState> ConversationDialogState { get; set; }

        public IStatePropertyAccessor<string> CommandState { get; set; }

        public ConversationState ConversationState { get; }

        public UserState UserState { get; }
    }
}