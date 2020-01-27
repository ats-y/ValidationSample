using System;
using Prism.Navigation;
using Reactive.Bindings;

namespace ValidationSample.ViewModels
{
    public class MainPageViewModel
    {
        public ReactiveCommand NavigateCommand { get; } = new ReactiveCommand();

        private INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigateCommand.Subscribe(x =>
            {
                string pageName = x as string;
                _navigationService.NavigateAsync(pageName);
            });
        }
    }
}
