using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CB.Xaml.Behaviors.Impl;


namespace CB.Xaml.Behaviors
{
    public class AutoSuggestTextBehavior: SuggestTextBehaviorBase
    {
        protected override string[] GetSuggestionSource()
        {
            return Regex.Split(AssociatedObject.Text, @"\s+").Distinct().ToArray();
        }
    }
}