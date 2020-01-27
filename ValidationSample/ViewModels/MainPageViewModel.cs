using System;
using Prism.Navigation;
using Reactive.Bindings;

namespace ValidationSample.ViewModels
{
    public class MainPageViewModel
    {
        public ReactiveCommand NavigateSingleCommand { get; } = new ReactiveCommand();
        public ReactiveCommand NavigateWithCommand { get; } = new ReactiveCommand();

        private INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigateSingleCommand.Subscribe(_ =>
            {
                _navigationService.NavigateAsync("SimpleValidationPage");
            });

            NavigateWithCommand.Subscribe(_ =>
            {
                _navigationService.NavigateAsync("WithCommandPage");
            });
        }
    }
}
