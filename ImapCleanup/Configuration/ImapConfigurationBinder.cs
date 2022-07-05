using System.CommandLine.Binding;
using System.CommandLine;

namespace ImapCleanup.Configuration
{
    public class ImapConfigurationBinder : BinderBase<ImapConfiguration>
    {
        private readonly Option<int> _portOption;
        private readonly Option<string> _hostnameOption;
        private readonly Option<string> _usernameOption;
        private readonly Option<string> _passwordOption;

        public ImapConfigurationBinder(
            Option<int> portOption,
            Option<string> hostnameOption,
            Option<string> usernameOption,
            Option<string> passwordOption)
        {
            _portOption = portOption;
            _hostnameOption = hostnameOption;
            _usernameOption = usernameOption;
            _passwordOption = passwordOption;
        }

        protected override ImapConfiguration GetBoundValue(BindingContext bindingContext)
        {
            return new ImapConfiguration
            {
                Port = bindingContext.ParseResult.GetValueForOption(_portOption),
                Hostname = bindingContext.ParseResult.GetValueForOption(_hostnameOption),
                Username = bindingContext.ParseResult.GetValueForOption(_usernameOption),
                Password = bindingContext.ParseResult.GetValueForOption(_passwordOption)
            };
        }
    }
}
