﻿using System;
using System.Threading.Tasks;
using NextGen.BiddingPlatform.Views;
using Xamarin.Forms;

namespace NextGen.BiddingPlatform.Services.Navigation
{
    public interface INavigationService
    {
        Task InitializeAsync();

        Task SetMainPage<TView>(object navigationParameter = null, bool clearNavigationHistory = false)
            where TView : IXamarinView;

        Task SetDetailPageAsync(Type viewType, object navigationParameter = null, bool pushToStack = false);

        Task<Page> GoBackAsync();
    }
}