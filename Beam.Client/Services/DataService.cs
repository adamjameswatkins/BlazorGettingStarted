﻿using Beam.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beam.Client.Services
{
    public class DataService : IDataService
    {
        public IReadOnlyList<Frequency> Frequencies { get; private set; }
        private IReadOnlyList<Ray> rays = new List<Ray>();
        public IReadOnlyList<Ray> Rays
        {
            get => rays;
            private set => rays = value.OrderByDescending(r => r.RayId).ToList();
        }

        public User CurrentUser { get; set; }

        private int? selectedFrequency;
        private readonly IBeamApiService apiService;

        private readonly NavigationManager navigationManager;

        public int SelectedFrequency
        {
            get
            {
                if (!selectedFrequency.HasValue && Frequencies.Count > 0)
                {
                    selectedFrequency = Frequencies.First().Id;
                }
                return selectedFrequency ?? 0;
            }
            set
            {
                selectedFrequency = value;
                GetRays(value).ConfigureAwait(false);
            }
        }

        public DataService(IBeamApiService apiService, NavigationManager navigationManager)
        {
            this.apiService = apiService;
            this.navigationManager = navigationManager;
            if (CurrentUser == null) CurrentUser = new User() { Name = "Anon" + new Random().Next(0, 10) };
        }

        public event Action UdpatedFrequencies;
        public event Action UpdatedRays;

        public async Task GetFrequencies()
        {
            Frequencies = await this.apiService.FrequencyList();
            UdpatedFrequencies?.Invoke();
        }

        public async Task GetRays(int FrequencyId)
        {
            Rays = new List<Ray>();
            Rays = await this.apiService.RayList(FrequencyId);
            UpdatedRays?.Invoke();
        }

        public async Task<List<Ray>> GetUserRays(string name)
        {
            return await this.apiService.UserRays(name ?? CurrentUser.Name);
        }

        public async Task AddFrequency(string Name)
        {
            Frequencies = await this.apiService.AddFrequency(new Frequency() { Name = Name });
            UdpatedFrequencies?.Invoke();
        }

        public async Task CreateRay(string text)
        {
            var ray = new Ray()
            {
                FrequencyId = selectedFrequency.Value,
                Text = text,
                UserId = CurrentUser.Id
            };

            if (CurrentUser.Id == 0)
            {
                navigationManager.NavigateTo("/login");
                ray.UserId = CurrentUser.Id;
            }

            Rays = await this.apiService.AddRay(ray);
            UpdatedRays?.Invoke();
        }

        public async Task PrismRay(int RayId)
        {
            if (CurrentUser.Id == 0) navigationManager.NavigateTo("/login");
            Rays = await this.apiService.PrismRay(new Prism() { RayId = RayId, UserId = CurrentUser.Id });
            UpdatedRays?.Invoke();
        }

        public async Task UnPrismRay(int RayId)
        {
            if (CurrentUser.Id == 0) navigationManager.NavigateTo("/login");
            Rays = await this.apiService.UnPrismRay(RayId, CurrentUser.Id);
            UpdatedRays?.Invoke();
        }
    }
}
